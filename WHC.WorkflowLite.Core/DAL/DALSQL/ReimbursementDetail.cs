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
    /// 报销明细
    /// </summary>
	public class ReimbursementDetail : BaseDALSQL<ReimbursementDetailInfo>, IReimbursementDetail
	{
		#region 对象实例及构造函数

		public static ReimbursementDetail Instance
		{
			get
			{
				return new ReimbursementDetail();
			}
		}
		public ReimbursementDetail() : base("TW_ReimbursementDetail","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override ReimbursementDetailInfo DataReaderToEntity(IDataReader dataReader)
		{
			ReimbursementDetailInfo info = new ReimbursementDetailInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Apply_ID = reader.GetString("Apply_ID");
			info.Header_ID = reader.GetString("Header_ID");
			info.FeeType = reader.GetString("FeeType");
			info.OccurTime = reader.GetDateTime("OccurTime");
			info.FeeAmount = reader.GetDecimal("FeeAmount");
			info.FeeDescription = reader.GetString("FeeDescription");
			info.AttachGUID = reader.GetString("AttachGUID");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(ReimbursementDetailInfo obj)
		{
		    ReimbursementDetailInfo info = obj as ReimbursementDetailInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Apply_ID", info.Apply_ID);
 			hash.Add("Header_ID", info.Header_ID);
 			hash.Add("FeeType", info.FeeType);
 			hash.Add("OccurTime", info.OccurTime);
 			hash.Add("FeeAmount", info.FeeAmount);
 			hash.Add("FeeDescription", info.FeeDescription);
 			hash.Add("AttachGUID", info.AttachGUID);
 				
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
             dict.Add("Apply_ID", "申请单编号");
             dict.Add("Header_ID", "主表单头ID");
             dict.Add("FeeType", "费用类型");
             dict.Add("OccurTime", "发生时间");
             dict.Add("FeeAmount", "费用金额");
             dict.Add("FeeDescription", "费用说明");
             dict.Add("AttachGUID", "附件组别ID");
             #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,Apply_ID,Header_ID,FeeType,OccurTime,FeeAmount,FeeDescription,AttachGUID";
        }
    }
}