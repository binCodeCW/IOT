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
    /// 产品信息
    /// </summary>
	public class Product : BaseBLL<ProductInfo>
    {
        public Product() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 获取所有正常使用的产品列表
        /// </summary>
        /// <returns></returns>
        public List<ProductInfo> GetAllInUsed()
        {
            string condition = string.Format("Status=0");
            return baseDal.Find(condition);
        }

        /// <summary>
        /// 根据ID获取产品名称
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public string GetNameById(string id, DbTransaction trans = null)
        {
            return GetFieldValue(id, "ProductName", trans);
        }

        /// <summary>
        /// 根据产品ID，增加或减少相关产品的数量
        /// </summary>
        /// <param name="id">产品ID</param>
        /// <param name="quantity">产品数量，正数为增加，负数为减少</param>
        /// <returns></returns>
        public bool ModifyQuantity(string id, int quantity, DbTransaction trans = null)
        {
            IProduct dal = baseDal as IProduct;
            return dal.ModifyQuantity(id, quantity, trans);
        }
    }
}
