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
	/// IAppProc 的摘要说明。
	/// </summary>
	public interface IFormProc : YH.Framework.ControlUtil.IBaseDAL<FormProcInfo>
	{              
        /// <summary>
        /// 获取目前在用的处理类型
        /// </summary>
        /// <returns></returns>
        Dictionary<string, int> GetAllProcType();
                      
        /// <summary>
        /// 获取流程管理环节sql
        /// </summary>
        /// <returns></returns>
        string GetManageProcSql();
    }
}