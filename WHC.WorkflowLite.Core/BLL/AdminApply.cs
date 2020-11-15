using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using WHC.WorkflowLite.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 业务审批流程管理类
    /// </summary>
    public class AdminApply : BaseBLL<ApplyInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AdminApply()
        {
        }

        #region 单例对象实现
        private static AdminApply adminApply = new AdminApply();

        /// <summary>
        /// 单例对象
        /// </summary>
        public static AdminApply Instance
        {
            get
            {
                return adminApply;
            }
        } 
        #endregion

        /// <summary>
        /// 根据登录用户、表单ID、申请单标题创建所需要的流程数据。
        /// 创建流程前，业务表的数据必须先保存，在执行该流程创建操作，因为逻辑需要调用AppForm的GetApplyCount函数，根据条件过滤不需要的流程。
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单的ID标识，请使用一个不重复的ID值，如:System.Guid.NewGuid().ToString()，并且和业务表单的Apply_ID保持一致。</param>
        /// <param name="formId">表单ID</param>
        /// <param name="applyTitle">申请单标题</param>
        /// <param name="selprocuser">第一步的流程处理人</param>
        /// <param name="remark">备注说明</param>
        /// <returns></returns>
        public bool CreateApply(LoginUserInfo userInfo, string applyId, string formId, string applyTitle, string selprocuser, string remark)
        {
            ArgumentCheck.Begin().NotNullOrEmpty(userInfo.DeptId, "部门ID").NotNullOrEmpty(userInfo.CompanyId, "公司ID");
                    
            bool insertFlag = false;
            try
            {
                string message = "";
                FormInfo formInfo = BLLFactory<Form>.Instance.FindByID(formId);
                ArgumentCheck.Begin().NotNull(formInfo, "表单对象formInfo");

                #region 创建申请单记录
                ApplyInfo applyInfo = new ApplyInfo(formInfo, userInfo);
                applyInfo.ID = applyId;         //申请单ID
                applyInfo.Title = applyTitle;   //申请标题
                applyInfo.Remark = remark;      //备注说明

                insertFlag = BLLFactory<Apply>.Instance.Insert(applyInfo);
                #endregion

                if (insertFlag)
                {
                    #region 保存流程信息

                    List<FormFlowInfo> flowList = GetRelatedFlows(userInfo, applyInfo.FormId, applyId);
                    int orderNum = 0;
                    foreach (FormFlowInfo flowInfo in flowList)
                    {
                        orderNum++;

                        //申请单流程数据
                        ApplyFlowInfo appFlowInfo = new ApplyFlowInfo(flowInfo);
                        appFlowInfo.ApplyId = applyId;//外部指定
                        appFlowInfo.OrderNo = orderNum;

                        //如果是第一个流程，那么采用传过来的流程处理人（具体的人员名字）
                        if (orderNum == 1 && !string.IsNullOrEmpty(selprocuser))
                        {
                            appFlowInfo.ProcUser = selprocuser;
                        }

                        BLLFactory<ApplyFlow>.Instance.Insert(appFlowInfo);
                    }
                    #endregion

                    //保存申请单日志
                    var content = string.Format("创建了该申请单的处理流程，共有{0}个流程环节。", flowList.Count);
                    BLLFactory<ApplyLog>.Instance.AddApplyLog(applyId, userInfo.ID.ToInt32(), content);

                    message = "您已经提交了该申请单。";
                    message += CreateCurrentFlow(userInfo, applyId);
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);
                throw;
            }

            return insertFlag;
        }

        /// <summary>
        /// 批准流程
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="msgsendto">消息发送人</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="selprocuser">流程选择人</param>
        /// <returns></returns>
        public string ApproveApply(LoginUserInfo userInfo, string applyId, string msgsendto, string opinion, string selprocuser)
        {
            int is_proc = (int)ApplyIsProc.通过;//当前流程是否通过,1为通过，2为拒绝
            ProcessApplyFlow(userInfo, applyId, msgsendto, opinion, is_proc, selprocuser);
            string message = CreateCurrentFlow(userInfo, applyId);

            return message;
        }
                
        /// <summary>
        /// 在当前流程上增加一级同级流程，并批准当前流程
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="msgsendto">消息发送人</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="selprocuser">流程选择人</param>
        /// <returns></returns>
        public string ApproveApplyWithAddFlow(LoginUserInfo userInfo, string applyId, string msgsendto, string opinion, string selprocuser)
        {
            string message = ApproveApply(userInfo, applyId, msgsendto, opinion, selprocuser);
            BLLFactory<Apply>.Instance.SetStatus(applyId, (int)ApplyStatus.处理中);//确保未完成状态。

            BLLFactory<ApplyFlow>.Instance.AddAppFlowByPrevious(applyId, selprocuser);
            
            return message;
        }

        /// <summary>
        /// 批准当前流程，并根据选择用户添加多步的阅办步骤
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="msgsendto">消息发送人</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="selprocuser">流程选择人</param>
        /// <returns></returns>
        public string ApproveApplyWithAddReadFlow(LoginUserInfo userInfo, string applyId, string msgsendto, string opinion, string selprocuser)
        {
            string message = ApproveApply(userInfo, applyId, msgsendto, opinion, selprocuser);
            BLLFactory<ApplyRead>.Instance.AddReadRecord(applyId, selprocuser);

            return message;
        }

        /// <summary>
        /// 用户阅办了该申请单
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="opinion">审批意见</param>
        /// <returns></returns>
        public void ReadApply(LoginUserInfo userInfo, string applyId, string opinion)
        {
            BLLFactory<ApplyRead>.Instance.UpdateReadInfo(applyId, userInfo.ID.ToInt32(), opinion);
        }

        /// <summary>
        /// 拒绝流程
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="msgsendto">消息发送人</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="selprocuser">流程选择人</param>
        /// <returns></returns>
        public void RefuseApply(LoginUserInfo userInfo, string applyId, string msgsendto, string opinion, string selprocuser)
        {
            int is_proc = (int)ApplyIsProc.拒绝;//当前流程是否通过,1为通过，2为拒绝
            ProcessApplyFlow(userInfo, applyId, msgsendto, opinion, is_proc, selprocuser);
        }

        
        /// <summary>
        /// 根据登录用户、表单ID，对已经存在的申请单，跳转流程到上一流程。
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="opinion">处理意见</param>
        public void SkipPreviousApply(LoginUserInfo userInfo, string applyId, string opinion)
        {
            //找到上一流程进行跳转
            var flowInfo = BLLFactory<ApplyFlow>.Instance.GetLastCompletedFlow(applyId);
            if (flowInfo != null)
            {
                //在当前流程中添加意见
                AddCurrentFlowLog(userInfo, applyId, opinion);
                ResetApply(userInfo, applyId, opinion, flowInfo.ID);
            }
            else
            {
                SkipFirstApply(userInfo, applyId, opinion);
            }
        }

        /// <summary>
        /// 根据登录用户、表单ID，对已经存在的申请单，跳转到第一步的流程
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="opinion">处理已经</param>
        public void SkipFirstApply(LoginUserInfo userInfo, string applyId, string opinion)
        {
            ResetApply(userInfo, applyId, opinion);//这里已经增加意见了
        }

        /// <summary>
        /// 根据登录用户、表单ID，对已经存在的申请单，重置流程相关的数据。（记录操作日志）
        /// 用户可以指定跳回到那个流程点，如果流程ID（atflowId）为空，则跳回到第一个流程点上，并初始化相关数据。
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="opinion">处理意见</param>
        /// <param name="atflowId">恢复到的流程ID，如果为空，则表示到第一个流程，否则为具体的流程ID</param>
        public void ResetApply(LoginUserInfo userInfo, string applyId, string opinion, string atflowId = "")
        {
            if (string.IsNullOrEmpty(atflowId))
            {
                //跳回到第一个的时候就实行拒绝操作
                string condition = string.Format("apply_id='{0}' ", applyId);
                var firstFlowInfo = BLLFactory<ApplyFlow>.Instance.FindFirst(condition);
                if (firstFlowInfo != null)
                {
                    RefuseApply(userInfo, applyId, firstFlowInfo.ProcUser, opinion, firstFlowInfo.ProcUser);
                }
            }
            else
            {
                #region 找出要跳到的流程点下所有的流程并初始化

                //对指定的流程步骤，重置相关的状态
                var flowInfo = BLLFactory<ApplyFlow>.Instance.FindByID(atflowId);
                if (flowInfo != null)
                {
                    BLLFactory<ApplyFlow>.Instance.ResetFlowInfo(flowInfo);//更新状态到初始
                }
                #endregion

                #region 流程后续处理
                var appflowInfo = BLLFactory<ApplyFlow>.Instance.GetFirstUnHandledFlow(applyId);
                if (appflowInfo != null)
                {
                    //删除当前审批人，重置申请表的初始化状态，并增加当前审批人和记录日志
                    InitApplyUserAndLog(userInfo, applyId, appflowInfo);

                    //记录到流程日志
                    var flowName = string.Format("重置流程，跳转到\"{0}\"", appflowInfo.FlowName);
                    ApplyFlowlogInfo flowLogInfo = new ApplyFlowlogInfo(applyId, flowName, userInfo.ID.ToInt32());
                    BLLFactory<ApplyFlowlog>.Instance.Insert(flowLogInfo);
                } 
                #endregion
            }
        }

        /// <summary>
        /// 跳回到指定步骤，不记录相关日志。
        /// 用户可以指定跳回到那个流程点，如果为空，则跳回到第一个流程点上，并初始化相关数据。
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="atflowId">恢复到的流程ID，如果为空，则表示到第一个流程，否则为具体的流程ID</param>
        public void ResetApplyNoRecord(LoginUserInfo userInfo, string applyId, string atflowId = null)
        {
            if (string.IsNullOrEmpty(atflowId))
            {
                //跳回到第一个的时候就实行拒绝操作
                string condition = string.Format("apply_id='{0}' ", applyId);
                var firstFlowInfo = BLLFactory<ApplyFlow>.Instance.FindFirst(condition);
                if (firstFlowInfo != null)
                {
                    RefuseApply(userInfo, applyId, firstFlowInfo.ProcUser, "", firstFlowInfo.ProcUser);
                }
            }
            else
            {
                #region 找出要跳到的流程点下所有的流程并初始化

                //对所有的流程步骤，重置相关的状态
                var flowInfo = BLLFactory<ApplyFlow>.Instance.FindByID(atflowId);
                if (flowInfo != null)
                {
                    BLLFactory<ApplyFlow>.Instance.ResetFlowInfo(flowInfo);//更新状态到初始
                    BLLFactory<ApplyFlowlog>.Instance.DeleteByFlowId(applyId, flowInfo.ID);//删除相关流程日志
                }

                #endregion

                #region 流程后续处理

                var appflowInfo = BLLFactory<ApplyFlow>.Instance.GetFirstUnHandledFlow(applyId);
                if (appflowInfo != null)
                {
                    //删除当前审批人，重置申请表的初始化状态，并增加当前审批人和记录日志
                    InitApplyUserAndLog(userInfo, applyId, appflowInfo);
                }
                #endregion
            }
        }

        /// <summary>
        /// 检查申请单及流程的相关情况
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <returns>提示消息</returns>
        public string ApplyCheck(LoginUserInfo userInfo, string applyId)
        {
            ApplyInfo applyInfo = BLLFactory<Apply>.Instance.FindByID(applyId);
            if (applyInfo == null)
            {
                return "该申请单不存在。";
            }
            else if(applyInfo.Status != ApplyStatus.处理中)
            {
                return "该申请单不是[处理中]状态的申请。";
            }

            int applyUserCount = BLLFactory<ApplyUser>.Instance.GetCountByApplyIdAndUserId(applyId, userInfo.ID.ToInt32());
            if (applyUserCount <= 0)
            {
                return "您不是当前流程处理人，也未被授权处理该申请。";
            }

            ApplyFlowInfo flowInfo = BLLFactory<ApplyFlow>.Instance.GetFirstUnHandledFlow(applyId);
            if (flowInfo == null)
            {
                return "该申请单没有需要处理的流程了。";
            }

            return "";
        }

        /// <summary>
        /// 撤销申请单
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="whyCancel">撤销理由</param>
        public void CancelApply(LoginUserInfo userInfo, string applyId, string whyCancel)
        {
            ApplyInfo applyInfo = BLLFactory<Apply>.Instance.FindByID(applyId);
            if (applyInfo != null)
            {
                applyInfo.Status = ApplyStatus.已撤消;
                applyInfo.ProcType = (int)ProcType.无处理;
                applyInfo.ProcUser = "";
                applyInfo.WhyCancel = whyCancel;
                BLLFactory<Apply>.Instance.Update(applyInfo, applyId);//更新申请单状态

                BLLFactory<ApplyUser>.Instance.DeleteByApplyId(applyId);//删除当前审批人

                #region 撤销的时候，对所有的流程步骤，重置相关的状态
                var flowListAll = BLLFactory<ApplyFlow>.Instance.GetAllByApplyId(applyId);
                foreach (ApplyFlowInfo flowInfo in flowListAll)
                {
                    BLLFactory<ApplyFlow>.Instance.ResetFlowInfo(flowInfo);//更新状态到初始
                } 
                #endregion

                #region 记录系统和流程操作日志
                var content = "用户撤消了该申请单。";
                BLLFactory<ApplyLog>.Instance.AddApplyLog(applyId, userInfo.ID.ToInt32(), content);

                //记录到流程日志
                var flowName = "用户撤消";
                var flowLogInfo = new ApplyFlowlogInfo(applyId, flowName, userInfo.ID.ToInt32(), whyCancel, applyInfo.ProcType);
                BLLFactory<ApplyFlowlog>.Instance.Insert(flowLogInfo);  
                #endregion
            }
        }

        /// <summary>
        /// 重新创建申请单流程
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="applyTitle">申请单标题</param>
        /// <param name="selprocuser">流程处理人</param>
        /// <param name="remark">备注信息</param>
        public void ReCreateApplyFlow(LoginUserInfo userInfo, string applyId, string applyTitle, string selprocuser, string remark)
        {
            string message = "";

            ApplyInfo applyInfo = BLLFactory<Apply>.Instance.FindByID(applyId);
            if (applyInfo != null)
            {
                //更新流程的标题
                applyInfo.Status = ApplyStatus.处理中;//重新设置为处理中
                applyInfo.Title = applyTitle;
                applyInfo.Remark = remark;
                BLLFactory<Apply>.Instance.Update(applyInfo, applyId);

                //删除原来所有的流程步骤
                BLLFactory<ApplyFlow>.Instance.DeleteAllFlow(applyId);

                #region 添加新的流程,并保存

                //获取当前用户相关联的流程，有些给过滤掉了
                List<FormFlowInfo> flowList = GetRelatedFlows(userInfo, applyInfo.FormId, applyId);
                int orderNum = 0;
                foreach (FormFlowInfo flowInfo in flowList)
                {
                    orderNum++;

                    #region 申请单流程数据
                    ApplyFlowInfo appFlowInfo = new ApplyFlowInfo(flowInfo);
                    appFlowInfo.ApplyId = applyId;
                    appFlowInfo.OrderNo = orderNum;
                    #endregion

                    //如果是第一个流程，那么采用传过来的流程处理人
                    if (orderNum == 1 && !string.IsNullOrEmpty(selprocuser))
                    {
                        appFlowInfo.ProcUser = selprocuser;
                    }

                    BLLFactory<ApplyFlow>.Instance.Insert(appFlowInfo);
                }
                #endregion

                #region 保存申请单日志

                var content = string.Format("创建了该申请单的处理流程，共有{0}个流程环节。", flowList.Count);
                BLLFactory<BLL.ApplyLog>.Instance.AddApplyLog(applyId, userInfo.ID.ToInt32(), content);

                //记录到流程日志
                var flowName = "重新提交表单";
                ApplyFlowlogInfo flowLogInfo = new ApplyFlowlogInfo(applyId, flowName, userInfo.ID.ToInt32(), "");
                BLLFactory<ApplyFlowlog>.Instance.Insert(flowLogInfo);

                #endregion

                //添加流程处理人
                message = "您已经更新了该申请单。";
                message += CreateCurrentFlow(userInfo, applyId);
            }
        }

        /// <summary>
        /// 在创建/修改 申请单的时候，获取当前用户相关联的流程步骤信息。
        /// 如果申请单ID不为空，则可能会执行过滤条件Cond_verify的判断；如果申请单ID为空，则列出所有条件的流程。
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="formId">申请单类型ID</param>
        /// <param name="applyId">申请单ID，如果不为空（存在记录） 则判断过滤条件Cond_verify；如果申请单ID为空，则列出所有条件的流程。</param>
        /// <returns></returns>
        public List<FormFlowInfo> GetRelatedFlows(LoginUserInfo userInfo, string formId, string applyId = null)
        {
            List<FormFlowInfo> relatedFlowList = new List<FormFlowInfo>();

            List<FormFlowInfo> flowList = BLLFactory<FormFlow>.Instance.GetFormFlow(formId);
            foreach (FormFlowInfo flowInfo in flowList)
            {
                #region 过滤不进行的流程

                //当仅当申请单不为空的时候，即重新修改表单的时候进行条件过滤
                if (!string.IsNullOrEmpty(flowInfo.CondVerify) && !string.IsNullOrEmpty(applyId))
                {
                    FormInfo formInfo = BLLFactory<Form>.Instance.FindByID(formId);
                    if (formInfo != null)
                    {
                        //如果在业务表中没有存在指定条件的记录，则跳过该流程
                        int count = BLLFactory<Form>.Instance.GetApplyCount(formInfo.DataTable, applyId, flowInfo.CondVerify);
                        if (count <= 0)
                        {
                            continue;
                        }
                    }
                }

                #endregion

                relatedFlowList.Add(flowInfo);
            }

            return relatedFlowList;
        }


        /// <summary>
        /// 发起会签流程，根据选择用户添加多步的会签步骤
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="msgsendto">消息发送人</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="selprocuser">流程选择人</param>
        /// <returns></returns>
        public string ApproveApplyWithAddSignFlow(LoginUserInfo userInfo, string applyId, string msgsendto, string opinion, string selprocuser)
        {
            string message = CreateCurrentFlow(userInfo, applyId, false);
            //写入流程处理人记录,及会签人记录
            BLLFactory<ApplySign>.Instance.AddSignRecord(applyId, selprocuser);

            return message;
        }

        /// <summary>
        /// 用户会签了该申请单
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="is_proc">当前流程是否通过,1为通过，2为拒绝</param>
        /// <returns></returns>
        public string SignApply(LoginUserInfo userInfo, string applyId, string opinion, int is_proc)
        {
            var selprocuser = string.Empty;
            ProcessApplyFlow(userInfo, applyId, string.Empty, opinion, is_proc, selprocuser);
            string message = CreateCurrentFlow(userInfo, applyId, false);

            return message;
        }


        /// <summary>
        /// 在当前流程步骤中添加审批记录
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="opinion">处理意见</param>
        private void AddCurrentFlowLog(LoginUserInfo userInfo, string applyId, string opinion)
        {
            var flowInfo = BLLFactory<ApplyFlow>.Instance.GetFirstUnHandledFlow(applyId);
            if (flowInfo != null)
            {
                //记录用户处理意见
                var logInfo = new ApplyFlowlogInfo(applyId, flowInfo.FlowName, userInfo.ID.ToInt32(), opinion);
                BLLFactory<ApplyFlowlog>.Instance.Insert(logInfo);
            }
        }
                
        /// <summary>
        /// 处理当前的流程Approve/Refuse。
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="msgsendto">发送消息给谁</param>
        /// <param name="opinion">处理意见</param>
        /// <param name="is_proc">当前流程是否通过,1为通过，2为拒绝</param>
        /// <param name="selprocuser">流程处理人</param>
        private void ProcessApplyFlow(LoginUserInfo userInfo, string applyId, string msgsendto, string opinion, int is_proc, string selprocuser)
        {
            ApplyInfo applyInfo = BLLFactory<Apply>.Instance.FindByID(applyId);
            ApplyFlowInfo flowInfo = BLLFactory<ApplyFlow>.Instance.GetFirstUnHandledFlow(applyId);
            if (applyInfo != null && flowInfo != null)
            {
                #region 计算处理用时

                string last_time = applyInfo.ProcTime;
                if (string.IsNullOrEmpty(last_time))
                {
                    last_time = applyInfo.Edittime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                long deltatime = (long)DateTime.Now.Subtract(Convert.ToDateTime(last_time)).TotalSeconds;
                
                #endregion

                int is_auth = BLLFactory<ApplyUser>.Instance.GetCountByApplyIdAndUserId(applyId, userInfo.ID.ToInt32()) > 0 ? 0 : 1;

                if (applyInfo.ProcType == (int)ProcType.会签)
                {
                    #region 会签处理
                    //更新会签记录
                    BLLFactory<ApplySign>.Instance.UpdateSignInfo(applyId, flowInfo.ID, userInfo.ID.ToInt32(), opinion, is_proc);

                    //判断是否完成会签
                    var signFinished = BLLFactory<ApplySign>.Instance.IsSignFinished(flowInfo.ID);
                    if (signFinished)
                    {
                        //判断是否全部通过会签
                        var signPassed = BLLFactory<ApplySign>.Instance.IsSignPassed(flowInfo.ID);

                        //如果全部会签流程处理完毕
                        var signMessage = signPassed ? "会签通过" : "会签不通过";
                        flowInfo.ProcTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //在发起人未审批前，会签不完成
                        //flowInfo.IsProc = signPassed ? (int)ApplyStatus.已完成 : (int)ApplyStatus.已退回;
                        flowInfo.MsgSendTo = msgsendto;
                        flowInfo.Deltatime = Convert.ToInt32(deltatime);
                        flowInfo.Opinion = signMessage;
                        BLLFactory<ApplyFlow>.Instance.Update(flowInfo, flowInfo.ID);

                        //更新流程状态(审批状态，未处理，让发起人审批是否提交）
                        applyInfo.Status = ApplyStatus.处理中;
                        applyInfo.ProcType = (int)ProcType.审批;
                        BLLFactory<Apply>.Instance.Update(applyInfo, applyId);

                        var applyUserInfo = new ApplyUserInfo(applyId, applyInfo.ProcUser.ToInt32());
                        BLLFactory<ApplyUser>.Instance.Insert(applyUserInfo);

                        //保存申请单日志(系统流程日志）
                        var content = string.Format("该申请在流程环节[{0}][{1}]。", flowInfo.FlowName, signMessage);
                        BLLFactory<ApplyLog>.Instance.AddApplyLog(applyId, userInfo.ID.ToInt32(), content); 
                    } 
                    #endregion
                }
                //else if(applyInfo.ProcType == (int)ProcType.阅办)
                //{
                //    //阅办处理
                //}
                else
                {
                    #region 更新流程及处理单信息
                    flowInfo.ProcUid = userInfo.ID.ToInt32();
                    flowInfo.ProcTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    flowInfo.IsProc = is_proc;
                    flowInfo.MsgSendTo = msgsendto;
                    flowInfo.Deltatime = Convert.ToInt32(deltatime);
                    flowInfo.Opinion = opinion;
                    BLLFactory<ApplyFlow>.Instance.Update(flowInfo, flowInfo.ID);
                                        
                    #endregion

                    if (is_proc == (int)ApplyIsProc.拒绝)
                    {
                        #region 拒绝流程

                        //更新流程状态
                        applyInfo.Status = ApplyStatus.已退回;
                        applyInfo.ProcType = 0;
                        applyInfo.ProcUser = "";
                        BLLFactory<Apply>.Instance.Update(applyInfo, applyId);

                        //拒绝流程后，删除申请单的所有流程用户
                        BLLFactory<ApplyUser>.Instance.DeleteByApplyId(applyId);

                        //保存申请单日志(系统流程日志）
                        string content = string.Format("申请单({0})已经被退回", applyInfo.Title);
                        FormInfo formInfo = BLLFactory<Form>.Instance.FindByID(applyInfo.FormId);
                        if (formInfo != null)
                        {
                            content = string.Format("用户ID为 {0}，于 {1} 创建的 {2}({3}) 已经被退回。",
                                      applyInfo.Editor, applyInfo.Edittime, formInfo.FormName, applyInfo.Title);
                        }

                        BLLFactory<ApplyLog>.Instance.AddApplyLog(applyId, userInfo.ID.ToInt32(), content);

                        //"您已经退回了该申请。"; 
                        #endregion
                    }
                    else
                    {
                        #region 批准流程后续处理
                        //该申请单的下一处理流程
                        ApplyFlowInfo nextFlowInfo = BLLFactory<ApplyFlow>.Instance.GetNextUnHandledFlow(applyId, flowInfo.ID);
                        if (nextFlowInfo != null)
                        {
                            selprocuser = selprocuser.Trim();
                            if (!string.IsNullOrEmpty(selprocuser) && selprocuser != nextFlowInfo.ProcUser)
                            {
                                nextFlowInfo.ProcUser = selprocuser;
                                BLLFactory<ApplyFlow>.Instance.Update(nextFlowInfo, nextFlowInfo.ID);
                            }
                        }

                        //保存申请单日志(系统流程日志）
                        var content = string.Format("在流程环节[{0}]用户批准了申请。", flowInfo.FlowName);
                        BLLFactory<ApplyLog>.Instance.AddApplyLog(applyId, userInfo.ID.ToInt32(), content); 
                        #endregion
                    }

                    //更新处理单最后处理时间
                    applyInfo.ProcTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    BLLFactory<Apply>.Instance.Update(applyInfo, applyId); 
                }

                #region 记录到流程日志

                var flowLogInfo = new ApplyFlowlogInfo(applyId, flowInfo.FlowName, userInfo.ID.ToInt32(), opinion, applyInfo.ProcType);
                flowLogInfo.FlowId = flowInfo.ID;
                flowLogInfo.OrderNo = flowInfo.OrderNo;
                BLLFactory<ApplyFlowlog>.Instance.Insert(flowLogInfo); 

                #endregion
            }
        }

        /// <summary>
        /// 审批流程后，进行下一步流程准备
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="initApplyUser">是否初始化流程用户</param>
        /// <returns>处理消息</returns>
        private string CreateCurrentFlow(LoginUserInfo userInfo, string applyId, bool initApplyUser = true)
        {
            string message = "";

            ApplyFlowInfo flowInfo = BLLFactory<ApplyFlow>.Instance.GetNextUnHandledFlow(applyId);
            if (flowInfo == null)
            {
                //再也找不到下一步流程情况，则更新为完成状态
                BLLFactory<Apply>.Instance.SetStatusFinished(applyId, userInfo.ID.ToInt32());
                message += "该申请单已经完成";
            }
            else
            {
                //判读是否需要重新初始化用户（如会签过程中不需要处理）
                if (initApplyUser)
                {
                    //审批流程：删除当前审批人, 重置申请表的初始化状态，并增加当前审批人和记录日志
                    InitApplyUserAndLog(userInfo, applyId, flowInfo);
                }
            }

            return message;
        }

        /// <summary>
        /// 重置申请表为处理中状态，并增加当前审批人和记录日志
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="flowInfo">流程信息</param>
        private void InitApplyUserAndLog(LoginUserInfo userInfo, string applyId, ApplyFlowInfo flowInfo)
        {
            //删除当前审批人
            BLLFactory<ApplyUser>.Instance.DeleteByApplyId(applyId);

            ApplyInfo applyInfo = BLLFactory<Apply>.Instance.FindByID(applyId);
            if (applyInfo != null)
            {
                //如果指定了用户，则使用指定的，否则使用流程用户
                var procUsers = flowInfo.ProcUser;

                #region 更新申请表单的状态及信息

                applyInfo.ProcUser = procUsers;
                applyInfo.ProcType = flowInfo.ProcType;
                applyInfo.MustSelect = false;
                applyInfo.Status = ApplyStatus.处理中;
                applyInfo.ProcTime = "";
                applyInfo.WhyCancel = "";
                BLLFactory<Apply>.Instance.Update(applyInfo, applyId);

                #endregion

                #region 增加新的流程处理人ApplyUser

                if (!string.IsNullOrEmpty(procUsers))
                {
                    var userList = procUsers.ToDelimitedList<string>(",");
                    foreach (string userId in userList)
                    {
                        var applyUserInfo = new ApplyUserInfo(applyInfo.ID, userId.Trim().ToInt32());
                        BLLFactory<ApplyUser>.Instance.Insert(applyUserInfo);
                    }
                }
                #endregion

                #region 申请单日志记录

                var content = string.Format("创建流程环节[{0}]的处理人信息。", flowInfo.FlowName);
                BLLFactory<ApplyLog>.Instance.AddApplyLog(applyId, userInfo.ID.ToInt32(), content);

                #endregion
            }
        }
    }
}
