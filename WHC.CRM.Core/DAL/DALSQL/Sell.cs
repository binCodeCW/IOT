using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.CRM.Entity;
using WHC.CRM.IDAL;

namespace WHC.CRM.DALSQL
{
    /// <summary>
    /// 产品销售记录
    /// </summary>
	public class Sell : BaseDALSQL<SellInfo>, ISell
	{
		#region 对象实例及构造函数

		public static Sell Instance
		{
			get
			{
				return new Sell();
			}
		}
		public Sell() : base("T_CRM_Sell","ID")
        {
            this.SortField = "OrderDate";
            this.IsDescending = true;
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override SellInfo DataReaderToEntity(IDataReader dataReader)
		{
			SellInfo info = new SellInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Customer_ID = reader.GetString("Customer_ID");
			info.Conatct_ID = reader.GetString("Conatct_ID");
			info.OrderNo = reader.GetString("OrderNo");
			info.Contact = reader.GetString("Contact");
			info.ContactPhone = reader.GetString("ContactPhone");
			info.ContactMobile = reader.GetString("ContactMobile");
			info.Quantity = reader.GetInt32("Quantity");
			info.Amount = reader.GetDecimal("Amount");
			info.DiscountAmount = reader.GetDecimal("DiscountAmount");
			info.ReceivedMoney = reader.GetDecimal("ReceivedMoney");
			info.DiscountNote = reader.GetString("DiscountNote");
			info.PaymentType = reader.GetString("PaymentType");
			info.Operator = reader.GetString("Operator");
			info.OrderDate = reader.GetDateTime("OrderDate");
			info.OrderStatus = reader.GetString("OrderStatus");
            info.IsShipped = reader.GetInt32("IsShipped") > 0;
			info.RequiredDate = reader.GetDateTime("RequiredDate");
			info.ShipAddress = reader.GetString("ShipAddress");
			info.ShipTelephone = reader.GetString("ShipTelephone");
			info.ReceiveMan = reader.GetString("ReceiveMan");
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
        protected override Hashtable GetHashByEntity(SellInfo obj)
		{
		    SellInfo info = obj as SellInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Customer_ID", info.Customer_ID);
 			hash.Add("Conatct_ID", info.Conatct_ID);
 			hash.Add("OrderNo", info.OrderNo);
 			hash.Add("Contact", info.Contact);
 			hash.Add("ContactPhone", info.ContactPhone);
 			hash.Add("ContactMobile", info.ContactMobile);
 			hash.Add("Quantity", info.Quantity);
 			hash.Add("Amount", info.Amount);
 			hash.Add("DiscountAmount", info.DiscountAmount);
 			hash.Add("ReceivedMoney", info.ReceivedMoney);
 			hash.Add("DiscountNote", info.DiscountNote);
 			hash.Add("PaymentType", info.PaymentType);
 			hash.Add("Operator", info.Operator);
 			hash.Add("OrderDate", info.OrderDate);
 			hash.Add("OrderStatus", info.OrderStatus);
            hash.Add("IsShipped", info.IsShipped ? 1 : 0);
 			hash.Add("RequiredDate", info.RequiredDate);
 			hash.Add("ShipAddress", info.ShipAddress);
 			hash.Add("ShipTelephone", info.ShipTelephone);
 			hash.Add("ReceiveMan", info.ReceiveMan);
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
            dict.Add("OrderNo", "订单编号");
            dict.Add("Contact", "联系人");
            dict.Add("ContactPhone", "联系人电话");
            dict.Add("ContactMobile", "联系人手机");
            dict.Add("Quantity", "销售数量");
            dict.Add("Amount", "总金额");
            dict.Add("DiscountAmount", "折后金额");
            dict.Add("ReceivedMoney", "已收金额");
            dict.Add("DiscountNote", "优惠折扣");
            dict.Add("PaymentType", "支付方式");
            dict.Add("Operator", "经办人");
            dict.Add("OrderDate", "订单日期");
            dict.Add("OrderStatus", "订单状态");
            dict.Add("IsShipped", "是否已发货");
            dict.Add("RequiredDate", "要求到货日期");
            dict.Add("ShipAddress", "收货地址");
            dict.Add("ShipTelephone", "收货电话");
            dict.Add("ReceiveMan", "收货人员");
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
        /// 获取订单日期年度列表
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

        /// <summary>
        /// 获取订单统计报表的一些数据
        /// </summary>
        /// <param name="fieldName">统计字段名称，可选为ProductName, ProductType, Model</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataTable GetDetailStatData(string fieldName, string condition)
        {
            string where = "";
            if (!string.IsNullOrEmpty(condition))
            {
                condition = condition.ToLower().Replace("createtime", "d.createtime");
                where = string.Format("Where {0}", condition);
            }

            string sql = string.Format(@"select ProductName, ProductType, Model, d.Quantity,d.SubAmout,d.Color,d.ProductSize, d.CreateTime
            from T_CRM_OrderDetail d inner join T_CRM_Sell s on d.OrderNo = s.OrderNo {0} and s.IsShipped = 1", where);

            sql = string.Format("select {0} as argument, count(*) as datavalue from ({1}) A  group by A.{0} order by count(*) desc", fieldName, sql);

            return SqlTable(sql);
        }
    }
}