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
	/// IApplySign 的摘要说明。
	/// </summary>
	public interface IApplySign : YH.Framework.ControlUtil.IBaseDAL<ApplySignInfo>
	{
        /// <summary>
        /// 获取指定流程的会签步骤数量
        /// </summary>
        /// <param name="flowId">流程ID</param>
        /// <returns></returns>
        int GetCountByFlowId(string flowId);
    }
}