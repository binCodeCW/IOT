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
    /// 供应商分组
    /// </summary>
	public class SupplierGroup : BaseBLL<SupplierGroupInfo>
    {
        public SupplierGroup() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据用户，获取树形结构的供应商分组列表
        /// </summary>
        public List<SupplierGroupNodeInfo> GetTree(string creator, string condition)
        {
            ISupplierGroup dal = baseDal as ISupplierGroup;
            return dal.GetTree(creator, condition);
        }

        /// <summary>
        /// 根据供应商ID，获取供应商对应的分组集合
        /// </summary>
        /// <param name="customerId">供应商ID</param>
        /// <returns></returns>
        public List<SupplierGroupInfo> GetBySupplier(string customerId)
        {
            ISupplierGroup dal = baseDal as ISupplierGroup;
            return dal.GetBySupplier(customerId);
        }

        /// <summary>
        /// 根据用户标识，获取对应用户的分组集合
        /// </summary>
        /// <param name="creator">用户标识（用户登陆名称）</param>
        /// <returns></returns>
        public List<SupplierGroupInfo> GetAllByUser(string creator)
        {
            string condition = string.Format("creator='{0}'", creator);
            return Find(condition);
        }
    }
}
