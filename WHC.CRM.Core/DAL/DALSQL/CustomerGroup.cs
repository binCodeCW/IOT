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
    /// 客户组别
    /// </summary>
	public class CustomerGroup : BaseDALSQL<CustomerGroupInfo>, ICustomerGroup
	{
		#region 对象实例及构造函数

		public static CustomerGroup Instance
		{
			get
			{
				return new CustomerGroup();
			}
		}
		public CustomerGroup() : base("T_CRM_CustomerGroup","ID")
        {
            this.SortField = "HandNo";
            this.IsDescending = false;
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override CustomerGroupInfo DataReaderToEntity(IDataReader dataReader)
		{
			CustomerGroupInfo info = new CustomerGroupInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.PID = reader.GetString("PID");
			info.HandNo = reader.GetString("HandNo");
			info.Name = reader.GetString("Name");
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
        protected override Hashtable GetHashByEntity(CustomerGroupInfo obj)
		{
		    CustomerGroupInfo info = obj as CustomerGroupInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("PID", info.PID);
 			hash.Add("HandNo", info.HandNo);
 			hash.Add("Name", info.Name);
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
            dict.Add("PID", "上级ID");
            dict.Add("HandNo", "编号");
            dict.Add("Name", "分组名称");
            dict.Add("Note", "备注");
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
        /// 根据用户，获取树形结构的客户分组列表
        /// </summary>
        public List<CustomerGroupNodeInfo> GetTree(string creator, string condition)
        {
            string where = !string.IsNullOrEmpty(creator) ? string.Format("AND Creator='{0}'", creator) : "";
            if (!string.IsNullOrEmpty(condition))
            {
                where += " AND " + condition;
            }
            List<CustomerGroupNodeInfo> nodeList = new List<CustomerGroupNodeInfo>();
            string sql = string.Format("Select * From {0} Where 1=1 {1} Order By PID, HandNo ", tableName, where);

            DataTable dt = base.SqlTable(sql);
            DataRow[] dataRows = dt.Select(string.Format(" PID = '{0}' ", -1));
            for (int i = 0; i < dataRows.Length; i++)
            {
                string id = dataRows[i]["ID"].ToString();
                CustomerGroupNodeInfo nodeInfo = GetNode(id, dt);
                nodeList.Add(nodeInfo);
            }

            return nodeList;
        }

        private CustomerGroupNodeInfo GetNode(string id, DataTable dt)
        {
            CustomerGroupInfo info = this.FindByID(id);
            CustomerGroupNodeInfo nodeInfo = new CustomerGroupNodeInfo(info);

            DataRow[] dChildRows = dt.Select(string.Format(" PID='{0}'", id));
            for (int i = 0; i < dChildRows.Length; i++)
            {
                string childId = dChildRows[i]["ID"].ToString();
                CustomerGroupNodeInfo childNodeInfo = GetNode(childId, dt);
                nodeInfo.Children.Add(childNodeInfo);
            }
            return nodeInfo;
        }
                       
        /// <summary>
        /// 根据客户ID，获取客户对应的分组集合
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <returns></returns>
        public List<CustomerGroupInfo> GetByCustomer(string customerId)
        {
            string sql = string.Format(@"Select t.* From {0} t inner join T_CRM_CustomerGroup_Customer m 
            ON t.ID = m.CustomerGroup_ID Where m.Customer_ID = '{1}' ", tableName, customerId);

            return base.GetList(sql);
        }
    }
}