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
    /// 客户组别
    /// </summary>
	public interface ICustomerGroup : IBaseDAL<CustomerGroupInfo>
    {        
        /// <summary>
        /// 根据用户，获取树形结构的客户分组列表
        /// </summary>
        /// <param name="creator">创建用户</param>
        /// <param name="condition">过滤条件，可以根据区域过滤</param>
        List<CustomerGroupNodeInfo> GetTree(string creator, string condition);
                        
        /// <summary>
        /// 根据客户ID，获取客户对应的分组集合
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <returns></returns>
        List<CustomerGroupInfo> GetByCustomer(string customerId);
    }
}