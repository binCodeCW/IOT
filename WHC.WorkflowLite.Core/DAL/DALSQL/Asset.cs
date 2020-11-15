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
    /// 实物资产表
    /// </summary>
	public class Asset : BaseDALSQL<AssetInfo>, IAsset
	{
		#region 对象实例及构造函数

		public static Asset Instance
		{
			get
			{
				return new Asset();
			}
		}
		public Asset() : base("T_Asset","ID")
        {
            this.SortField = "OperateDate";
            this.IsDescending = true;
        }

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override AssetInfo DataReaderToEntity(IDataReader dataReader)
		{
			AssetInfo info = new AssetInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetDecimal("ID");
			info.Code = reader.GetString("Code");
			info.Name = reader.GetString("Name");
			info.ClassName = reader.GetString("ClassName");
			info.CurrDept = reader.GetString("CurrDept");
			info.ChargeDept = reader.GetString("ChargeDept");
			info.AddMethod = reader.GetString("AddMethod");
			info.UseStatus = reader.GetString("UseStatus");
			info.PurchaseDate = reader.GetDateTime("PurchaseDate");
			info.UsePerson = reader.GetString("UsePerson");
			info.Spec = reader.GetString("Spec");
			info.KeepAddr = reader.GetString("KeepAddr");
			info.Qty = reader.GetInt32("Qty");
			info.Remark = reader.GetString("Remark");
			info.Unit = reader.GetString("Unit");
            info.Price = reader.GetDecimal("Price");
            info.OriginValue = reader.GetDecimal("OriginValue");
			info.LimitYears = reader.GetInt32("LimitYears");
			info.DueDate = reader.GetDateTime("DueDate");
			info.UseYears = reader.GetString("UseYears");
			info.IsAge = reader.GetString("IsAge");
			info.Status = reader.GetString("Status");
			info.UseType = reader.GetString("UseType");
			info.Keeper = reader.GetString("Keeper");
			info.InvStatus = reader.GetString("InvStatus");
			info.InvDate = reader.GetDateTime("InvDate");
			info.Inventory = reader.GetString("Inventory");
			info.Account = reader.GetString("Account");
			info.AccDate = reader.GetDateTime("AccDate");
			info.AccHolder = reader.GetString("AccHolder");
			info.UserName = reader.GetString("UserName");
			info.UserDef1 = reader.GetString("UserDef1");
			info.UserDef2 = reader.GetString("UserDef2");
			info.UserDef3 = reader.GetString("UserDef3");
			info.Note = reader.GetString("Note");
			info.Operator = reader.GetString("Operator");
			info.OperateDate = reader.GetDateTime("OperateDate");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(AssetInfo obj)
		{
		    AssetInfo info = obj as AssetInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("Code", info.Code);
 			hash.Add("Name", info.Name);
 			hash.Add("ClassName", info.ClassName);
 			hash.Add("CurrDept", info.CurrDept);
 			hash.Add("ChargeDept", info.ChargeDept);
 			hash.Add("AddMethod", info.AddMethod);
 			hash.Add("UseStatus", info.UseStatus);
 			hash.Add("PurchaseDate", info.PurchaseDate);
 			hash.Add("UsePerson", info.UsePerson);
 			hash.Add("Spec", info.Spec);
 			hash.Add("KeepAddr", info.KeepAddr);
 			hash.Add("Qty", info.Qty);
 			hash.Add("Remark", info.Remark);
 			hash.Add("Unit", info.Unit);
            hash.Add("Price", info.Price);
            hash.Add("OriginValue", info.OriginValue);
 			hash.Add("LimitYears", info.LimitYears);
 			hash.Add("DueDate", info.DueDate);
 			hash.Add("UseYears", info.UseYears);
 			hash.Add("IsAge", info.IsAge);
 			hash.Add("Status", info.Status);
 			hash.Add("UseType", info.UseType);
 			hash.Add("Keeper", info.Keeper);
 			hash.Add("InvStatus", info.InvStatus);
 			hash.Add("InvDate", info.InvDate);
 			hash.Add("Inventory", info.Inventory);
 			hash.Add("Account", info.Account);
 			hash.Add("AccDate", info.AccDate);
 			hash.Add("AccHolder", info.AccHolder);
 			hash.Add("UserName", info.UserName);
 			hash.Add("UserDef1", info.UserDef1);
 			hash.Add("UserDef2", info.UserDef2);
 			hash.Add("UserDef3", info.UserDef3);
 			hash.Add("Note", info.Note);
 			hash.Add("Operator", info.Operator);
 			hash.Add("OperateDate", info.OperateDate);
 				
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
             dict.Add("Code", "资产编码");
             dict.Add("Name", "资产类别");
             dict.Add("ClassName", "类别名称");
             dict.Add("CurrDept", "使用部门");
             dict.Add("ChargeDept", "管理部门");
             dict.Add("AddMethod", "增加方式");
             dict.Add("UseStatus", "使用状况");
             dict.Add("PurchaseDate", "购置日期");
             dict.Add("UsePerson", "使用人");
             dict.Add("Spec", "规格型号");
             dict.Add("KeepAddr", "存放地点");
             dict.Add("Qty", "数量");
             dict.Add("Remark", "备注");
             dict.Add("Unit", "计量单位");
            dict.Add("Price", "单价");
            dict.Add("OriginValue", "本币原值");
             dict.Add("LimitYears", "使用年限");
             dict.Add("DueDate", "使用到期日");
             dict.Add("UseYears", "已使用年数");
             dict.Add("IsAge", "是否超年限");
             dict.Add("Status", "资产动态");
             dict.Add("UseType", "在用类型");
             dict.Add("Keeper", "保管人");
             dict.Add("InvStatus", "盘点状态");
             dict.Add("InvDate", "盘点日期");
             dict.Add("Inventory", "盘点人");
             dict.Add("Account", "财务对账");
             dict.Add("AccDate", "对账日期");
             dict.Add("AccHolder", "核对人");
             dict.Add("UserName", "用户账号");
             dict.Add("UserDef1", "自定义1");
             dict.Add("UserDef2", "自定义2");
             dict.Add("UserDef3", "自定义3");
             dict.Add("Note", "备注");
             dict.Add("Operator", "操作人");
             dict.Add("OperateDate", "操作日期");
             #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,Code,Name,ClassName,CurrDept,ChargeDept,AddMethod,UseStatus,PurchaseDate,UsePerson,Spec,KeepAddr,Qty,Remark,Unit,Price,OriginValue,LimitYears,DueDate,UseYears,IsAge,Status,UseType,Keeper,InvStatus,InvDate,Inventory,Account,AccDate,AccHolder,UserName,UserDef1,UserDef2,UserDef3,Note,Operator,OperateDate";
        }
    }
}