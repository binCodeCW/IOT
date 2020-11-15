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
    /// 资产转移明细
    /// </summary>
	public class AssetZyDetail : BaseDALSQL<AssetZyDetailInfo>, IAssetZyDetail
	{
		#region 对象实例及构造函数

		public static AssetZyDetail Instance
		{
			get
			{
				return new AssetZyDetail();
			}
		}
		public AssetZyDetail() : base("T_AssetZyDetail","ID")
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
		protected override AssetZyDetailInfo DataReaderToEntity(IDataReader dataReader)
		{
			AssetZyDetailInfo info = new AssetZyDetailInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.BillNo = reader.GetString("BillNo");
			info.AssetCode = reader.GetString("AssetCode");
			info.AssetName = reader.GetString("AssetName");
			info.ZyDept = reader.GetString("ZyDept");
			info.OldUser = reader.GetString("OldUser");
			info.OldAddr = reader.GetString("OldAddr");
			info.CurrUser = reader.GetString("CurrUser");
			info.CurrAddr = reader.GetString("CurrAddr");
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
        protected override Hashtable GetHashByEntity(AssetZyDetailInfo obj)
		{
		    AssetZyDetailInfo info = obj as AssetZyDetailInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("BillNo", info.BillNo);
 			hash.Add("AssetCode", info.AssetCode);
 			hash.Add("AssetName", info.AssetName);
 			hash.Add("ZyDept", info.ZyDept);
 			hash.Add("OldUser", info.OldUser);
 			hash.Add("OldAddr", info.OldAddr);
 			hash.Add("CurrUser", info.CurrUser);
 			hash.Add("CurrAddr", info.CurrAddr);
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
             dict.Add("BillNo", "转移单号");
             dict.Add("AssetCode", "资产编号");
             dict.Add("AssetName", "资产名称");
             dict.Add("ZyDept", "转移部门");
             dict.Add("OldUser", "原使用人");
             dict.Add("OldAddr", "原存放地点");
             dict.Add("CurrUser", "现使用人");
             dict.Add("CurrAddr", "现存放地点");
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
            return "ID,BillNo,AssetCode,AssetName,ZyDept,OldUser,OldAddr,CurrUser,CurrAddr,Unit,Price,TotalQty,TotalAmount,Note,Creator,CreateTime";
        }
    }
}