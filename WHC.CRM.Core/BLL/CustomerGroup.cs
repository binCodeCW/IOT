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
    /// 客户组别
    /// </summary>
	public class CustomerGroup : BaseBLL<CustomerGroupInfo>
    {
        public CustomerGroup() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 根据用户，获取树形结构的客户分组列表
        /// </summary>
        /// <param name="creator">创建用户</param>
        /// <param name="condition">过滤条件，可以根据区域进行过滤</param>
        public List<CustomerGroupNodeInfo> GetTree(string creator, string condition)
        {
            ICustomerGroup dal = baseDal as ICustomerGroup;
            return dal.GetTree(creator, condition);
        }

        /// <summary>
        /// 根据客户ID，获取客户对应的分组集合
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <returns></returns>
        public List<CustomerGroupInfo> GetByCustomer(string customerId)
        {
            ICustomerGroup dal = baseDal as ICustomerGroup;
            return dal.GetByCustomer(customerId);
        }

        /// <summary>
        /// 根据用户标识，获取对应用户的分组集合
        /// </summary>
        /// <param name="creator">用户标识（用户登陆名称）</param>
        /// <returns></returns>
        public List<CustomerGroupInfo> GetAllByUser(string creator)
        {
            string condition = string.Format("creator='{0}'", creator);
            return Find(condition);
        }
    }
}
