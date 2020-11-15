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
    /// YH01_DeviceDataRec
    /// </summary>
	public class YH01_DeviceDataRec : BaseDALSQL<YH01_DeviceDataRecInfo>, IYH01_DeviceDataRec
	{
		#region 对象实例及构造函数

		public static YH01_DeviceDataRec Instance
		{
			get
			{
				return new YH01_DeviceDataRec();
			}
		}
		public YH01_DeviceDataRec() : base("YH01_DeviceDataRec","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override YH01_DeviceDataRecInfo DataReaderToEntity(IDataReader dataReader)
		{
			YH01_DeviceDataRecInfo info = new YH01_DeviceDataRecInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetInt32("ID");
			info.Timestamp = reader.GetString("timestamp");
			info.Stamptime = reader.GetDateTime("stamptime");
			info.Appid = reader.GetString("appid");
			info.Serviceid = reader.GetString("serviceid");
			info.Deviceid = reader.GetString("deviceid");
			info.Cpm = reader.GetString("cpm");
			info.Bendi = reader.GetString("bendi");
			info.Position = reader.GetString("position");
			info.Efficiency = reader.GetString("efficiency");
			info.Time = reader.GetString("time");
			info.Recordtime = reader.GetDateTime("recordtime");
			info.DeviceNo = reader.GetString("deviceNo");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH01_DeviceDataRecInfo obj)
		{
		    YH01_DeviceDataRecInfo info = obj as YH01_DeviceDataRecInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("timestamp", info.Timestamp);
 			hash.Add("stamptime", info.Stamptime);
 			hash.Add("appid", info.Appid);
 			hash.Add("serviceid", info.Serviceid);
 			hash.Add("deviceid", info.Deviceid);
 			hash.Add("cpm", info.Cpm);
 			hash.Add("bendi", info.Bendi);
 			hash.Add("position", info.Position);
 			hash.Add("efficiency", info.Efficiency);
 			hash.Add("time", info.Time);
 			hash.Add("recordtime", info.Recordtime);
 			hash.Add("deviceNo", info.DeviceNo);
 				
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
             dict.Add("Timestamp", "时间戳");
             dict.Add("Stamptime", "时刻");
             dict.Add("Appid", "应用ID");
             dict.Add("Serviceid", "服务ID");
             dict.Add("Deviceid", "设备ID");
             dict.Add("Cpm", "CPM值");
             dict.Add("Bendi", "本底");
             dict.Add("Position", "通道号");
             dict.Add("Efficiency", "探测效率");
             dict.Add("Time", "检测时长");
             dict.Add("Recordtime", "消息记录时间");
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
            return "ID,timestamp,stamptime,appid,serviceid,deviceid,cpm,bendi,position,efficiency,time,recordtime,deviceNo";
        }
    }
}