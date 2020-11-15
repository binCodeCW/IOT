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
    /// 供应商
    /// </summary>
	public class Supplier : BaseDALSQL<SupplierInfo>, ISupplier
	{
		#region 对象实例及构造函数

		public static Supplier Instance
		{
			get
			{
				return new Supplier();
			}
		}
		public Supplier() : base("T_CRM_Supplier","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override SupplierInfo DataReaderToEntity(IDataReader dataReader)
		{
			SupplierInfo info = new SupplierInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.HandNo = reader.GetString("HandNo");
			info.Name = reader.GetString("Name");
			info.SimpleName = reader.GetString("SimpleName");
			info.Province = reader.GetString("Province");
			info.City = reader.GetString("City");
			info.District = reader.GetString("District");
			info.Area = reader.GetString("Area");
			info.Address = reader.GetString("Address");
			info.ZipCode = reader.GetString("ZipCode");
			info.Telephone = reader.GetString("Telephone");
			info.Fax = reader.GetString("Fax");
			info.Contact = reader.GetString("Contact");
			info.ContactPhone = reader.GetString("ContactPhone");
			info.ContactMobile = reader.GetString("ContactMobile");
			info.Email = reader.GetString("Email");
			info.QQ = reader.GetString("QQ");
			info.Industry = reader.GetString("Industry");
			info.BusinessScope = reader.GetString("BusinessScope");
			info.Brand = reader.GetString("Brand");
			info.PrimaryClient = reader.GetString("PrimaryClient");
			info.PrimaryBusiness = reader.GetString("PrimaryBusiness");
			info.RegisterCapital = reader.GetDecimal("RegisterCapital");
			info.TurnOver = reader.GetDecimal("TurnOver");
			info.LicenseNo = reader.GetString("LicenseNo");
			info.Bank = reader.GetString("Bank");
			info.BankAccount = reader.GetString("BankAccount");
			info.LocalTaxNo = reader.GetString("LocalTaxNo");
			info.NationalTaxNo = reader.GetString("NationalTaxNo");
			info.LegalMan = reader.GetString("LegalMan");
			info.LegalTelephone = reader.GetString("LegalTelephone");
			info.LegalMobile = reader.GetString("LegalMobile");
			info.Source = reader.GetString("Source");
			info.WebSite = reader.GetString("WebSite");
			info.CompanyPictureGUID = reader.GetString("CompanyPictureGUID");
			info.AttachGUID = reader.GetString("AttachGUID");
			info.CustomerType = reader.GetString("CustomerType");
			info.Grade = reader.GetString("Grade");
			info.CreditStatus = reader.GetString("CreditStatus");
			info.Importance = reader.GetString("Importance");
            info.IsPublic = reader.GetInt32("IsPublic") > 0;
            info.Satisfaction = reader.GetInt32("Satisfaction");
			info.Note = reader.GetString("Note");
			info.TransactionCount = reader.GetInt32("TransactionCount");
			info.TransactionTotal = reader.GetDecimal("TransactionTotal");
			info.TransactionFirstDay = reader.GetDateTime("TransactionFirstDay");
			info.TransactionLastDay = reader.GetDateTime("TransactionLastDay");
			info.LastContactDate = reader.GetDateTime("LastContactDate");
			info.Stage = reader.GetString("Stage");
			info.Status = reader.GetString("Status");
			info.Creator = reader.GetString("Creator");
			info.CreateTime = reader.GetDateTime("CreateTime");
			info.Editor = reader.GetString("Editor");
			info.EditTime = reader.GetDateTime("EditTime");
            info.Deleted = reader.GetInt32("Deleted") > 0;
            info.Dept_ID = reader.GetString("Dept_ID");
			info.Company_ID = reader.GetString("Company_ID");
			info.MarkColor = reader.GetString("MarkColor");
            info.ShareUsers = reader.GetString("ShareUsers");

            return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(SupplierInfo obj)
		{
		    SupplierInfo info = obj as SupplierInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("HandNo", info.HandNo);
 			hash.Add("Name", info.Name);
 			hash.Add("SimpleName", info.SimpleName);
 			hash.Add("Province", info.Province);
 			hash.Add("City", info.City);
 			hash.Add("District", info.District);
 			hash.Add("Area", info.Area);
 			hash.Add("Address", info.Address);
 			hash.Add("ZipCode", info.ZipCode);
 			hash.Add("Telephone", info.Telephone);
 			hash.Add("Fax", info.Fax);
 			hash.Add("Contact", info.Contact);
 			hash.Add("ContactPhone", info.ContactPhone);
 			hash.Add("ContactMobile", info.ContactMobile);
 			hash.Add("Email", info.Email);
 			hash.Add("QQ", info.QQ);
 			hash.Add("Industry", info.Industry);
 			hash.Add("BusinessScope", info.BusinessScope);
 			hash.Add("Brand", info.Brand);
 			hash.Add("PrimaryClient", info.PrimaryClient);
 			hash.Add("PrimaryBusiness", info.PrimaryBusiness);
 			hash.Add("RegisterCapital", info.RegisterCapital);
 			hash.Add("TurnOver", info.TurnOver);
 			hash.Add("LicenseNo", info.LicenseNo);
 			hash.Add("Bank", info.Bank);
 			hash.Add("BankAccount", info.BankAccount);
 			hash.Add("LocalTaxNo", info.LocalTaxNo);
 			hash.Add("NationalTaxNo", info.NationalTaxNo);
 			hash.Add("LegalMan", info.LegalMan);
 			hash.Add("LegalTelephone", info.LegalTelephone);
 			hash.Add("LegalMobile", info.LegalMobile);
 			hash.Add("Source", info.Source);
 			hash.Add("WebSite", info.WebSite);
 			hash.Add("CompanyPictureGUID", info.CompanyPictureGUID);
 			hash.Add("AttachGUID", info.AttachGUID);
 			hash.Add("CustomerType", info.CustomerType);
 			hash.Add("Grade", info.Grade);
 			hash.Add("CreditStatus", info.CreditStatus);
 			hash.Add("Importance", info.Importance);
            hash.Add("IsPublic", info.IsPublic ? 1 : 0);
            hash.Add("Satisfaction", info.Satisfaction);
 			hash.Add("Note", info.Note);
 			hash.Add("TransactionCount", info.TransactionCount);
 			hash.Add("TransactionTotal", info.TransactionTotal);
 			hash.Add("TransactionFirstDay", info.TransactionFirstDay);
 			hash.Add("TransactionLastDay", info.TransactionLastDay);
 			hash.Add("LastContactDate", info.LastContactDate);
 			hash.Add("Stage", info.Stage);
 			hash.Add("Status", info.Status);
 			hash.Add("Creator", info.Creator);
 			hash.Add("CreateTime", info.CreateTime);
 			hash.Add("Editor", info.Editor);
 			hash.Add("EditTime", info.EditTime);
            hash.Add("Deleted", info.Deleted ? 1 : 0);
            hash.Add("Dept_ID", info.Dept_ID);
 			hash.Add("Company_ID", info.Company_ID);
 			hash.Add("MarkColor", info.MarkColor);
            hash.Add("ShareUsers", info.ShareUsers);

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
            //dict.Add("ID", "编号");
            dict.Add("ID", "");
            dict.Add("HandNo", "编号");
            dict.Add("Name", "供应商名称");
            dict.Add("SimpleName", "供应商简称");
            dict.Add("Province", "所在省份");
            dict.Add("City", "城市");
            dict.Add("District", "所在行政区");
            dict.Add("Area", "市场分区");
            dict.Add("Address", "公司地址");
            dict.Add("ZipCode", "公司邮编");
            dict.Add("Telephone", "办公电话");
            dict.Add("Fax", "传真号码");
            dict.Add("Contact", "主联系人");
            dict.Add("ContactPhone", "联系人电话");
            dict.Add("ContactMobile", "联系人手机");
            dict.Add("Email", "电子邮件");
            dict.Add("QQ", "QQ号码");
            dict.Add("Industry", "所属行业");
            dict.Add("BusinessScope", "经营范围");
            dict.Add("Brand", "经营品牌");
            dict.Add("PrimaryClient", "主要客户群");
            dict.Add("PrimaryBusiness", "主营业务");
            dict.Add("RegisterCapital", "注册资金");
            dict.Add("TurnOver", "营业额");
            dict.Add("LicenseNo", "营业执照");
            dict.Add("Bank", "开户银行");
            dict.Add("BankAccount", "银行账号");
            dict.Add("LocalTaxNo", "地税登记号");
            dict.Add("NationalTaxNo", "国税登记号");
            dict.Add("LegalMan", "法人名称");
            dict.Add("LegalTelephone", "法人电话");
            dict.Add("LegalMobile", "法人手机");
            dict.Add("Source", "供应商来源");
            dict.Add("WebSite", "单位网站");
            dict.Add("CompanyPictureGUID", "公司图片信息");
            dict.Add("AttachGUID", "附件组别ID");
            dict.Add("CustomerType", "供应商类别");
            dict.Add("Grade", "供应商级别");
            dict.Add("CreditStatus", "信用等级");
            dict.Add("Importance", "重要级别");
            dict.Add("IsPublic", "公开与否");
            dict.Add("Satisfaction", "客户满意度");
            dict.Add("Note", "备注信息");
            dict.Add("TransactionCount", "交易次数");
            dict.Add("TransactionTotal", "交易金额");
            dict.Add("TransactionFirstDay", "首次交易时间");
            dict.Add("TransactionLastDay", "最近交易时间");
            dict.Add("LastContactDate", "最近联系日期");
            dict.Add("Stage", "客户阶段");
            dict.Add("Status", "客户状态");
            dict.Add("Creator", "创建人");
            dict.Add("CreateTime", "创建时间");
            dict.Add("Editor", "编辑人");
            dict.Add("EditTime", "编辑时间");
            dict.Add("Deleted", "是否已删除");
            dict.Add("Dept_ID", "所属部门");
            dict.Add("Company_ID", "所属公司");
            dict.Add("MarkColor", "标记颜色");
            dict.Add("ShareUsers", "业务分享用户");
            #endregion

            return dict;
        }

        /// <summary>
        /// 设置删除标志
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <param name="deleted">是否删除</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public bool SetDeletedFlag(string id, bool deleted = true, DbTransaction trans = null)
        {
            int intDeleted = deleted ? 1 : 0;
            string sql = string.Format("Update {0} Set Deleted={1} Where ID = '{2}' ", tableName, intDeleted, id);
            return SqlExecute(sql, trans) > 0;
        }

        /// <summary>
        /// 根据供应商分组的名称，搜索属于该分组的供应商列表
        /// </summary>
        /// <param name="ownerUser">供应商所属用户</param>
        /// <param name="groupName">供应商分组的名称,如果供应商分组为空，那么返回未分组供应商列表</param>
        /// <param name="pagerInfo">分页条件</param>
        /// <returns></returns>
        public List<SupplierInfo> FindByGroupName(string ownerUser, string groupName, string condition, PagerInfo pagerInfo = null)
        {
            //所属用户条件,非删除状态
            string where = string.Format(" AND t.Creator='{0}' AND Deleted=0", ownerUser);
            if (!string.IsNullOrEmpty(condition))
            {
                where += " AND " + condition;
            }

            string sql = "";
            if (string.IsNullOrEmpty(groupName))
            {
                sql = string.Format("Select t.* from {0} t where ID not In (select Supplier_ID from T_CRM_SupplierGroup_Supplier) {1}", tableName, where);
            }
            else
            {
                string subSql = string.Format("select ID from T_CRM_SupplierGroup g where g.Name ='{0}' ", groupName);
                sql = string.Format(@"select t.* from {0} t inner join T_CRM_SupplierGroup_Supplier m on t.ID = m.Supplier_ID 
                where m.SupplierGroup_ID in ({1}) {2} ", tableName, subSql, where);
            }

            if (pagerInfo != null)
            {
                return base.GetListWithPager(sql, pagerInfo);
            }
            else
            {
                return base.GetList(sql);
            }
        }

        /// <summary>
        /// 调整供应商的组别
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <param name="groupIdList">供应商分组Id集合</param>
        /// <returns></returns>
        public bool ModifyGroup(string id, List<string> groupIdList)
        {
            bool result = false;
            DbTransaction trans = base.CreateTransaction();
            if (trans != null)
            {
                string sql = string.Format("Delete from T_CRM_SupplierGroup_Supplier where Supplier_ID='{0}' ", id);
                base.SqlExecute(sql, trans);

                foreach (string groupId in groupIdList)
                {
                    sql = string.Format("Insert into T_CRM_SupplierGroup_Supplier(Supplier_ID,SupplierGroup_ID) values('{0}', '{1}') ", id, groupId);
                    base.SqlExecute(sql, trans);
                }

                try
                {
                    trans.Commit();
                    result = true;
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// 更新供应商的状态信息
        /// </summary>
        /// <param name="id">供应商Id</param>
        /// <param name="orderDate">订单日期</param>
        /// <param name="orderCount">交易次数</param>
        /// <param name="orderMoney">交易金额</param>
        /// <returns></returns>
        public bool UpdateTransactionStatus(string id, DateTime orderDate, int orderCount, decimal orderMoney, DbTransaction trans = null)
        {
            bool result = false;

            Database db = CreateDatabase();

            string sql = string.Format("update {0} set TransactionCount = {2} where ID='{1}' ", tableName, id, orderCount);
            DbCommand command = db.GetSqlStringCommand(sql);
            if (trans != null)
            {
                db.ExecuteNonQuery(command, trans);
            }
            else
            {
                db.ExecuteNonQuery(command);
            }

            sql = string.Format("update {0} set TransactionTotal = {2} where ID='{1}' ", tableName, id, orderMoney);
            command = db.GetSqlStringCommand(sql);
            if (trans != null)
            {
                db.ExecuteNonQuery(command, trans);
            }
            else
            {
                db.ExecuteNonQuery(command);
            }

            sql = string.Format("update {0} set TransactionFirstDay = {2}TransactionFirstDay where ID='{1}' AND TransactionFirstDay is null ", tableName, id, ParameterPrefix);
            command = db.GetSqlStringCommand(sql);
            db.AddInParameter(command, ParameterPrefix + "TransactionFirstDay", DbType.DateTime, orderDate);
            if (trans != null)
            {
                db.ExecuteNonQuery(command, trans);
            }
            else
            {
                db.ExecuteNonQuery(command);
            }

            //仅当日期为最新才更新
            sql = string.Format("update {0} set TransactionLastDay = {2}TransactionLastDay where ID='{1}' and (TransactionLastDay < {2}TransactionLastDay or TransactionLastDay is null) ", tableName, id, ParameterPrefix);
            command = db.GetSqlStringCommand(sql);
            db.AddInParameter(command, ParameterPrefix + "TransactionLastDay", DbType.DateTime, orderDate);
            if (trans != null)
            {
                db.ExecuteNonQuery(command, trans);
            }
            else
            {
                db.ExecuteNonQuery(command);
            }

            result = true;//最后设置为True

            return result;
        }

        /// <summary>
        /// 更新供应商的最后联系日期
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <param name="lastContactDate">最后联系日期</param>
        /// <returns></returns>
        public bool UpdateContactDate(string id, DateTime lastContactDate, DbTransaction trans = null)
        {
            bool result = false;

            Database db = CreateDatabase();

            //仅当日期为最新才更新
            string sql = string.Format("update {0} set LastContactDate = {2}LastContactDate where ID='{1}' and (LastContactDate < {2}LastContactDate or LastContactDate is null) ", tableName, id, ParameterPrefix);
            DbCommand command = db.GetSqlStringCommand(sql);
            db.AddInParameter(command, ParameterPrefix + "LastContactDate", DbType.DateTime, lastContactDate);
            if (trans != null)
            {
                result = db.ExecuteNonQuery(command, trans) > 0;
            }
            else
            {
                result = db.ExecuteNonQuery(command) > 0;
            }

            return result;
        }

        /// <summary>
        /// 根据客户ID获取供应商关联ID
        /// </summary>
        /// <param name="customerID">客户ID</param>
        /// <returns></returns>
        public List<string> GetSupplierByCustomer(string customerID)
        {
            List<string> list = new List<string>();

            string sql = string.Format("Select Supplier_ID from T_CRM_Customer_Supplier  where Customer_ID ='{0}' ", customerID);
            string result = SqlValueList(sql);
            if (!string.IsNullOrEmpty(result))
            {
                list = result.ToDelimitedList<string>(",");
            }
            return list;
        }
        
        /// <summary>
        /// 根据供应商所属客户ID，分页获取供应商列表
        /// </summary>
        /// <param name="customerID">供应商所属客户ID</param>
        /// <param name="pagerInfo">分页条件</param>
        /// <returns></returns>
        public List<SupplierInfo> FindByCustomer(string customerID, string condition, PagerInfo pagerInfo = null)
        {
            string sql = "";

            sql = string.Format(@"select t.* from {0} t inner join T_CRM_Customer_Supplier m on t.ID = m.Supplier_ID 
            where m.Customer_ID ='{1}' ", tableName, customerID);
            if (!string.IsNullOrEmpty(condition))
            {
                sql += string.Format("AND {0}", condition);
            }

            if (pagerInfo != null)
            {
                return base.GetListWithPager(sql, pagerInfo);
            }
            else
            {
                return base.GetList(sql);
            }
        }
    }
}