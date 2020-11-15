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
    /// 印章使用申请单
    /// </summary>
	public class StampUsage : BaseDALSQL<StampUsageInfo>, IStampUsage
	{
		#region 对象实例及构造函数

		public static StampUsage Instance
		{
			get
			{
				return new StampUsage();
			}
		}
		public StampUsage() : base("TW_StampUsage", "ID")
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
        protected override StampUsageInfo DataReaderToEntity(IDataReader dataReader)
		{
			StampUsageInfo info = new StampUsageInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Reason = reader.GetString("Reason");
			info.StampType = reader.GetString("StampType");
			info.FileName = reader.GetString("FileName");
			info.FileCopies = reader.GetString("FileCopies");
			info.StampUseDate = reader.GetDateTime("StampUseDate");
			info.Apply_ID = reader.GetString("Apply_ID");
			info.ApplyDate = reader.GetDateTime("ApplyDate");
			info.ApplyDept = reader.GetString("ApplyDept");
			info.Note = reader.GetString("Note");
			info.AttachGUID = reader.GetString("AttachGUID");
			info.Creator = reader.GetString("Creator");
			info.CreateTime = reader.GetDateTime("CreateTime");
			info.Editor = reader.GetString("Editor");
			info.EditTime = reader.GetDateTime("EditTime");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(StampUsageInfo obj)
		{
		    StampUsageInfo info = obj as StampUsageInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Reason", info.Reason);
 			hash.Add("StampType", info.StampType);
 			hash.Add("FileName", info.FileName);
 			hash.Add("FileCopies", info.FileCopies);
 			hash.Add("StampUseDate", info.StampUseDate);
 			hash.Add("Apply_ID", info.Apply_ID);
 			hash.Add("ApplyDate", info.ApplyDate);
 			hash.Add("ApplyDept", info.ApplyDept);
 			hash.Add("Note", info.Note);
 			hash.Add("AttachGUID", info.AttachGUID);
 			hash.Add("Creator", info.Creator);
 			hash.Add("CreateTime", info.CreateTime);
 			hash.Add("Editor", info.Editor);
 			hash.Add("EditTime", info.EditTime);
 				
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
             dict.Add("Reason", "用章事由");
             dict.Add("StampType", "印章类型");
             dict.Add("FileName", "文件名称");
             dict.Add("FileCopies", "文件份数");
             dict.Add("StampUseDate", "用印日期");
             dict.Add("Apply_ID", "申请单编号");
             dict.Add("ApplyDate", "申请单日期");
             dict.Add("ApplyDept", "申请部门");
             dict.Add("Note", "备注信息");
             dict.Add("AttachGUID", "附件组别ID");
             dict.Add("Creator", "创建人");
             dict.Add("CreateTime", "创建时间");
             dict.Add("Editor", "编辑人");
             dict.Add("EditTime", "编辑时间");
             #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,Reason,StampType,FileName,FileCopies,StampUseDate,Apply_ID,ApplyDate,ApplyDept,Note,AttachGUID,Creator,CreateTime,Editor,EditTime";
        }
    }
}