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
    /// 采购申请单
    /// </summary>
	public class PurchaseApproval : BaseDALSQL<PurchaseApprovalInfo>, IPurchaseApproval
	{
		#region 对象实例及构造函数

		public static PurchaseApproval Instance
		{
			get
			{
				return new PurchaseApproval();
			}
		}
		public PurchaseApproval() : base("TW_PurchaseApproval", "ID")
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
        protected override PurchaseApprovalInfo DataReaderToEntity(IDataReader dataReader)
		{
			PurchaseApprovalInfo info = new PurchaseApprovalInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Reason = reader.GetString("Reason");
			info.DeliveryDate = reader.GetDateTime("DeliveryDate");
			info.ItemName = reader.GetString("ItemName");
			info.Specification = reader.GetString("Specification");
			info.Quantity = reader.GetDecimal("Quantity");
			info.TotalAmount = reader.GetDecimal("TotalAmount");
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
        protected override Hashtable GetHashByEntity(PurchaseApprovalInfo obj)
		{
		    PurchaseApprovalInfo info = obj as PurchaseApprovalInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Reason", info.Reason);
 			hash.Add("DeliveryDate", info.DeliveryDate);
 			hash.Add("ItemName", info.ItemName);
 			hash.Add("Specification", info.Specification);
 			hash.Add("Quantity", info.Quantity);
 			hash.Add("TotalAmount", info.TotalAmount);
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
             dict.Add("Reason", "申请事由");
             dict.Add("DeliveryDate", "期望交付日期");
             dict.Add("ItemName", "物品名称");
             dict.Add("Specification", "型号或规格");
             dict.Add("Quantity", "数量");
             dict.Add("TotalAmount", "费用金额");
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
            return "ID,Reason,DeliveryDate,ItemName,Specification,Quantity,TotalAmount,Apply_ID,ApplyDate,ApplyDept,Note,AttachGUID,Creator,CreateTime,Editor,EditTime";
        }
    }
}