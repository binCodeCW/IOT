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
    /// 供应商分组
    /// </summary>
	public interface ISupplierGroup : IBaseDAL<SupplierGroupInfo>
    {      
        /// <summary>
        /// 根据用户，获取树形结构的供应商分组列表
        /// </summary>
        List<SupplierGroupNodeInfo> GetTree(string creator, string condition);

        /// <summary>
        /// 根据供应商ID，获取供应商对应的分组集合
        /// </summary>
        /// <param name="supplierId">供应商ID</param>
        /// <returns></returns>
        List<SupplierGroupInfo> GetBySupplier(string supplierId);
    }
}