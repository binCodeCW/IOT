using System.Collections;
using System.Data;
using System.Collections.Generic;

using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.CRM.Entity;
using WHC.CRM.IDAL;

namespace WHC.CRM.DALSQL
{
    /// <summary>
    /// 销售阶段跟进记录
    /// </summary>
	public class SaleChanceFollow : BaseDALSQL<SaleChanceFollowInfo>, ISaleChanceFollow
	{
		#region 对象实例及构造函数

		public static SaleChanceFollow Instance
		{
			get
			{
				return new SaleChanceFollow();
			}
		}
		public SaleChanceFollow() : base("T_CRM_SaleChanceFollow","ID")
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
		protected override SaleChanceFollowInfo DataReaderToEntity(IDataReader dataReader)
		{
			SaleChanceFollowInfo info = new SaleChanceFollowInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Customer_ID = reader.GetString("Customer_ID");
			info.SaleChance_ID = reader.GetString("SaleChance_ID");
			info.Stage = reader.GetString("Stage");
			info.Title = reader.GetString("Title");
			info.Content = reader.GetString("Content");
			info.Analysis = reader.GetString("Analysis");
			info.ContactTime = reader.GetDateTime("ContactTime");
			info.NextContactTime = reader.GetDateTime("NextContactTime");
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
        protected override Hashtable GetHashByEntity(SaleChanceFollowInfo obj)
		{
		    SaleChanceFollowInfo info = obj as SaleChanceFollowInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Customer_ID", info.Customer_ID);
 			hash.Add("SaleChance_ID", info.SaleChance_ID);
 			hash.Add("Stage", info.Stage);
 			hash.Add("Title", info.Title);
 			hash.Add("Content", info.Content);
 			hash.Add("Analysis", info.Analysis);
 			hash.Add("ContactTime", info.ContactTime);
 			hash.Add("NextContactTime", info.NextContactTime);
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
            //dict.Add("ID", "编号");
            dict.Add("ID", "");
             dict.Add("Customer_ID", "客户ID");
             dict.Add("SaleChance_ID", "销售机会ID");
             dict.Add("Stage", "跟进阶段");
             dict.Add("Title", "跟进主题");
             dict.Add("Content", "跟进内容");
             dict.Add("Analysis", "客户联系人信息项");
             dict.Add("ContactTime", "联系客户时间");
             dict.Add("NextContactTime", "下一次联系客户时间");
             dict.Add("AttachGUID", "附件组别ID");
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
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,Customer_ID,SaleChance_ID,Stage,Title,Content,Analysis,ContactTime,NextContactTime,AttachGUID,Note,Creator,CreateTime,Editor,EditTime,Dept_ID,Company_ID";
        }
    }
}