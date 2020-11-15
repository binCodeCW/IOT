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
    /// 客户合同信息
    /// </summary>
	public class Contract : BaseDALSQL<ContractInfo>, IContract
	{
		#region 对象实例及构造函数

		public static Contract Instance
		{
			get
			{
				return new Contract();
			}
		}
		public Contract() : base("T_CRM_Contract","ID")
        {
            this.SortField = "SignDate";
            this.IsDescending = true;
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override ContractInfo DataReaderToEntity(IDataReader dataReader)
		{
			ContractInfo info = new ContractInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Customer_ID = reader.GetString("Customer_ID");
			info.HandNo = reader.GetString("HandNo");
			info.ExpenditureType = reader.GetString("ExpenditureType");
			info.ContractType = reader.GetString("ContractType");
			info.ContractName = reader.GetString("ContractName");
			info.ContractMoney = reader.GetDecimal("ContractMoney");
			info.CompanySigner = reader.GetString("CompanySigner");
			info.CustomerSigner = reader.GetString("CustomerSigner");
			info.SignDate = reader.GetDateTime("SignDate");
			info.SignLocation = reader.GetString("SignLocation");
			info.PartyBName = reader.GetString("PartyBName");
			info.StartDate = reader.GetDateTime("StartDate");
			info.EndDate = reader.GetDateTime("EndDate");
			info.Settlement = reader.GetString("Settlement");
			info.Status = reader.GetString("Status");
			info.RelatedItems = reader.GetString("RelatedItems");
			info.Contact = reader.GetString("Contact");
			info.ContactPhone = reader.GetString("ContactPhone");
			info.ContactMobile = reader.GetString("ContactMobile");
			info.Content = reader.GetString("Content");
			info.Note = reader.GetString("Note");
			info.AttachGUID = reader.GetString("AttachGUID");
			info.Operator = reader.GetString("Operator");
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
        protected override Hashtable GetHashByEntity(ContractInfo obj)
		{
		    ContractInfo info = obj as ContractInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Customer_ID", info.Customer_ID);
 			hash.Add("HandNo", info.HandNo);
 			hash.Add("ExpenditureType", info.ExpenditureType);
 			hash.Add("ContractType", info.ContractType);
 			hash.Add("ContractName", info.ContractName);
 			hash.Add("ContractMoney", info.ContractMoney);
 			hash.Add("CompanySigner", info.CompanySigner);
 			hash.Add("CustomerSigner", info.CustomerSigner);
 			hash.Add("SignDate", info.SignDate);
 			hash.Add("SignLocation", info.SignLocation);
 			hash.Add("PartyBName", info.PartyBName);
 			hash.Add("StartDate", info.StartDate);
 			hash.Add("EndDate", info.EndDate);
 			hash.Add("Settlement", info.Settlement);
 			hash.Add("Status", info.Status);
 			hash.Add("RelatedItems", info.RelatedItems);
 			hash.Add("Contact", info.Contact);
 			hash.Add("ContactPhone", info.ContactPhone);
 			hash.Add("ContactMobile", info.ContactMobile);
 			hash.Add("Content", info.Content);
 			hash.Add("Note", info.Note);
 			hash.Add("AttachGUID", info.AttachGUID);
 			hash.Add("Operator", info.Operator);
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
            dict.Add("Supplier_ID", "供应商名称");
            dict.Add("HandNo", "合同编号");
            dict.Add("ExpenditureType", "收支类型");
            dict.Add("ContractType", "合同类型");
            dict.Add("ContractName", "合同名称");
            dict.Add("ContractMoney", "合同金额");
            dict.Add("CompanySigner", "公司签约人");
            dict.Add("CustomerSigner", "客户签约人");
            dict.Add("SignDate", "签约日期");
            dict.Add("SignLocation", "签约地点");
            dict.Add("PartyBName", "乙方名称");
            dict.Add("StartDate", "合同开始日期");
            dict.Add("EndDate", "合同结束日期");
            dict.Add("Settlement", "结算情况");
            dict.Add("Status", "合同状态");
            dict.Add("RelatedItems", "关联项目");
            dict.Add("Contact", "联系人");
            dict.Add("ContactPhone", "联系人电话");
            dict.Add("ContactMobile", "联系人手机");
            dict.Add("Content", "合同内容");
            dict.Add("Note", "备注说明");
            dict.Add("AttachGUID", "附件组别ID");
            dict.Add("Operator", "经办人");
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
        /// 获取合同签约年度列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetSignYearList()
        {
            List<string> list = new List<string>();
            string sql = string.Format("Select distinct year(SignDate) as SignYear From {0} order by SignYear desc", tableName);
            
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