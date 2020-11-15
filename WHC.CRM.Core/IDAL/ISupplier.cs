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
    /// 供应商
    /// </summary>
	public interface ISupplier : IBaseDAL<SupplierInfo>
	{
        /// <summary>
        /// 设置删除标志
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <param name="deleted">是否删除</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        bool SetDeletedFlag(string id, bool deleted = true, DbTransaction trans = null);

        /// <summary>
        /// 根据供应商分组的名称，搜索属于该分组的供应商列表
        /// </summary>
        /// <param name="ownerUser">供应商所属用户</param>
        /// <param name="groupId">供应商分组的名称,如果供应商分组为空，那么返回未分组供应商列表</param>
        /// <param name="pagerInfo">分页条件</param>
        /// <returns></returns>
        List<SupplierInfo> FindByGroupName(string ownerUser, string groupName, string condition, PagerInfo pagerInfo = null);

        /// <summary>
        /// 调整供应商的组别
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <param name="groupIdList">供应商分组Id集合</param>
        /// <returns></returns>
        bool ModifyGroup(string id, List<string> groupIdList);

        /// <summary>
        /// 更新供应商的状态信息
        /// </summary>
        /// <param name="id">供应商Id</param>
        /// <param name="orderDate">订单日期</param>
        /// <param name="orderCount">交易次数</param>
        /// <param name="orderMoney">交易金额</param>
        /// <returns></returns>
        bool UpdateTransactionStatus(string id, DateTime orderDate, int orderCount, decimal orderMoney, DbTransaction trans = null);

        /// <summary>
        /// 更新供应商的最后联系日期
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <param name="lastContactDate">最后联系日期</param>
        /// <returns></returns>
        bool UpdateContactDate(string id, DateTime lastContactDate, DbTransaction trans = null);
                      
        /// <summary>
        /// 根据客户ID获取供应商关联ID
        /// </summary>
        /// <param name="customerID">客户ID</param>
        /// <returns></returns>
        List<string> GetSupplierByCustomer(string customerID);
                       
        /// <summary>
        /// 根据供应商所属客户ID，分页获取供应商列表
        /// </summary>
        /// <param name="customerID">供应商所属客户ID</param>
        /// <param name="pagerInfo">分页条件</param>
        /// <returns></returns>
        List<SupplierInfo> FindByCustomer(string customerID, string condition, PagerInfo pagerInfo = null);
    }
}