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
	/// IFlowLog 的摘要说明。
	/// </summary>
	public interface IApplyFlowlog : YH.Framework.ControlUtil.IBaseDAL<ApplyFlowlogInfo>
	{                
        /// <summary>
        /// 根据申请单ID和流程名称获取对应的记录列表
        /// </summary>
        /// <param name="applyId"></param>
        /// <param name="flowName"></param>
        /// <returns></returns>
        List<ApplyFlowlogInfo> GetFlowLog(string applyId, string flowName);
    }
}