using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.CRM.Entity;
using WHC.CRM.IDAL;

namespace WHC.CRM.DALSQL
{
    /// <summary>
    /// 产品报价单
    /// </summary>
    public class Quotation : BaseDALSQL<QuotationInfo>, IQuotation
    {
        #region 对象实例及构造函数

        public static Quotation Instance
        {
            get
            {
                return new Quotation();
            }
        }
        public Quotation()
            : base("T_CRM_Quotation", "ID")
        {
            this.SortField = "CreateTime";
            this.IsDescending = true;
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override QuotationInfo DataReaderToEntity(IDataReader dataReader)
        {
            QuotationInfo info = new QuotationInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.Customer_ID = reader.GetString("Customer_ID");
            info.Conatct_ID = reader.GetString("Conatct_ID");
            info.HandNo = reader.GetString("HandNo");
            info.Contact = reader.GetString("Contact");
            info.ContactPhone = reader.GetString("ContactPhone");
            info.ContactMobile = reader.GetString("ContactMobile");
            info.Quantity = reader.GetInt32("Quantity");
            info.Amount = reader.GetDecimal("Amount");
            info.DiscountAmount = reader.GetDecimal("DiscountAmount");
            info.ReceivedMoney = reader.GetDecimal("ReceivedMoney");
            info.DiscountNote = reader.GetString("DiscountNote");
            info.Operator = reader.GetString("Operator");
            info.OrderDate = reader.GetDateTime("OrderDate");
            info.OrderStatus = reader.GetString("OrderStatus");
            info.AttachGUID = reader.GetString("AttachGUID");
            info.Note = reader.GetString("Note");
			info.Creator = reader.GetString("Creator");
			info.CreateTime = reader.GetDateTime("CreateTime");
            info.Editor = reader.GetString("Editor");
            info.EditTime = reader.GetDateTime("EditTime");
            info.Dept_ID = reader.GetString("Dept_ID");
            info.Company_ID = reader.GetString("Company_ID");
            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(QuotationInfo obj)
        {
            QuotationInfo info = obj as QuotationInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("Customer_ID", info.Customer_ID);
            hash.Add("Conatct_ID", info.Conatct_ID);
            hash.Add("HandNo", info.HandNo);
            hash.Add("Contact", info.Contact);
            hash.Add("ContactPhone", info.ContactPhone);
            hash.Add("ContactMobile", info.ContactMobile);
            hash.Add("Quantity", info.Quantity);
            hash.Add("Amount", info.Amount);
            hash.Add("DiscountAmount", info.DiscountAmount);
            hash.Add("ReceivedMoney", info.ReceivedMoney);
            hash.Add("DiscountNote", info.DiscountNote);
            hash.Add("Operator", info.Operator);
            hash.Add("OrderDate", info.OrderDate);
            hash.Add("OrderStatus", info.OrderStatus);
            hash.Add("AttachGUID", info.AttachGUID);
            hash.Add("Note", info.Note);
 			hash.Add("Creator", info.Creator);
 			hash.Add("CreateTime", info.CreateTime);
            hash.Add("Editor", info.Editor);
            hash.Add("EditTime", info.EditTime);
            hash.Add("Dept_ID", info.Dept_ID);
            hash.Add("Company_ID", info.Company_ID);
            return hash;
        }

        /// <summary>
        /// 获取字段中文别名（用于界面显示）的字典集合
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> GetColumnNameAlias()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            #region 添加别名解析
            dict.Add("ID", "编号");
            dict.Add("Customer_ID", "客户名称");
            dict.Add("Conatct_ID", "客户联系人");
            dict.Add("HandNo", "报价单编号");
            dict.Add("Contact", "联系人");
            dict.Add("ContactPhone", "联系人电话");
            dict.Add("ContactMobile", "联系人手机");
            dict.Add("Quantity", "销售数量");
            dict.Add("Amount", "总金额");
            dict.Add("DiscountAmount", "折后金额");
            dict.Add("ReceivedMoney", "已收金额");
            dict.Add("DiscountNote", "优惠折扣");
            dict.Add("Operator", "经办人");
            dict.Add("OrderDate", "报价单日期");
            dict.Add("OrderStatus", "报价单状态");
            dict.Add("AttachGUID", "附件组别ID");
            dict.Add("Note", "备注信息");
            dict.Add("Creator", "创建人");
            dict.Add("CreateTime", "创建时间");
            dict.Add("Editor", "编辑人");
            dict.Add("EditTime", "编辑时间");
            dict.Add("Dept_ID", "所属部门");
            dict.Add("Company_ID", "所属公司");
            #endregion

            return dict;
        }

        /// <summary>
        /// 获取报价日期年度列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetOrderYearList()
        {
            List<string> list = new List<string>();
            string sql = string.Format("Select distinct year(OrderDate) as OrderYear From {0} order by OrderYear desc", tableName);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            using (IDataReader dr = db.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    string number = dr[0].ToString();
                    if (!string.IsNullOrEmpty(number))
                    {
                        list.Add(number);
                    }
                }
            }
            return list;
        }

    }
}