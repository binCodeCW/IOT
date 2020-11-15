using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 会签处理
    /// </summary>
	public class ApplySign : BaseBLL<ApplySignInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ApplySign() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 获取指定流程的会签步骤数量
        /// </summary>
        /// <param name="flowId">会签流程ID</param>
        /// <returns></returns>
        public int GetCountByFlowId(string flowId)
        {
            IApplySign dal = baseDal as IApplySign;
            return dal.GetCountByFlowId(flowId);
        }

        /// <summary>
        /// 判断会签是否开始了（用于在首次发起会签处理状态判断）
        /// （如果会签用户列表没有记录，则没有开始，反之这是在会签过程中）
        /// </summary>
        /// <param name="flowId">流程ID</param>
        /// <returns></returns>
        public bool IsSignReady(string flowId)
        {
            int count = GetCountByFlowId(flowId);
            return count > 0;
        }

        /// <summary>
        /// 是否完成会签所有步骤（不一定全部通过）
        /// </summary>
        /// <param name="flowId">会签流程ID</param>
        /// <returns></returns>
        public bool IsSignFinished(string flowId)
        {
            bool allFinished = true;
            string condition = string.Format("FLOW_ID='{0}' ", flowId);
            var list = baseDal.Find(condition);
            if (list == null || list.Count == 0)
            {
                allFinished = false; //如果会签人员列表没有，那么尚未开始，因此是没完成
            }
            else
            {
                foreach(var info in list)
                {
                    if(info.IsProc == 0)
                    {
                        allFinished = false;//如果有任何一个记录处于初始状态，则是没完成
                    }
                }
            }

            return allFinished;
        }

        /// <summary>
        /// 是否通过了会签所有步骤
        /// </summary>
        /// <param name="flowId">会签流程ID</param>
        /// <returns></returns>
        public bool IsSignPassed(string flowId)
        {
            bool allFinished = true;
            string condition = string.Format("FLOW_ID='{0}' ", flowId);
            var list = baseDal.Find(condition);
            foreach (var info in list)
            {
                if (info.IsProc != (int)ApplyIsProc.通过)
                {
                    allFinished = false;
                }
            }
            return allFinished;
        }

        /// <summary>
        /// 增加会签处理记录
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="userIdList">流程用户ID列表</param>
        /// <returns></returns>
        public bool AddSignRecord(string applyId, string userIdList)
        {
            if (string.IsNullOrEmpty(applyId) || string.IsNullOrEmpty(userIdList))
                return false;

            string[] userIdArray = userIdList.Split(',');
            if (userIdArray == null || userIdArray.Length == 0)
                return false;

            //获取当前流程ID
            string flowId = "";
            var currentFlowInfo = BLLFactory<ApplyFlow>.Instance.GetFirstUnHandledFlow(applyId);
            if (currentFlowInfo != null)
            {
                flowId = currentFlowInfo.ID;
            }

            //删除当前审批人
            BLLFactory<ApplyUser>.Instance.DeleteByApplyId(applyId);

            foreach (string userId in userIdArray)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    //写入流程处理人记录
                    var applyUserInfo = new ApplyUserInfo(applyId, Convert.ToInt32(userId));
                    BLLFactory<ApplyUser>.Instance.Insert(applyUserInfo);

                    //写入会签记录信息(初始化）
                    ApplySignInfo signInfo = new ApplySignInfo(applyId, Convert.ToInt32(userId), flowId);
                    baseDal.Insert(signInfo);
                }
            }

            return true;
        }


        /// <summary>
        /// 更新已读内容及时间
        /// </summary>
        /// <param name="applyId">申请单信息</param>
        /// <param name="flowId">会签流程ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="content">处理意见</param>
        /// <param name="isProc">是否处理(0:未处理/未处理完,1:已处理,2/其它:已退回)</param>
        /// <returns></returns>
        public bool UpdateSignInfo(string applyId, string flowId, int userId, string content = "", int isProc = 1)
        {
            var result = false;
            var condition = string.Format("FLOW_ID='{0}' and USER_ID={1}", flowId, userId);
            var info = baseDal.FindSingle(condition);
            if(info != null)
            {
                info.IsProc = isProc;
                info.Opinion = content;
                info.Deltatime =  (int)DateTime.Now.Subtract(info.ProcTime).TotalSeconds;
                info.ProcTime = DateTime.Now;//重新更新时间
                result = baseDal.Update(info, info.ID);
            }

            BLLFactory<ApplyUser>.Instance.DeleteByApplyId(applyId, userId);

            return result;
        }

        /// <summary>
        /// 根据申请单ID获取对应的会签记录
        /// </summary>
        /// <param name="flowId">流程步骤ID</param>
        /// <returns></returns>
        public List<ApplySignInfo> FindByFlowId(string flowId)
        {
            string condition = string.Format("FLOW_ID='{0}' ", flowId);
            return baseDal.Find(condition);
        }
    }
}
