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
	/// IApplyRead 的摘要说明。
	/// </summary>
	public interface IApplyRead : YH.Framework.ControlUtil.IBaseDAL<ApplyReadInfo>
	{                
        /// <summary>
        /// 更新已读内容及时间
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="content">已读处理意见</param>
        /// <returns></returns>
        bool UpdateReadInfo(string applyId, int userId, string content);
    }
}