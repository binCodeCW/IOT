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
    /// 资产盘点明细
    /// </summary>
	public class AssetCheckDetail : BaseDALSQL<AssetCheckDetailInfo>, IAssetCheckDetail
	{
		#region 对象实例及构造函数

		public static AssetCheckDetail Instance
		{
			get
			{
				return new AssetCheckDetail();
			}
		}
		public AssetCheckDetail() : base("T_AssetCheckDetail","ID")
        {
            this.SortField = "CreateTime";
        }

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override AssetCheckDetailInfo DataReaderToEntity(IDataReader dataReader)
		{
			AssetCheckDetailInfo info = new AssetCheckDetailInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.BillNo = reader.GetString("BillNo");
			info.AssetCode = reader.GetString("AssetCode");
			info.AssetName = reader.GetString("AssetName");
			info.AttachGUID = reader.GetString("AttachGUID");
			info.Qty = reader.GetInt32("Qty");
			info.UsePerson = reader.GetString("UsePerson");
			info.CurrDept = reader.GetString("CurrDept");
			info.KeepAddr = reader.GetString("KeepAddr");
			info.CheckResult = reader.GetInt32("CheckResult");
			info.CheckDisplay = reader.GetString("CheckDisplay");
			info.Note = reader.GetString("Note");
			info.Creator = reader.GetString("Creator");
			info.CreateTime = reader.GetDateTime("CreateTime");
			info.CorpUserId = reader.GetString("CorpUserId");
			info.CheckPerson = reader.GetString("CheckPerson");
			info.CheckDate = reader.GetDateTime("CheckDate");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(AssetCheckDetailInfo obj)
		{
		    AssetCheckDetailInfo info = obj as AssetCheckDetailInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("BillNo", info.BillNo);
 			hash.Add("AssetCode", info.AssetCode);
 			hash.Add("AssetName", info.AssetName);
 			hash.Add("AttachGUID", info.AttachGUID);
 			hash.Add("Qty", info.Qty);
 			hash.Add("UsePerson", info.UsePerson);
 			hash.Add("CurrDept", info.CurrDept);
 			hash.Add("KeepAddr", info.KeepAddr);
 			hash.Add("CheckResult", info.CheckResult);
 			hash.Add("CheckDisplay", info.CheckDisplay);
 			hash.Add("Note", info.Note);
 			hash.Add("Creator", info.Creator);
 			hash.Add("CreateTime", info.CreateTime);
 			hash.Add("CorpUserId", info.CorpUserId);
 			hash.Add("CheckPerson", info.CheckPerson);
 			hash.Add("CheckDate", info.CheckDate);
 				
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
             dict.Add("AssetCode", "资产代码");
             dict.Add("AssetName", "资产名称");
             dict.Add("AttachGUID", "资产图片");
             dict.Add("Qty", "数量");
             dict.Add("UsePerson", "使用人");
             dict.Add("CurrDept", "使用部门");
             dict.Add("KeepAddr", "存放地点");
             dict.Add("CheckResult", "盘点结果");
             dict.Add("CheckDisplay", "盘点结果表达");
             dict.Add("Note", "备注");
             dict.Add("Creator", "创建人");
             dict.Add("CreateTime", "创建时间");
             dict.Add("CorpUserId", "企业号UserID");
             dict.Add("CheckPerson", "盘点人");
             dict.Add("CheckDate", "盘点日期");
             #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,BillNo,AssetCode,AssetName,AttachGUID,Qty,UsePerson,CurrDept,KeepAddr,CheckResult,CheckDisplay,Note,Creator,CreateTime,CorpUserId,CheckPerson,CheckDate";
        }
    }
}