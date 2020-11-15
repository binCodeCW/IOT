using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.CRM.Entity;
using WHC.CRM.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;

namespace WHC.CRM.BLL
{
    /// <summary>
    /// 系统列表集合
    /// </summary>
    public class SystemTree : BaseBLL<SystemTreeInfo>
    {
        public SystemTree()
            : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 获取树形结构的菜单列表
        /// </summary>
        public List<SystemTreeNodeInfo> GetTree(string category)
        {
            ISystemTree dal = baseDal as ISystemTree;
            return dal.GetTree(category);
        }
    }
}
