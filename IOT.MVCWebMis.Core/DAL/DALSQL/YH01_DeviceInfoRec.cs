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
    /// YH01_DeviceInfoRec
    /// </summary>
	public class YH01_DeviceInfoRec : BaseDALSQL<YH01_DeviceInfoRecInfo>, IYH01_DeviceInfoRec
	{
		#region 对象实例及构造函数

		public static YH01_DeviceInfoRec Instance
		{
			get
			{
				return new YH01_DeviceInfoRec();
			}
		}
		public YH01_DeviceInfoRec() : base("YH01_DeviceInfoRec","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override YH01_DeviceInfoRecInfo DataReaderToEntity(IDataReader dataReader)
		{
			YH01_DeviceInfoRecInfo info = new YH01_DeviceInfoRecInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetInt32("ID");
			info.Timestamp = reader.GetString("timestamp");
			info.Stamptime = reader.GetDateTime("stamptime");
			info.Appid = reader.GetString("appid");
			info.Serviceid = reader.GetString("serviceid");
			info.Deviceid = reader.GetString("deviceid");
			info.Time = reader.GetString("time");
			info.Efficiency = reader.GetString("efficiency");
			info.Bendi = reader.GetString("bendi");
			info.Autoresult = reader.GetString("autoresult");
			info.Enscan = reader.GetString("enscan");
			info.Zpcorrectiondirection = reader.GetString("zpcorrectiondirection");
			info.Zpcorrectionpulse = reader.GetString("zpcorrectionpulse");
			info.Recordtime = reader.GetDateTime("recordtime");
			info.Deviceno = reader.GetString("deviceno");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH01_DeviceInfoRecInfo obj)
		{
		    YH01_DeviceInfoRecInfo info = obj as YH01_DeviceInfoRecInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("timestamp", info.Timestamp);
 			hash.Add("stamptime", info.Stamptime);
 			hash.Add("appid", info.Appid);
 			hash.Add("serviceid", info.Serviceid);
 			hash.Add("deviceid", info.Deviceid);
 			hash.Add("time", info.Time);
 			hash.Add("efficiency", info.Efficiency);
 			hash.Add("bendi", info.Bendi);
 			hash.Add("autoresult", info.Autoresult);
 			hash.Add("enscan", info.Enscan);
 			hash.Add("zpcorrectiondirection", info.Zpcorrectiondirection);
 			hash.Add("zpcorrectionpulse", info.Zpcorrectionpulse);
 			hash.Add("recordtime", info.Recordtime);
 			hash.Add("deviceno", info.Deviceno);
 				
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
             dict.Add("Time", "检测时长");
             dict.Add("Efficiency", "探测效率");
             dict.Add("Bendi", "本底");
             dict.Add("Autoresult", "自动模式");
             dict.Add("Enscan", "检测时是否扫码");
             dict.Add("Zpcorrectiondirection", "零位校准方向");
             dict.Add("Zpcorrectionpulse", "零位校准脉冲");
             dict.Add("Recordtime", "记录时间");
             dict.Add("Deviceno", "仪器编号");
             #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,timestamp,stamptime,appid,serviceid,deviceid,time,efficiency,bendi,autoresult,enscan,zpcorrectiondirection,zpcorrectionpulse,recordtime,deviceno";
        }
    }
}