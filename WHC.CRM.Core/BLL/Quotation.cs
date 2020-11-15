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
    /// 产品报价单
    /// </summary>
	public class Quotation : BaseBLL<QuotationInfo>
    {
        public Quotation() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 生成单据号码
        /// </summary>
        /// <param name="userId">操作人员ID</param>
        /// <returns></returns>
        public string GetHandNo(int userId)
        {
            string prefix = string.Format("BJ-{0}-{1}", userId, DateTime.Now.ToString("yyyyMMdd"));

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
                if (CheckNumberExist(number))
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

        private bool CheckNumberExist(string handNo)
        {
            return base.IsExistKey("HandNo", handNo);
        }

        /// <summary>
        /// 根据报价单编号获取对象信息
        /// </summary>
        /// <param name="orderNo">报价单编号</param>
        /// <returns></returns>
        public QuotationInfo FindByOrderNo(string orderNo, DbTransaction trans = null)
        {
            string condition = string.Format("HandNo = '{0}'", orderNo);
            return FindSingle(condition, trans);
        }

        /// <summary>
        /// 把报价单转换为销售订单
        /// </summary>
        /// <param name="quotationNo">报价单编号</param>
        /// <returns></returns>
        public bool ConvertToOrder(string orderNo, int userId)
        {
            bool result = false;

            DbTransaction trans = baseDal.CreateTransaction();
            if (trans != null)
            {
                SellInfo sellInfo = ConvertSellInfo(orderNo, userId, trans);
                List<OrderDetailInfo> detailList = new List<OrderDetailInfo>();
                if (sellInfo != null)
                {
                    detailList = ConvertOrderDetal(sellInfo, orderNo, trans);
                }

                bool success = BLLFactory<Sell>.Instance.Insert(sellInfo, trans);
                if (success)
                {
                    foreach (OrderDetailInfo info in detailList)
                    {
                        BLLFactory<OrderDetail>.Instance.Insert(info, trans);
                    }                    
                }

                try
                {
                    trans.Commit();
                    result = true;
                }
                catch
                {
                    trans.Rollback();
                    throw;//重新抛出异常
                }
            }
            return result;
        }

        private SellInfo ConvertSellInfo(string orderNo, int userId, DbTransaction trans = null)
        {
            SellInfo result = null;
            QuotationInfo info = FindByOrderNo(orderNo, trans);
            if (info != null)
            {
                result = new SellInfo();
                result.Amount = info.Amount;
                result.AttachGUID = info.AttachGUID;
                result.Conatct_ID = info.Conatct_ID;
                result.Contact = info.Contact;
                result.ContactMobile = info.ContactMobile;
                result.ContactPhone = info.ContactPhone;
                result.Customer_ID = info.Customer_ID;
                result.DiscountAmount = info.DiscountAmount;
                result.DiscountNote = info.DiscountNote;
                result.Note = info.Note;
                result.Operator = info.Operator;
                result.OrderDate = info.OrderDate;
                //result.OrderStatus = info.OrderStatus; //报价单的状态和销售单不同，不能复制过去
                result.Quantity = info.Quantity;
                result.ReceivedMoney = info.ReceivedMoney;
                result.Creator = info.Editor;
                result.Operator = info.Operator;
                result.CreateTime = DateTime.Now;
                result.Editor = info.Editor;
                result.EditTime = DateTime.Now;
                result.Company_ID = info.Company_ID;
                result.Dept_ID = info.Dept_ID;

                result.OrderDate = DateTime.Now;
                result.OrderNo = BLLFactory<Sell>.Instance.GetOrderNo(userId);//生成订单编号
                result.Note = info.HandNo;//备注为报价单

                //更新报价单状态
                info.OrderStatus = "已生成订单";
                baseDal.Update(info, info.ID, trans);
            }
            return result;
        }

        private List<OrderDetailInfo> ConvertOrderDetal(SellInfo sellInfo,string orderNo, DbTransaction trans = null)
        {
            List<OrderDetailInfo> list = new List<OrderDetailInfo>();
            List<QuotationDetailInfo> quotationList = BLLFactory<QuotationDetail>.Instance.FindByOrderNo(orderNo, trans);
            foreach (QuotationDetailInfo quotationInfo in quotationList)
            {
                OrderDetailInfo info = new OrderDetailInfo();
                info.BarCode = quotationInfo.BarCode;
                info.Color = quotationInfo.Color;
                info.Conatct_ID = quotationInfo.Conatct_ID;
                info.Customer_ID = quotationInfo.Customer_ID;
                info.Editor = quotationInfo.Editor;
                info.EditTime = DateTime.Now;
                info.ExpireDate = quotationInfo.ExpireDate;
                info.MaterialCode = quotationInfo.MaterialCode;
                info.Model = quotationInfo.Model;
                info.Note = quotationInfo.Note;
                info.OrderNo = sellInfo.OrderNo;//
                info.PinyinCode = quotationInfo.PinyinCode;
                info.Product_ID = quotationInfo.Product_ID;
                info.ProductName = quotationInfo.ProductName;
                info.ProductNo = quotationInfo.ProductNo;
                info.ProductSize = quotationInfo.ProductSize;
                info.ProductType = quotationInfo.ProductType;
                info.Quantity = quotationInfo.Quantity;
                info.SalePrice = quotationInfo.SalePrice;
                info.Specification = quotationInfo.Specification;
                info.SubAmout = quotationInfo.SubAmout;
                info.Unit = quotationInfo.Unit;

                list.Add(info);
            }

            return list;
        }

        /// <summary>
        /// 获取报价单日期年度列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetOrderYearList()
        {
            IQuotation dal = baseDal as IQuotation;
            return dal.GetOrderYearList();
        }

        /// <summary>
        /// 删除报价单及明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteQuotationRelated(string id)
        {
            bool result = false;
            DbTransaction trans = CreateTransaction();
            if (trans != null)
            {
                QuotationInfo info = baseDal.FindByID(id, trans);
                if (info != null)
                {
                    List<QuotationDetailInfo> detailList = BLLFactory<QuotationDetail>.Instance.FindByOrderNo(info.HandNo, trans);
                    foreach (QuotationDetailInfo detailInfo in detailList)
                    {
                        BLLFactory<QuotationDetail>.Instance.Delete(detailInfo.ID, trans);
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
    }
}
