using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using WHC.WorkflowLite.Entity;

namespace WHC.WorkflowLite.IDAL
{
	/// <summary>
	/// IApplyFlow 的摘要说明。
	/// </summary>
	public interface IApplyFlow : YH.Framework.ControlUtil.IBaseDAL<ApplyFlowInfo>
	{                   
        /// <summary>
        /// 获取最后一条完成的流程
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        ApplyFlowInfo GetLastCompletedFlow(string applyId); 

        /// <summary>
        /// 获取第一个未被处理的申请单流程记录
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        ApplyFlowInfo GetFirstUnHandledFlow(string applyId);
                        
        /// <summary>
        /// 获取下一个未被处理的流程记录
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        ApplyFlowInfo GetNextUnHandledFlow(string applyId, string flowId);                        

        /// <summary>
        /// 设置流程为已处理并增加意见
        /// </summary>
        /// <param name="id">流程ID</param>
        /// <param name="apply_id">申请单ID</param>
        /// <param name="opinion">处理意见</param>
        /// <returns></returns>
        bool HandleFlowWithOpinion(string id, string apply_id, string opinion);
                
        /// <summary>
        /// 获取对应表单下的所有流程，根据order_no由小到大排序
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        List<ApplyFlowInfo> GetAllByApplyId(string applyId);
    }
}