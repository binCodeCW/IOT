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
	/// IApplyUser 的摘要说明。
	/// </summary>
	public interface IApplyUser : YH.Framework.ControlUtil.IBaseDAL<ApplyUserInfo>
	{               
        /// <summary>
        /// 获取用户已办业务的ID字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetApplyIdDone(int userId);
                
        /// <summary>
        /// 根据申请表单ID获取对应的处理人员
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        string GetUsersByApplyId(string applyId);
    }
}