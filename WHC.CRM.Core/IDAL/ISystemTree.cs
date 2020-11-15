using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using WHC.CRM.Entity;

namespace WHC.CRM.IDAL
{
    /// <summary>
    /// 系统列表集合
    /// </summary>
	public interface ISystemTree : IBaseDAL<SystemTreeInfo>
	{
                
        /// <summary>
        /// 获取树形结构的菜单列表
        /// </summary>
        List<SystemTreeNodeInfo> GetTree(string category);
    }
}