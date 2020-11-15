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
    /// 产品销售记录
    /// </summary>
	public class Sell : BaseBLL<SellInfo>
    {
        public Sell() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 生成单据号码
        /// </summary>
        /// <param name="userId">操作人员ID</param>
        /// <returns></returns>
        public string GetOrderNo(int userId)
        {
            string prefix = string.Format("XS-{0}-{1}", userId, DateTime.Now.ToString("yyyyMMdd"));

            //获取当天的记录数量+1
            DateTime dt = DateTime.Now.ToString("yyyy-MM-dd").ToDateTime(); //当前日期
            SearchCondition condition = new SearchCondition();
            condition.AddCondition("OrderDate", dt, SqlOperator.MoreThanOrEqual)
                     .AddCondition("OrderDate", dt.AddDays(1), SqlOperator.LessThan);
            string conditionSql = condition.BuildConditionSql().Replace("Where", "");
            int count = baseDal.GetRecordCount(conditionSql) + 1;

            //循环检索，直到不重复的编号
            string number = string.Format("{0}-{1}", prefix, count);
            while (true)
            {
                if (CheckOrderNumberExist(number))
                {
                    number = string.Format("{0}-{1}", prefix, count++);
                }
                else
                {
                    break;
                }
            }

            return number;
        }

        /// <summary>
        /// 根据订单编号检查是否存在记录
        /// </summary>
        /// <param name="orderNumber">订单编号</param>
        /// <returns></returns>
        private bool CheckOrderNumberExist(string orderNumber)
        {
            return base.IsExistKey("OrderNo", orderNumber);
        }

        /// <summary>
        /// 根据订单编号获取对应的订单信息
        /// </summary>
        /// <param name="orderNumber">订单编号</param>
        /// <returns></returns>
        public SellInfo FindByOrderNumber(string orderNumber, DbTransaction trans = null)
        {
            string condition = string.Format("OrderNo ='{0}'", orderNumber);
            return FindSingle(condition, trans);
        }
                 
        /// <summary>
        /// 获取订单日期年度列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetOrderYearList()
        {
            ISell dal = baseDal as ISell;
            return dal.GetOrderYearList();
        }

        /// <summary>
        /// 获取客户当前记录所有的次数(仅当订单已发货）
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <returns></returns>
        public int GetOrderCount(string customerId, DbTransaction trans = null)
        {
            string condition = string.Format("Customer_ID='{0}' and IsShipped > 0", customerId);
            return GetRecordCount(condition, trans);
        }

        /// <summary>
        /// 获取客户订单记录的总金额（折后金额）(仅当订单已发货）
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public decimal GetOrderAmout(string customerId, DbTransaction trans = null)
        {
            string sql = string.Format("Select Sum(DiscountAmount) from T_CRM_Sell where Customer_ID='{0}' and IsShipped > 0", customerId);
            return SqlValueList(sql, trans).ToDecimal();
        }

        /// <summary>
        /// 更新客户的相关交易信息
        /// </summary>
        private bool UpdateTransactionStatus(SellInfo obj, DbTransaction trans = null)
        {
            //如果订单增加，需要调整客户信息里面的相关交易状态：交易次数(+)，交易金额(+)，最近交易时间，首次交易时间
            int orderCount = GetOrderCount(obj.Customer_ID, trans);
            decimal orderAmout = GetOrderAmout(obj.Customer_ID, trans);

            bool result =  BLLFactory<Customer>.Instance.UpdateTransactionStatus(obj.Customer_ID, obj.OrderDate, orderCount, orderAmout, trans);
            return result;
        }

        /// <summary>
        /// 订单发货，并从产品数量中减少相关的订单数量
        /// </summary>
        /// <returns></returns>
        public bool OrderShipped(string orderNumber, DbTransaction trans = null)
        {
            bool result = false;
            SellInfo info = FindByOrderNumber(orderNumber, trans);
            if (info != null)
            {
                info.IsShipped = true;
                result = base.Update(info, info.ID, trans);
                
                //第一次发货的时候，减少库存数量
                List<OrderDetailInfo> detailList = BLLFactory<OrderDetail>.Instance.FindByOrderNo(orderNumber, trans);
                foreach (OrderDetailInfo detailInfo in detailList)
                {
                    int quantity = detailInfo.Quantity * (-1);//减少库存数量
                    BLLFactory<Product>.Instance.ModifyQuantity(detailInfo.Product_ID, quantity, trans);
                }

                //如果订单发货，则需要更新订单状态
                UpdateTransactionStatus(info, trans);
            }

            return result;
        }

        /// <summary>
        /// 订单取消，并从产品数量中减少相关的订单数量（如果已发货）
        /// </summary>
        /// <returns></returns>
        public bool OrderCancel(string orderNumber, DbTransaction trans = null)
        {
            bool result = false;
            SellInfo info = FindByOrderNumber(orderNumber, trans);
            if (info != null)
            {
                //如果已发货，取消订单则返回增加库存数量
                if (info.IsShipped)
                {
                    List<OrderDetailInfo> detailList = BLLFactory<OrderDetail>.Instance.FindByOrderNo(orderNumber, trans);
                    foreach (OrderDetailInfo detailInfo in detailList)
                    {
                        int quantity = detailInfo.Quantity * (1);//返回增加库存数量
                        BLLFactory<Product>.Instance.ModifyQuantity(detailInfo.Product_ID, quantity, trans);
                    }
                }

                //不管是否已发货，都设置为取消发货状态
                info.IsShipped = false;
                info.OrderStatus = "已取消";
                result = base.Update(info, info.ID, trans);

                //需要更新订单状态
                UpdateTransactionStatus(info, trans);
            }

            return result;
        }

        /// <summary>
        /// 删除订单及订单明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteOrderRelated(string id)
        {
            bool result = false;
            DbTransaction trans = CreateTransaction();
            if (trans != null)
            {
                SellInfo info = baseDal.FindByID(id, trans);
                if (info != null)
                {
                    List<OrderDetailInfo> detailList = BLLFactory<OrderDetail>.Instance.FindByOrderNo(info.OrderNo, trans);
                    foreach (OrderDetailInfo detailInfo in detailList)
                    {
                        BLLFactory<OrderDetail>.Instance.Delete(detailInfo.ID, trans);
                    }

                    //最后删除主表订单数据
                    baseDal.Delete(id, trans);

                    try
                    {
                        trans.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        LogTextHelper.Error(ex);
                        throw;
                    }
                }                
            }
            return result;
        }

        /// <summary>
        /// 获取订单统计报表的一些数据
        /// </summary>
        /// <param name="fieldName">统计字段名称，可选为ProductName, ProductType, Model</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataTable GetDetailStatData(string fieldName, string condition)
        {
            ISell dal = baseDal as ISell;
            return dal.GetDetailStatData(fieldName, condition);
        }
    }
}
