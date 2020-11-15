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
    /// 存放地点
    /// </summary>
	public class StoreAddress : BaseDALSQL<StoreAddressInfo>, IStoreAddress
	{
		#region 对象实例及构造函数

		public static StoreAddress Instance
		{
			get
			{
				return new StoreAddress();
			}
		}
		public StoreAddress() : base("T_StoreAddress","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override StoreAddressInfo DataReaderToEntity(IDataReader dataReader)
		{
			StoreAddressInfo info = new StoreAddressInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Dept_ID = reader.GetString("Dept_ID");
			info.KeepAddr = reader.GetString("KeepAddr");
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
        protected override Hashtable GetHashByEntity(StoreAddressInfo obj)
		{
		    StoreAddressInfo info = obj as StoreAddressInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Dept_ID", info.Dept_ID);
 			hash.Add("KeepAddr", info.KeepAddr);
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
            dict.Add("ID", "编号");
             dict.Add("Dept_ID", "部门");
             dict.Add("KeepAddr", "存放地点");
             dict.Add("Note", "备注信息");
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
            return "ID,Dept_ID,KeepAddr,Note,Creator,CreateTime";
        }
    }
}