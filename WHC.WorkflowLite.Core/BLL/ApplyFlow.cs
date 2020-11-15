using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Data.Common;
using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 申请单流程信息管理
    /// </summary>
    public class ApplyFlow : BaseBLL<ApplyFlowInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ApplyFlow()  : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 在当前的流程上增加一个流程
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="selprocuser">流程用户</param>
        /// <returns>增加成功返回True，否则False</returns>
        public bool AddAppFlowByPrevious(string applyId, string selprocuser)
        {
            IApplyFlow dal = baseDal as IApplyFlow;
            ApplyFlowInfo currentFlowInfo = dal.GetLastCompletedFlow(applyId);
            if (currentFlowInfo == null)
            {
                return false;
            }

            string condition = string.Format(" Apply_id='{0}' and Order_no>{1} ", currentFlowInfo.ApplyId, currentFlowInfo.OrderNo);
            string orderBy = " order by Order_no ";

            List<ApplyFlowInfo> appFlowAfter = baseDal.Find(condition, orderBy);//原有在当前步骤之后的所有步骤
            if (appFlowAfter != null)
            {
                try
                {
                    #region 增加一流程，顺序为当前流程+1

                    //复制当前流程的数据
                    ApplyFlowInfo addFlowInfo = new ApplyFlowInfo();
                    addFlowInfo.ApplyId = currentFlowInfo.ApplyId;
                    addFlowInfo.CanBack = currentFlowInfo.CanBack;
                    addFlowInfo.CanBeBack = currentFlowInfo.CanBeBack;
                    addFlowInfo.CanSms = currentFlowInfo.CanSms;
                    addFlowInfo.CondVerify = currentFlowInfo.CondVerify;
                    addFlowInfo.FlowName = currentFlowInfo.FlowName;
                    addFlowInfo.IsAdd = 1;
                    addFlowInfo.MayAddflow = currentFlowInfo.MayAddflow;
                    addFlowInfo.MayMsgsend = currentFlowInfo.MayMsgsend;
                    addFlowInfo.MaySelproc = currentFlowInfo.MaySelproc;
                    addFlowInfo.MsgSendTo = currentFlowInfo.MsgSendTo;
                    addFlowInfo.ProcType = currentFlowInfo.ProcType;
                    addFlowInfo.ProcUser = selprocuser;//该步骤的处理人
                    addFlowInfo.SmsProc = currentFlowInfo.SmsProc;

                    addFlowInfo.OrderNo = currentFlowInfo.OrderNo + 1;
                    bool added = baseDal.Insert(addFlowInfo);

                    #endregion

                    if (added)
                    {
                        //在原有流程顺序上增加1
                        foreach (ApplyFlowInfo info in appFlowAfter)
                        {
                            info.OrderNo += 1;
                            baseDal.Update(info, info.ID.ToString());
                        }

                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 为最后的流程增加阅办步骤（根据selprocuser增加1~n步）
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="selprocuser">流程用户ID字符串，如：1,2,3</param>
        /// <returns></returns>
        public bool AddReadAppFlow(string applyId, string selprocuser)
        {
            if (string.IsNullOrEmpty(applyId) || string.IsNullOrEmpty(selprocuser))
                return false;

            IApplyFlow dal = baseDal as IApplyFlow;
            ApplyFlowInfo currentFlowInfo = dal.GetLastCompletedFlow(applyId);
            int orderno = currentFlowInfo.OrderNo + 1;

            #region 为流程中每个用户创建一个阅办流程, 并增加流程处理人ApplyUser信息

            string[] userArray = selprocuser.Split(',');
            foreach (string userId in userArray)
            {
                ApplyFlowInfo addFlowInfo = new ApplyFlowInfo();
                addFlowInfo.ApplyId = applyId;
                addFlowInfo.FlowName = "阅办";
                addFlowInfo.IsAdd = 1;
                addFlowInfo.ProcType = (int)ProcType.阅办;//阅办
                addFlowInfo.ProcUser = userId;//该步骤的处理人
                addFlowInfo.OrderNo = orderno++;

                baseDal.Insert(addFlowInfo);
            }
            
            foreach (string userId in userArray)
            {
                var userInfo = new ApplyUserInfo(applyId, Convert.ToInt32(userId.Trim()));
                BLLFactory<ApplyUser>.Instance.Insert(userInfo);
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 获取最后一条完成的流程
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public ApplyFlowInfo GetLastCompletedFlow(string applyId)
        {
            IApplyFlow dal = baseDal as IApplyFlow;
            return dal.GetLastCompletedFlow(applyId);
        }

        /// <summary>
        /// 获取第一个未被处理的申请单流程记录
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public ApplyFlowInfo GetFirstUnHandledFlow(string applyId)
        {
            IApplyFlow dal = baseDal as IApplyFlow;
            return dal.GetFirstUnHandledFlow(applyId);
        }

        /// <summary>
        /// 获取下一个未被处理的流程记录
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="flowId">当前流程ID</param>
        /// <returns></returns>
        public ApplyFlowInfo GetNextUnHandledFlow(string applyId, string flowId)
        {
            IApplyFlow dal = baseDal as IApplyFlow;
            return dal.GetNextUnHandledFlow(applyId, flowId);
        }

        /// <summary>
        /// 获取第一个未被处理的申请单流程记录(下一个流程）
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public ApplyFlowInfo GetNextUnHandledFlow(string applyId)
        {
            ApplyInfo applyInfo = BLLFactory<Apply>.Instance.FindByID(applyId);
            ApplyFlowInfo flowInfo = null;
            while (true)
            {
                #region 自动跳过不符合流程环节

                flowInfo = BLLFactory<ApplyFlow>.Instance.GetFirstUnHandledFlow(applyId);
                if (flowInfo == null)
                {
                    break;
                }

                if (!string.IsNullOrEmpty(flowInfo.CondVerify))//匹配条件并过滤部分流程
                {
                    int count = BLLFactory<Form>.Instance.GetApplyCount(applyInfo.DataTable, applyId, flowInfo.CondVerify);
                    if (count <= 0)
                    {
                        string opinion = "系统自动跳过不符合条件的流程环节";
                        BLLFactory<ApplyFlow>.Instance.HandleFlowWithOpinion(flowInfo.ID, applyId, opinion);
                        continue;
                    }
                }

                #endregion

                break;//处理完毕就要跳出，切记
            }
            return flowInfo;
        }

        /// <summary>
        /// 设置流程为已处理并增加意见
        /// </summary>
        /// <param name="id">流程ID</param>
        /// <param name="apply_id">申请单ID</param>
        /// <param name="opinion">处理意见</param>
        /// <returns></returns>
        public bool HandleFlowWithOpinion(string id, string apply_id, string opinion)
        {
            IApplyFlow dal = baseDal as IApplyFlow;
            return dal.HandleFlowWithOpinion(id, apply_id, opinion);
        }

        /// <summary>
        /// 获取对应表单下的所有流程，根据order_no由小到大排序
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public List<ApplyFlowInfo> GetAllByApplyId(string applyId)
        {
            IApplyFlow dal = baseDal as IApplyFlow;
            return dal.GetAllByApplyId(applyId);
        }

        /// <summary>
        /// 删除指定申请单的流程步骤
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public bool DeleteAllFlow(string applyId, DbTransaction trans = null)
        {
            string condition = string.Format("apply_id='{0}' ", applyId);
            BLLFactory<ApplySign>.Instance.DeleteByCondition(condition, trans);    //会签记录一并删除
            BLLFactory<ApplyFlowlog>.Instance.DeleteByCondition(condition, trans);//流程日志也一起删除

            return baseDal.DeleteByCondition(condition, trans);
        }

        /// <summary>
        /// 对指定的流程步骤，重置相关的状态
        /// </summary>
        /// <param name="flowInfo">流程信息</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public bool ResetFlowInfo(ApplyFlowInfo flowInfo, DbTransaction trans = null)
        {
            flowInfo.ProcUid = 0;
            flowInfo.IsProc = 0;
            flowInfo.ProcTime = "";
            flowInfo.Opinion = "";
            flowInfo.Deltatime = 0;

            return baseDal.Update(flowInfo, flowInfo.ID, trans);//更新状态到初始
        }
    }
}
