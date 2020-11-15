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
    /// 销售机会
    /// </summary>
	public class SaleChance : BaseDALSQL<SaleChanceInfo>, ISaleChance
	{
		#region 对象实例及构造函数

		public static SaleChance Instance
		{
			get
			{
				return new SaleChance();
			}
		}
		public SaleChance() : base("T_CRM_SaleChance","ID")
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
		protected override SaleChanceInfo DataReaderToEntity(IDataReader dataReader)
		{
			SaleChanceInfo info = new SaleChanceInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.HandNo = reader.GetString("HandNo");
			info.Customer_ID = reader.GetString("Customer_ID");
			info.Name = reader.GetString("Name");
			info.ChanceType = reader.GetString("ChanceType");
			info.PreOrderDate = reader.GetDateTime("PreOrderDate");
			info.ActalOrderDate = reader.GetDateTime("ActalOrderDate");
			info.Budget = reader.GetDouble("Budget");
			info.Source = reader.GetString("Source");
			info.Stage = reader.GetDouble("Stage");
			//info.StageStayTime = reader.GetString("StageStayTime");

			info.CompetitiveIndex = reader.GetDouble("CompetitiveIndex");
			info.ConfidenceIndex = reader.GetDouble("ConfidenceIndex");
			info.Status = reader.GetString("Status");
			info.AttachGUID = reader.GetString("AttachGUID");
			info.Note = reader.GetString("Note");
			info.Creator = reader.GetString("Creator");
			info.CreateTime = reader.GetDateTime("CreateTime");
			info.Editor = reader.GetString("Editor");
			info.EditTime = reader.GetDateTime("EditTime");
			info.Dept_ID = reader.GetString("Dept_ID");
			info.Company_ID = reader.GetString("Company_ID");
            info.ShareUsers = reader.GetString("ShareUsers");

            //动态计算停留时间 当前时间-编辑时间
            var stageDays = DateTime.Now.Subtract(info.EditTime).TotalDays + 0.5;
            info.StageStayTime = string.Format("{0}天", (int)stageDays);

            return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(SaleChanceInfo obj)
		{
		    SaleChanceInfo info = obj as SaleChanceInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("HandNo", info.HandNo);
 			hash.Add("Customer_ID", info.Customer_ID);
 			hash.Add("Name", info.Name);
 			hash.Add("ChanceType", info.ChanceType);
 			hash.Add("PreOrderDate", info.PreOrderDate);
 			hash.Add("ActalOrderDate", info.ActalOrderDate);
 			hash.Add("Budget", info.Budget);
 			hash.Add("Source", info.Source);
 			hash.Add("Stage", info.Stage);
 			hash.Add("StageStayTime", info.StageStayTime);
 			hash.Add("CompetitiveIndex", info.CompetitiveIndex);
 			hash.Add("ConfidenceIndex", info.ConfidenceIndex);
 			hash.Add("Status", info.Status);
 			hash.Add("AttachGUID", info.AttachGUID);
 			hash.Add("Note", info.Note);
 			hash.Add("Creator", info.Creator);
 			hash.Add("CreateTime", info.CreateTime);
 			hash.Add("Editor", info.Editor);
 			hash.Add("EditTime", info.EditTime);
 			hash.Add("Dept_ID", info.Dept_ID);
 			hash.Add("Company_ID", info.Company_ID);
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
            dict.Add("Customer_ID", "客户");
            dict.Add("Name", "机会名称");
            dict.Add("ChanceType", "机会类型");
            dict.Add("PreOrderDate", "预计接单日期");
            dict.Add("ActalOrderDate", "实际接单日期");
            dict.Add("Budget", "项目预算");
            dict.Add("Source", "机会来源");
            dict.Add("Stage", "进展阶段");
            dict.Add("StageStayTime", "阶段停留时间");
            dict.Add("CompetitiveIndex", "竞争指数");
            dict.Add("ConfidenceIndex", "信心指数");
            dict.Add("Status", "机会状态");
            dict.Add("AttachGUID", "附件组别ID");
            dict.Add("Note", "备注");
            dict.Add("Creator", "创建人");
            dict.Add("CreateTime", "创建时间");
            dict.Add("Editor", "编辑人");
            dict.Add("EditTime", "编辑时间");
            dict.Add("Dept_ID", "所属部门");
            dict.Add("Company_ID", "所属公司");
            dict.Add("ShareUsers", "业务分享用户");
            #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,HandNo,Customer_ID,Name,ChanceType,PreOrderDate,ActalOrderDate,Budget,Source,Stage,StageStayTime,CompetitiveIndex,ConfidenceIndex,Status,AttachGUID,Note,Creator,CreateTime,Editor,EditTime,Dept_ID,Company_ID";
        }


        /// <summary>
        /// 获取记录日期年度列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetYearList()
        {
            List<string> list = new List<string>();
            string sql = string.Format("Select distinct year(CreateTime) as CreateTime From {0} order by CreateTime desc", tableName);

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