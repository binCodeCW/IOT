using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using Microsoft.Practices.EnterpriseLibrary.Data;
using IOT.MVCWebMis.Entity;
using IOT.MVCWebMis.IDAL;

namespace IOT.MVCWebMis.DALSQL
{
    /// <summary>
    /// YH04D_DeviceDataRec
    /// </summary>
	public class YH04D_DeviceDataRec : BaseDALSQL<YH04D_DeviceDataRecInfo>, IYH04D_DeviceDataRec
	{
		#region 对象实例及构造函数

		public static YH04D_DeviceDataRec Instance
		{
			get
			{
				return new YH04D_DeviceDataRec();
			}
		}
		public YH04D_DeviceDataRec() : base("YH04D_DeviceDataRec","rowid")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override YH04D_DeviceDataRecInfo DataReaderToEntity(IDataReader dataReader)
		{
			YH04D_DeviceDataRecInfo info = new YH04D_DeviceDataRecInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.Rowid = reader.GetInt32("rowid");
			info.Timestamp = reader.GetString("timestamp");
			info.Stamptime = reader.GetDateTime("stamptime");
			info.Appid = reader.GetString("appid");
			info.Serviceid = reader.GetString("serviceid");
			info.Deviceid = reader.GetString("deviceid");
			info.Bendi = reader.GetString("bendi");
			info.Testtime = reader.GetString("testtime");
			info.UsageCount = reader.GetString("UsageCount");
			info.TestResult = reader.GetString("testResult");
			info.Dpm = reader.GetString("dpm");
			info.C1 = reader.GetString("c1");
			info.C2 = reader.GetString("c2");
			info.SnH = reader.GetString("snH");
			info.SnL = reader.GetString("snL");
			info.NegativeLimit = reader.GetString("negativeLimit");
			info.Recordtime = reader.GetDateTime("recordtime");
			info.DeviceNo = reader.GetString("deviceNo");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH04D_DeviceDataRecInfo obj)
		{
		    YH04D_DeviceDataRecInfo info = obj as YH04D_DeviceDataRecInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("timestamp", info.Timestamp);
 			hash.Add("stamptime", info.Stamptime);
 			hash.Add("appid", info.Appid);
 			hash.Add("serviceid", info.Serviceid);
 			hash.Add("deviceid", info.Deviceid);
 			hash.Add("bendi", info.Bendi);
 			hash.Add("testtime", info.Testtime);
 			hash.Add("UsageCount", info.UsageCount);
 			hash.Add("testResult", info.TestResult);
 			hash.Add("dpm", info.Dpm);
 			hash.Add("c1", info.C1);
 			hash.Add("c2", info.C2);
 			hash.Add("snH", info.SnH);
 			hash.Add("snL", info.SnL);
 			hash.Add("negativeLimit", info.NegativeLimit);
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
             dict.Add("Stamptime", "时间戳转换datetime");
             dict.Add("Appid", "应用ID");
             dict.Add("Serviceid", "服务ID");
             dict.Add("Deviceid", "设备ID");
             dict.Add("Bendi", "本底");
             dict.Add("Testtime", "检测时间");
             dict.Add("UsageCount", "耗材使用数量");
             dict.Add("TestResult", "检测结果");
             dict.Add("Dpm", "DPM值");
             dict.Add("C1", "C值1");
             dict.Add("C2", "C值2");
             dict.Add("SnH", "H值");
             dict.Add("SnL", "L值");
             dict.Add("NegativeLimit", "阴性上限");
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
            return "rowid,timestamp,stamptime,appid,serviceid,deviceid,bendi,testtime,UsageCount,testResult,dpm,c1,c2,snH,snL,negativeLimit,recordtime,deviceNo";
        }
    }
}