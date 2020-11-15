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
	/// IForm 的摘要说明。
	/// </summary>
	public interface IForm : YH.Framework.ControlUtil.IBaseDAL<FormInfo>
	{                
        /// <summary>
        /// 列出指定表的所有字段名称
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        List<string> ListColumns(string tableName);
                
        /// <summary>
        /// 获取对应的申请单中的记录数
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="apply_id"></param>
        /// <returns></returns>
        int GetApplyCount(string datatable, string apply_id, string cond_verify);

    }
}