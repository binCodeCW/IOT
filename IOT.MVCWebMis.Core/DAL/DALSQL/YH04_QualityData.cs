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
    /// YH04_QualityData
    /// </summary>
	public class YH04_QualityData : BaseDALSQL<YH04_QualityDataInfo>, IYH04_QualityData
	{
		#region 对象实例及构造函数

		public static YH04_QualityData Instance
		{
			get
			{
				return new YH04_QualityData();
			}
		}
		public YH04_QualityData() : base("YH04_QualityData","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override YH04_QualityDataInfo DataReaderToEntity(IDataReader dataReader)
		{
			YH04_QualityDataInfo info = new YH04_QualityDataInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetInt64("ID");
			info.Timestamp = reader.GetString("timestamp");
			info.Stamptime = reader.GetDateTime("stamptime");
			info.Appid = reader.GetString("appid");
			info.Serviceid = reader.GetString("serviceid");
			info.Deviceid = reader.GetString("deviceid");
			info.Gm1 = reader.GetString("GM1");
			info.Gm2 = reader.GetString("GM2");
			info.Gm3 = reader.GetString("GM3");
			info.Gm4 = reader.GetString("GM4");
			info.BendiL = reader.GetString("bendiL");
			info.BendiR = reader.GetString("bendiR");
			info.Testtime = reader.GetString("testtime");
			info.DeviceNo = reader.GetString("deviceNo");
			info.Recordtime = reader.GetDateTime("recordtime");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH04_QualityDataInfo obj)
		{
		    YH04_QualityDataInfo info = obj as YH04_QualityDataInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("timestamp", info.Timestamp);
 			hash.Add("stamptime", info.Stamptime);
 			hash.Add("appid", info.Appid);
 			hash.Add("serviceid", info.Serviceid);
 			hash.Add("deviceid", info.Deviceid);
 			hash.Add("GM1", info.Gm1);
 			hash.Add("GM2", info.Gm2);
 			hash.Add("GM3", info.Gm3);
 			hash.Add("GM4", info.Gm4);
 			hash.Add("bendiL", info.BendiL);
 			hash.Add("bendiR", info.BendiR);
 			hash.Add("testtime", info.Testtime);
 			hash.Add("deviceNo", info.DeviceNo);
 			hash.Add("recordtime", info.Recordtime);
 				
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
             dict.Add("Gm1", "左通道计数1");
             dict.Add("Gm2", "左通道计数2");
             dict.Add("Gm3", "右通道计数1");
             dict.Add("Gm4", "右通道计数2");
             dict.Add("BendiL", "左通道本底");
             dict.Add("BendiR", "右通道本底");
             dict.Add("Testtime", "检测时间");
             dict.Add("DeviceNo", "仪器编号");
             dict.Add("Recordtime", "记录时间");
             #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,timestamp,stamptime,appid,serviceid,deviceid,GM1,GM2,GM3,GM4,bendiL,bendiR,testtime,deviceNo,recordtime";
        }
    }
}