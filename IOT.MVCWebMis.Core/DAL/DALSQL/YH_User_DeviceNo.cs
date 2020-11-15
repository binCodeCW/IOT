using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;


using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using Microsoft.Practices.EnterpriseLibrary.Data;
using IOT.MVCWebMis.Entity;
using IOT.MVCWebMis.IDAL;

namespace IOT.MVCWebMis.DALSQL
{
    /// <summary>
    /// 用户和仪器编号配置表
    /// </summary>
	public class YH_User_DeviceNo : BaseDALSQL<YH_User_DeviceNoInfo>, IYH_User_DeviceNo
	{
		#region 对象实例及构造函数

		public static YH_User_DeviceNo Instance
		{
			get
			{
				return new YH_User_DeviceNo();
			}
		}
		public YH_User_DeviceNo() : base("YH_User_DeviceNo","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override YH_User_DeviceNoInfo DataReaderToEntity(IDataReader dataReader)
		{
			YH_User_DeviceNoInfo info = new YH_User_DeviceNoInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetInt32("ID");
			info.Name = reader.GetString("Name");
			info.DeviceNo = reader.GetString("DeviceNo");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH_User_DeviceNoInfo obj)
		{
		    YH_User_DeviceNoInfo info = obj as YH_User_DeviceNoInfo;
			Hashtable hash = new Hashtable(); 
			
			//hash.Add("ID", info.ID);
 			hash.Add("Name", info.Name);
 			hash.Add("DeviceNo", info.DeviceNo);
 				
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
             dict.Add("Name", "用户名/登录名");
             dict.Add("DeviceNo", "仪器编号");
             #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,Name,DeviceNo";
        }
    }
}