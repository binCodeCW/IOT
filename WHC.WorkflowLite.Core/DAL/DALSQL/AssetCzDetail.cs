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
    /// 资产处置明细
    /// </summary>
	public class AssetCzDetail : BaseDALSQL<AssetCzDetailInfo>, IAssetCzDetail
	{
		#region 对象实例及构造函数

		public static AssetCzDetail Instance
		{
			get
			{
				return new AssetCzDetail();
			}
		}
		public AssetCzDetail() : base("T_AssetCzDetail","ID")
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
		protected override AssetCzDetailInfo DataReaderToEntity(IDataReader dataReader)
		{
			AssetCzDetailInfo info = new AssetCzDetailInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.BillNo = reader.GetString("BillNo");
			info.AssetCode = reader.GetString("AssetCode");
			info.AssetName = reader.GetString("AssetName");
			info.CzDept = reader.GetString("CzDept");
			info.UsePerson = reader.GetString("UsePerson");
			info.UseAddr = reader.GetString("UseAddr");
			info.KeepAddr = reader.GetString("KeepAddr");
			info.DisposeType = reader.GetString("DisposeType");
			info.Unit = reader.GetString("Unit");
			info.Price = reader.GetDecimal("Price");
			info.TotalQty = reader.GetInt32("TotalQty");
			info.TotalAmount = reader.GetDecimal("TotalAmount");
			info.Note = reader.GetString("Note");
			info.Creator = reader.GetString("Creator");
			info.CreateTime = reader.GetDateTime("CreateTime");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(AssetCzDetailInfo obj)
		{
		    AssetCzDetailInfo info = obj as AssetCzDetailInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("BillNo", info.BillNo);
 			hash.Add("AssetCode", info.AssetCode);
 			hash.Add("AssetName", info.AssetName);
 			hash.Add("CzDept", info.CzDept);
 			hash.Add("UsePerson", info.UsePerson);
 			hash.Add("UseAddr", info.UseAddr);
 			hash.Add("KeepAddr", info.KeepAddr);
 			hash.Add("DisposeType", info.DisposeType);
 			hash.Add("Unit", info.Unit);
 			hash.Add("Price", info.Price);
 			hash.Add("TotalQty", info.TotalQty);
 			hash.Add("TotalAmount", info.TotalAmount);
 			hash.Add("Note", info.Note);
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
            dict.Add("ID", "序号");
             dict.Add("BillNo", "处置单号");
             dict.Add("AssetCode", "资产编号");
             dict.Add("AssetName", "资产名称");
             dict.Add("CzDept", "处置部门");
             dict.Add("UsePerson", "使用人");
             dict.Add("UseAddr", "使用地点");
             dict.Add("KeepAddr", "存放地点");
             dict.Add("DisposeType", "处置方式");
             dict.Add("Unit", "单位");
             dict.Add("Price", "单价");
             dict.Add("TotalQty", "数量");
             dict.Add("TotalAmount", "金额");
             dict.Add("Note", "备注");
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
            return "ID,BillNo,AssetCode,AssetName,CzDept,UsePerson,UseAddr,KeepAddr,DisposeType,Unit,Price,TotalQty,TotalAmount,Note,Creator,CreateTime";
        }
    }
}