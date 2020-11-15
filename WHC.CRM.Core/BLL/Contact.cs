using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.CRM.Entity;
using WHC.CRM.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;

namespace WHC.CRM.BLL
{
    /// <summary>
    /// 客户联系人
    /// </summary>
	public class Contact : BaseBLL<ContactInfo>
    {
        public Contact() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 根据联系人分组的名称，搜索属于该分组的联系人列表
        /// </summary>
        /// <param name="ownerUser">联系人所属用户</param>
        /// <param name="groupName">联系人分组的名称,如果联系人分组为空，那么返回未分组联系人列表</param>
        /// <param name="pagerInfo">分页条件</param>
        /// <returns></returns>
        public List<ContactInfo> FindByGroupName(string ownerUser, string groupName, PagerInfo pagerInfo = null)
        {
            IContact dal = baseDal as IContact;
            return dal.FindByGroupName(ownerUser, groupName, pagerInfo);
        }

        /// <summary>
        /// 设置删除标志
        /// </summary>
        /// <param name="id">联系人ID</param>
        /// <param name="deleted">是否删除</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public bool SetDeletedFlag(string id, bool deleted = true, DbTransaction trans = null)
        {
            IContact dal = baseDal as IContact;
            return dal.SetDeletedFlag(id, deleted, trans);
        }

        /// <summary>
        /// 获取联系人的名称
        /// </summary>
        /// <param name="id">联系人ID</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public string GetContactName(string id, DbTransaction trans = null)
        {
            //使用缓存减轻数据库查询压力
            System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
            string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, id);
            string name = MemoryCacheHelper.GetCacheItem<string>(key, delegate()
            {
                return GetFieldValue(id, "Name", trans);
            },
            new TimeSpan(0, 30, 0));//30分钟后过期
            return name;
        }

        /// <summary>
        /// 调整联系人的组别
        /// </summary>
        /// <param name="contactId">联系人ID</param>
        /// <param name="groupIdList">分组Id集合</param>
        /// <returns></returns>
        public bool ModifyContactGroup(string contactId, List<string> groupIdList)
        {
            IContact dal = baseDal as IContact;
            return dal.ModifyContactGroup(contactId, groupIdList);
        }

        /// <summary>
        /// 根据客户ID获取对应的联系人列表
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <returns></returns>
        public List<ContactInfo> FindByCustomer(string customerID)
        {
            string condition = string.Format("Customer_ID='{0}'", customerID);
            return baseDal.Find(condition);
        }

        /// <summary>
        /// 根据客户ID和名称获取实体信息
        /// </summary>
        /// <param name="customerID">客户ID</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public ContactInfo FindByCustomerAndName(string customerID, string name)
        {
            string condition = string.Format("Customer_ID='{0}' AND Name='{1}'", customerID, name);
            return baseDal.FindSingle(condition);
        }

        /// <summary>
        /// 根据供应商ID获取对应的联系人列表
        /// </summary>
        /// <param name="supplierID">供应商ID</param>
        /// <returns></returns>
        public List<ContactInfo> FindBySupplier(string supplierID)
        {
            string condition = string.Format("Supplier_ID='{0}'", supplierID);
            return baseDal.Find(condition);
        }

        /// <summary>
        /// 根据供应商ID和名称获取实体信息
        /// </summary>
        /// <param name="supplierID">供应商ID</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public ContactInfo FindBySupplierAndName(string supplierID, string name)
        {
            string condition = string.Format("Supplier_ID='{0}' AND Name='{1}'", supplierID, name);
            return baseDal.FindSingle(condition);
        }

        /// <summary>
        /// 获取联系人的所属客户ID
        /// </summary>
        /// <param name="id">联系人id</param>
        /// <returns></returns>
        public string GetCustomerID(string id)
        {
            return baseDal.GetFieldValue(id, "Customer_ID");
        }

        /// <summary>
        /// 获取联系人的所属供应商ID
        /// </summary>
        /// <param name="id">联系人id</param>
        /// <returns></returns>
        public string GetSupplierID(string id)
        {
            return baseDal.GetFieldValue(id, "Supplier_ID");
        }
    }
}
