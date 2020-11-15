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
    /// 资产盘点主表
    /// </summary>
	public class AssetCheck : BaseDALSQL<AssetCheckInfo>, IAssetCheck
	{
		#region 对象实例及构造函数

		public static AssetCheck Instance
		{
			get
			{
				return new AssetCheck();
			}
		}
		public AssetCheck() : base("T_AssetCheck","ID")
		{
            this.SortField = "CreateTime";
        }

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override AssetCheckInfo DataReaderToEntity(IDataReader dataReader)
		{
			AssetCheckInfo info = new AssetCheckInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.BillNo = reader.GetString("BillNo");
			info.Company_ID = reader.GetString("Company_ID");
			info.Dept_ID = reader.GetString("Dept_ID");
			info.CheckQty = reader.GetInt32("CheckQty");
			info.DoneQty = reader.GetInt32("DoneQty");
			info.TodoQty = reader.GetInt32("TodoQty");
			info.CheckStatus = reader.GetInt32("CheckStatus");
			info.TaskStatus = reader.GetInt32("TaskStatus");
			info.ApplyDate = reader.GetDateTime("ApplyDate");
			info.Note = reader.GetString("Note");
			info.Creator = reader.GetString("Creator");
			info.CreateTime = reader.GetDateTime("CreateTime");
			info.Editor = reader.GetString("Editor");
			info.EditTime = reader.GetDateTime("EditTime");
			info.CorpUserId = reader.GetString("CorpUserId");
			info.CheckPerson = reader.GetString("CheckPerson");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(AssetCheckInfo obj)
		{
		    AssetCheckInfo info = obj as AssetCheckInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("BillNo", info.BillNo);
 			hash.Add("Company_ID", info.Company_ID);
 			hash.Add("Dept_ID", info.Dept_ID);
 			hash.Add("CheckQty", info.CheckQty);
 			hash.Add("DoneQty", info.DoneQty);
 			hash.Add("TodoQty", info.TodoQty);
 			hash.Add("CheckStatus", info.CheckStatus);
 			hash.Add("TaskStatus", info.TaskStatus);
 			hash.Add("ApplyDate", info.ApplyDate);
 			hash.Add("Note", info.Note);
 			hash.Add("Creator", info.Creator);
 			hash.Add("CreateTime", info.CreateTime);
 			hash.Add("Editor", info.Editor);
 			hash.Add("EditTime", info.EditTime);
 			hash.Add("CorpUserId", info.CorpUserId);
 			hash.Add("CheckPerson", info.CheckPerson);
 				
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
            dict.Add("ID", "序号");
             dict.Add("BillNo", "盘点单号");
             dict.Add("Company_ID", "盘点公司");
             dict.Add("Dept_ID", "盘点部门");
             dict.Add("CheckQty", "盘点数量");
             dict.Add("DoneQty", "已盘数量");
             dict.Add("TodoQty", "未盘数量");
             dict.Add("CheckStatus", "单据状态");
             dict.Add("TaskStatus", "任务状态");
             dict.Add("ApplyDate", "申请日期");
             dict.Add("Note", "备注");
             dict.Add("Creator", "创建人");
             dict.Add("CreateTime", "创建时间");
             dict.Add("Editor", "编辑人");
             dict.Add("EditTime", "编辑时间");
             dict.Add("CorpUserId", "企业号UserID");
             dict.Add("CheckPerson", "指定盘点人");
             #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,BillNo,Company_ID,Dept_ID,CheckQty,DoneQty,TodoQty,CheckStatus,TaskStatus,ApplyDate,Note,Creator,CreateTime,Editor,EditTime,CorpUserId,CheckPerson";
        }
    }
}