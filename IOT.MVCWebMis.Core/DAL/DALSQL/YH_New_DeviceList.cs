using System.Collections;
using System.Data;
using System.Collections.Generic;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.Entity;
using IOT.MVCWebMis.IDAL;

namespace IOT.MVCWebMis.DALSQL
{
    /// <summary>
    /// YH_New_DeviceList
    /// </summary>
	public class YH_New_DeviceList : BaseDALSQL<YH_New_DeviceListInfo>, IYH_New_DeviceList
	{
		#region 对象实例及构造函数

		public static YH_New_DeviceList Instance
		{
			get
			{
				return new YH_New_DeviceList();
			}
		}
		public YH_New_DeviceList() : base("YH_New_DeviceList","deviceId")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override YH_New_DeviceListInfo DataReaderToEntity(IDataReader dataReader)
		{
			YH_New_DeviceListInfo info = new YH_New_DeviceListInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.DeviceId = reader.GetString("deviceId");
			info.DeviceName = reader.GetString("deviceName");
			info.TenantId = reader.GetString("tenantId");
			info.ProductId = reader.GetString("productId");
			info.Imei = reader.GetString("imei");
			info.Imsi = reader.GetString("imsi");
			info.FirmwareVersion = reader.GetString("firmwareVersion");
			info.DeviceStatus = reader.GetString("deviceStatus");
			info.AutoObserver = reader.GetString("autoObserver");
			info.CreateTime = reader.GetString("createTime");
			info.CreateTimeTrans = reader.GetString("createTimeTrans");
			info.CreateBy = reader.GetString("createBy");
			info.UpdateTime = reader.GetString("updateTime");
			info.UpdateTimeTrans = reader.GetString("updateTimeTrans");
			info.UpdateBy = reader.GetString("updateBy");
			info.NetStatus = reader.GetString("netStatus");
			info.OnlineAt = reader.GetString("onlineAt");
			info.OnlineAtTrans = reader.GetString("onlineAtTrans");
			info.OfflineAt = reader.GetString("offlineAt");
			info.OfflineAtTrans = reader.GetString("offlineAtTrans");
			info.Model = reader.GetString("model");
			info.Serial = reader.GetString("serial");
			info.Locationrecent = reader.GetString("locationrecent");
			info.Locatetime = reader.GetDateTime("locatetime");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH_New_DeviceListInfo obj)
		{
		    YH_New_DeviceListInfo info = obj as YH_New_DeviceListInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("deviceId", info.DeviceId);
 			hash.Add("deviceName", info.DeviceName);
 			hash.Add("tenantId", info.TenantId);
 			hash.Add("productId", info.ProductId);
 			hash.Add("imei", info.Imei);
 			hash.Add("imsi", info.Imsi);
 			hash.Add("firmwareVersion", info.FirmwareVersion);
 			hash.Add("deviceStatus", info.DeviceStatus);
 			hash.Add("autoObserver", info.AutoObserver);
 			hash.Add("createTime", info.CreateTime);
 			hash.Add("createTimeTrans", info.CreateTimeTrans);
 			hash.Add("createBy", info.CreateBy);
 			hash.Add("updateTime", info.UpdateTime);
 			hash.Add("updateTimeTrans", info.UpdateTimeTrans);
 			hash.Add("updateBy", info.UpdateBy);
 			hash.Add("netStatus", info.NetStatus);
 			hash.Add("onlineAt", info.OnlineAt);
 			hash.Add("onlineAtTrans", info.OnlineAtTrans);
 			hash.Add("offlineAt", info.OfflineAt);
 			hash.Add("offlineAtTrans", info.OfflineAtTrans);
 			hash.Add("model", info.Model);
 			hash.Add("serial", info.Serial);
 			hash.Add("locationrecent", info.Locationrecent);
 			hash.Add("locatetime", info.Locatetime);
 				
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
            dict.Add("DeviceId", "设备编码");
             dict.Add("DeviceName", "设备名称，型号+ 设备编号");
             dict.Add("TenantId", "租户ID");
             dict.Add("ProductId", "产品ID");
             dict.Add("Imei", "IMEI号");
             dict.Add("Imsi", "IMSI号");
             dict.Add("FirmwareVersion", "");
             dict.Add("DeviceStatus", "");
             dict.Add("AutoObserver", "");
             dict.Add("CreateTime", "");
             dict.Add("CreateTimeTrans", "");
             dict.Add("CreateBy", "");
             dict.Add("UpdateTime", "");
             dict.Add("UpdateTimeTrans", "");
             dict.Add("UpdateBy", "");
             dict.Add("NetStatus", "");
             dict.Add("OnlineAt", "");
             dict.Add("OnlineAtTrans", "");
             dict.Add("OfflineAt", "");
             dict.Add("OfflineAtTrans", "");
             dict.Add("Model", "");
             dict.Add("Serial", "");
             dict.Add("Locationrecent", "最新位置");
             dict.Add("Locatetime", "最后定位时间");
             #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "deviceId,deviceName,tenantId,productId,imei,imsi,firmwareVersion,deviceStatus,autoObserver,createTime,createTimeTrans,createBy,updateTime,updateTimeTrans,updateBy,netStatus,onlineAt,onlineAtTrans,offlineAt,offlineAtTrans,model,serial,locationrecent,locatetime";
        }
    }
}