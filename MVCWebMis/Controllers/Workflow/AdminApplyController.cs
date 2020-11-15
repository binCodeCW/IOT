using Newtonsoft.Json.Linq;

using System.Web.Mvc;
using YH.Framework.Commons;
using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class AdminApplyController : BusinessController<AdminApply, ApplyInfo>
    {
        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(ApplyInfo info)
        {
            //留给子类对参数对象进行修改
        }

        protected override void OnBeforeUpdate(ApplyInfo info)
        {
            //留给子类对参数对象进行修改
        }
        #endregion

        /// <summary>
        /// 根据登录用户、表单ID、申请单标题创建所需要的流程数据。
        /// 创建流程前，业务表的数据必须先保存，在执行该流程创建操作，因为逻辑需要调用AppForm的GetApplyCount函数，根据条件过滤不需要的流程。
        /// </summary>
        /// <param name="applyId">申请单的ID标识，请使用一个不重复的ID值，如:System.Guid.NewGuid().ToString()，并且和业务表单的Apply_ID保持一致。</param>
        /// <param name="formId">表单ID</param>
        /// <param name="applyTitle">申请单标题</param>
        /// <param name="selprocuser">第一步的流程处理人</param>
        /// <param name="remark">备注说明</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateApply(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string formId = obj.formId;
                string applyTitle = obj.applyTitle;
                string selprocuser = obj.selprocuser;
                string remark = obj.remark;

                var sucess = AdminApply.Instance.CreateApply(info, applyId, formId, applyTitle, selprocuser, remark);
                var result = new CommonResult(sucess);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 批准流程
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="msgsendto">消息发送人</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="selprocuser">流程选择人</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveApply(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string msgsendto = obj.msgsendto;
                string opinion = obj.opinion;
                string selprocuser = obj.selprocuser;

                var result = AdminApply.Instance.ApproveApply(info, applyId, msgsendto, opinion, selprocuser);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 在当前流程上增加一级同级流程，并批准当前流程
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="msgsendto">消息发送人</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="selprocuser">流程选择人</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveApplyWithAddFlow(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string msgsendto = obj.msgsendto;
                string opinion = obj.opinion;
                string selprocuser = obj.selprocuser;

                var result = AdminApply.Instance.ApproveApplyWithAddFlow(info, applyId, msgsendto, opinion, selprocuser);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 批准当前流程，并根据选择用户添加多步的阅办步骤
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="msgsendto">消息发送人</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="selprocuser">流程选择人</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveApplyWithAddReadFlow(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string msgsendto = obj.msgsendto;
                string opinion = obj.opinion;
                string selprocuser = obj.selprocuser;

                var result = AdminApply.Instance.ApproveApplyWithAddReadFlow(info, applyId, msgsendto, opinion, selprocuser);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 拒绝流程
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="msgsendto">消息发送人</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="selprocuser">流程选择人</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RefuseApply(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string msgsendto = obj.msgsendto;
                string opinion = obj.opinion;
                string selprocuser = obj.selprocuser;

                AdminApply.Instance.RefuseApply(info, applyId, msgsendto, opinion, selprocuser);
                var result = new CommonResult(true);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 用户阅办了该申请单
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="opinion">审批意见</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReadApply(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string opinion = obj.opinion;

                AdminApply.Instance.ReadApply(info, applyId, opinion);
                var result = new CommonResult(true);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 根据登录用户、表单ID，对已经存在的申请单，跳转流程到上一流程。
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="opinion">处理意见</param>
        [HttpPost]
        public ActionResult SkipPreviousApply(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string opinion = obj.opinion;

                AdminApply.Instance.SkipPreviousApply(info, applyId, opinion);
                var result = new CommonResult(true);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 根据登录用户、表单ID，对已经存在的申请单，跳转到第一步的流程
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="opinion">处理已经</param>
        [HttpPost]
        public ActionResult SkipFirstApply(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string opinion = obj.opinion;

                AdminApply.Instance.SkipFirstApply(info, applyId, opinion);
                var result = new CommonResult(true);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 根据登录用户、表单ID，对已经存在的申请单，重置流程相关的数据。（记录操作日志）
        /// 用户可以指定跳回到那个流程点，如果流程ID（atflowId）为空，则跳回到第一个流程点上，并初始化相关数据。
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="opinion">处理意见</param>
        /// <param name="atflowId">恢复到的流程ID，如果为空，则表示到第一个流程，否则为具体的流程ID</param>
        [HttpPost]
        public ActionResult ResetApply(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string opinion = obj.opinion;
                string atflowId = obj.atflowId;

                AdminApply.Instance.ResetApply(info, applyId, opinion, atflowId);
                var result = new CommonResult(true);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 跳回到指定步骤，不记录相关日志。
        /// 用户可以指定跳回到那个流程点，如果为空，则跳回到第一个流程点上，并初始化相关数据。
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="atflowId">恢复到的流程ID，如果为空，则表示到第一个流程，否则为具体的流程ID</param>
        [HttpPost]
        public ActionResult ResetApplyNoRecord(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string opinion = obj.opinion;
                string atflowId = obj.atflowId;

                AdminApply.Instance.ResetApplyNoRecord(info, applyId, atflowId);
                var result = new CommonResult(true);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 检查申请单及流程的相关情况
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns>提示消息</returns>
        [HttpGet]
        public ActionResult ApplyCheck(string applyId)
        {
            LoginUserInfo info = GetLoginUser();//当前登录用户

            var result = AdminApply.Instance.ApplyCheck(info, applyId);
            return ToJsonContent(result);
        }

        /// <summary>
        /// 撤销申请单
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="whyCancel">撤销理由</param>
        [HttpPost]
        public ActionResult CancelApply(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string whyCancel = obj.whyCancel;

                AdminApply.Instance.CancelApply(info, applyId, whyCancel);
                var result = new CommonResult(true);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 重新创建申请单流程
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="applyTitle">申请单标题</param>
        /// <param name="selprocuser">流程处理人</param>
        /// <param name="remark">备注信息</param>
        [HttpPost]
        public ActionResult ReCreateApplyFlow(JObject param)
        {

            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string applyTitle = obj.applyTitle;
                string selprocuser = obj.selprocuser;
                string remark = obj.remark;

                AdminApply.Instance.ReCreateApplyFlow(info, applyId, applyTitle, selprocuser, remark);
                var result = new CommonResult(true);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 在创建/修改 申请单的时候，获取当前用户相关联的流程步骤信息。
        /// 如果申请单ID不为空，则可能会执行过滤条件Cond_verify的判断；如果申请单ID为空，则列出所有条件的流程。
        /// </summary>
        /// <param name="formId">申请单类型ID</param>
        /// <param name="applyId">申请单ID，如果不为空（存在记录） 则判断过滤条件Cond_verify；如果申请单ID为空，则列出所有条件的流程。</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRelatedFlows(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string formId = obj.formId;
                string applyId = obj.applyId;

                var result = AdminApply.Instance.GetRelatedFlows(info, formId, applyId);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
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
        [HttpPost]
        public ActionResult ApproveApplyWithAddSignFlow(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string msgsendto = obj.msgsendto;
                string opinion = obj.opinion;
                string selprocuser = obj.selprocuser;

                var result = AdminApply.Instance.ApproveApplyWithAddSignFlow(info, applyId, msgsendto, opinion, selprocuser);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 用户会签了该申请单
        /// </summary>
        /// <param name="userInfo">用户、部门、公司对象</param>
        /// <param name="applyId">申请单ID</param>
        /// <param name="opinion">审批意见</param>
        /// <param name="is_proc">当前流程是否通过,1为通过，2为拒绝</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignApply(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                LoginUserInfo info = GetLoginUser();//当前登录用户
                string applyId = obj.applyId;
                string opinion = obj.opinion;
                int is_proc = obj.is_proc;

                var result = AdminApply.Instance.SignApply(info, applyId, opinion, is_proc);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }
    }
}
