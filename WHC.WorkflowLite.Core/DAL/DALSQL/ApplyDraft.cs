using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;

namespace WHC.WorkflowLite.DALSQL
{
    /// <summary>
    /// 申请单草稿（通用存储）
    /// </summary>
	public class ApplyDraft : BaseDALSQL<ApplyDraftInfo>, IApplyDraft
	{
		#region 对象实例及构造函数

		public static ApplyDraft Instance
		{
			get
			{
				return new ApplyDraft();
			}
		}
		public ApplyDraft() : base("TBAPP_ApplyDraft", "ID")
		{
            this.IsDescending = true;
            this.SortField = "CreateTime";
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override ApplyDraftInfo DataReaderToEntity(IDataReader dataReader)
		{
			ApplyDraftInfo info = new ApplyDraftInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Form_ID = reader.GetString("Form_ID");
			info.Category = reader.GetString("Category");
			info.FormName = reader.GetString("FormName");
			info.Title = reader.GetString("Title");
			info.ApplyDraftJson = reader.GetString("ApplyDraftJson");
			info.BizDraftJson = reader.GetString("BizDraftJson");
			info.BizDraftJson2 = reader.GetString("BizDraftJson2");
			info.BizDraftJson3 = reader.GetString("BizDraftJson3");
			info.BizDraftJson4 = reader.GetString("BizDraftJson4");
			info.Creator = reader.GetString("Creator");
			info.CreateTime = reader.GetDateTime("CreateTime");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(ApplyDraftInfo obj)
		{
		    ApplyDraftInfo info = obj as ApplyDraftInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Form_ID", info.Form_ID);
 			hash.Add("Category", info.Category);
 			hash.Add("FormName", info.FormName);
 			hash.Add("Title", info.Title);
 			hash.Add("ApplyDraftJson", info.ApplyDraftJson);
 			hash.Add("BizDraftJson", info.BizDraftJson);
 			hash.Add("BizDraftJson2", info.BizDraftJson2);
 			hash.Add("BizDraftJson3", info.BizDraftJson3);
 			hash.Add("BizDraftJson4", info.BizDraftJson4);
 			hash.Add("Creator", info.Creator);
 			hash.Add("CreateTime", info.CreateTime);
 				
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
             dict.Add("Form_ID", "表单ID");
             dict.Add("Category", "表单分类");
             dict.Add("FormName", "表单名称");
             dict.Add("Title", "草稿标题");
             dict.Add("ApplyDraftJson", "申请单草稿(JSON格式)");
             dict.Add("BizDraftJson", "业务表单草稿(JSON格式)");
             dict.Add("BizDraftJson2", "业务明细表单草稿(JSON格式)");
             dict.Add("BizDraftJson3", "业务明细表单草稿(JSON格式)");
             dict.Add("BizDraftJson4", "业务明细表单草稿(JSON格式)");
             dict.Add("Creator", "创建人");
             dict.Add("CreateTime", "创建时间");
             #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,Form_ID,Category,FormName,Title,ApplyDraftJson,BizDraftJson,BizDraftJson2,BizDraftJson3,BizDraftJson4,Creator,CreateTime";
        }
    }
}