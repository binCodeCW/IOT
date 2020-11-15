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
	/// IApply 的摘要说明。
	/// </summary>
	public interface IApply : YH.Framework.ControlUtil.IBaseDAL<ApplyInfo>
	{
        /// <summary>
        /// 获取我的待办数量
        /// </summary>
        /// <param name="applyIdString">当前用户的申请单ID字符串</param>
        /// <returns></returns>
        int GetMyTodoCount(string applyIdString, string formTag);

        /// <summary>
        /// 获取我的待办数量
        /// </summary>
        /// <param name="applyIdString">当前用户的申请单ID字符串</param>
        /// <returns></returns>
        DataTable GetMyTodoList(string applyIdString, string formTag);

        /// <summary>
        /// 获取我的已办数量
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="formTag">表单分类标识（用来区分申请单类型），无则为null</param>
        /// <returns></returns>
        int GetMyDoneCount(int userId, string formTag);

        /// <summary>
        /// 获取我发起的数量
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="formTag">表单分类标识（用来区分申请单类型），无则为null</param>
        /// <returns></returns>
        int GetMyAddedCount(int userId, string formTag);

        /// <summary>
        /// 修改申请单状态为结束状态
        /// </summary>
        /// <param name="id">申请单ID</param>
        void SetStatusFinished(string id);
                       
        /// <summary>
        /// 删除业务表单中的相关数据
        /// </summary>
        /// <param name="apply_id">申请单ID</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        bool DeleteFormTableData(string apply_id, DbTransaction trans = null);

    }
}