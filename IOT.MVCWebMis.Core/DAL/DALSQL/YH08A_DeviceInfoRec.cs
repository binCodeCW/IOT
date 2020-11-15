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
    /// YH08A_DeviceInfoRec
    /// </summary>
	public class YH08A_DeviceInfoRec : BaseDALSQL<YH08A_DeviceInfoRecInfo>, IYH08A_DeviceInfoRec
	{
		#region 对象实例及构造函数

		public static YH08A_DeviceInfoRec Instance
		{
			get
			{
				return new YH08A_DeviceInfoRec();
			}
		}
		public YH08A_DeviceInfoRec() : base("YH08A_DeviceInfoRec","rowid")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override YH08A_DeviceInfoRecInfo DataReaderToEntity(IDataReader dataReader)
		{
			YH08A_DeviceInfoRecInfo info = new YH08A_DeviceInfoRecInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.Rowid = reader.GetInt32("rowid");
			info.Timestamp = reader.GetString("timestamp");
			info.Stamptime = reader.GetDateTime("stamptime");
			info.Appid = reader.GetString("appid");
			info.Serviceid = reader.GetString("serviceid");
			info.Deviceid = reader.GetString("deviceid");
			info.Dob0 = reader.GetString("DOB0");
			info.Dob1 = reader.GetString("DOB1");
			info.Dob2 = reader.GetString("DOB2");
			info.Dob3 = reader.GetString("DOB3");
			info.Dob4 = reader.GetString("DOB4");
			info.Dob5 = reader.GetString("DOB5");
			info.Dob6 = reader.GetString("DOB6");
			info.Dob7 = reader.GetString("DOB7");
			info.Dob8 = reader.GetString("DOB8");
			info.Dob9 = reader.GetString("DOB9");
			info.DeviceNo = reader.GetString("deviceNo");
			info.P = reader.GetString("P");
			info.T = reader.GetString("T");
			info.PressType = reader.GetString("PressType");
			info.M_lCfC12_0 = reader.GetString("m_lCfC12_0");
			info.M_lCfC12_1 = reader.GetString("m_lCfC12_1");
			info.M_lCfC12_2 = reader.GetString("m_lCfC12_2");
			info.M_lCfC12_3 = reader.GetString("m_lCfC12_3");
			info.M_lCfC12_4 = reader.GetString("m_lCfC12_4");
			info.M_lCfC12_5 = reader.GetString("m_lCfC12_5");
			info.M_lCfC13_0 = reader.GetString("m_lCfC13_0");
			info.M_lCfC13_1 = reader.GetString("m_lCfC13_1");
			info.M_lCfC13_2 = reader.GetString("m_lCfC13_2");
			info.M_lCfC13_3 = reader.GetString("m_lCfC13_3");
			info.M_lCfC13_4 = reader.GetString("m_lCfC13_4");
			info.M_lCfC13_5 = reader.GetString("m_lCfC13_5");
			info.Aver = reader.GetString("Aver");
			info.Se = reader.GetString("SE");
			info.Recordtime = reader.GetDateTime("recordtime");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH08A_DeviceInfoRecInfo obj)
		{
		    YH08A_DeviceInfoRecInfo info = obj as YH08A_DeviceInfoRecInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("timestamp", info.Timestamp);
 			hash.Add("stamptime", info.Stamptime);
 			hash.Add("appid", info.Appid);
 			hash.Add("serviceid", info.Serviceid);
 			hash.Add("deviceid", info.Deviceid);
 			hash.Add("DOB0", info.Dob0);
 			hash.Add("DOB1", info.Dob1);
 			hash.Add("DOB2", info.Dob2);
 			hash.Add("DOB3", info.Dob3);
 			hash.Add("DOB4", info.Dob4);
 			hash.Add("DOB5", info.Dob5);
 			hash.Add("DOB6", info.Dob6);
 			hash.Add("DOB7", info.Dob7);
 			hash.Add("DOB8", info.Dob8);
 			hash.Add("DOB9", info.Dob9);
 			hash.Add("deviceNo", info.DeviceNo);
 			hash.Add("P", info.P);
 			hash.Add("T", info.T);
 			hash.Add("PressType", info.PressType);
 			hash.Add("m_lCfC12_0", info.M_lCfC12_0);
 			hash.Add("m_lCfC12_1", info.M_lCfC12_1);
 			hash.Add("m_lCfC12_2", info.M_lCfC12_2);
 			hash.Add("m_lCfC12_3", info.M_lCfC12_3);
 			hash.Add("m_lCfC12_4", info.M_lCfC12_4);
 			hash.Add("m_lCfC12_5", info.M_lCfC12_5);
 			hash.Add("m_lCfC13_0", info.M_lCfC13_0);
 			hash.Add("m_lCfC13_1", info.M_lCfC13_1);
 			hash.Add("m_lCfC13_2", info.M_lCfC13_2);
 			hash.Add("m_lCfC13_3", info.M_lCfC13_3);
 			hash.Add("m_lCfC13_4", info.M_lCfC13_4);
 			hash.Add("m_lCfC13_5", info.M_lCfC13_5);
 			hash.Add("Aver", info.Aver);
 			hash.Add("SE", info.Se);
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
             dict.Add("Stamptime", "时间戳转换成datetime");
             dict.Add("Appid", "应用ID");
             dict.Add("Serviceid", "服务ID");
             dict.Add("Deviceid", "设备ID");
             dict.Add("Dob0", "质控第一次DOB");
             dict.Add("Dob1", "第二次DOB");
             dict.Add("Dob2", "第三次DOB");
             dict.Add("Dob3", "第四次DOB");
             dict.Add("Dob4", "第五次DOB");
             dict.Add("Dob5", "第六次DOB");
             dict.Add("Dob6", "第七次DOB");
             dict.Add("Dob7", "第八次DOB");
             dict.Add("Dob8", "第九次DOB");
             dict.Add("Dob9", "第十次DOB");
             dict.Add("DeviceNo", "仪器编号");
             dict.Add("P", "参数P");
             dict.Add("T", "参数T");
             dict.Add("PressType", "压力");
             dict.Add("M_lCfC12_0", "曲线系数（下同）");
             dict.Add("M_lCfC12_1", "");
             dict.Add("M_lCfC12_2", "");
             dict.Add("M_lCfC12_3", "");
             dict.Add("M_lCfC12_4", "");
             dict.Add("M_lCfC12_5", "");
             dict.Add("M_lCfC13_0", "");
             dict.Add("M_lCfC13_1", "");
             dict.Add("M_lCfC13_2", "");
             dict.Add("M_lCfC13_3", "");
             dict.Add("M_lCfC13_4", "");
             dict.Add("M_lCfC13_5", "");
             dict.Add("Aver", "平均值");
             dict.Add("Se", "标准方差");
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
            return "rowid,timestamp,stamptime,appid,serviceid,deviceid,DOB0,DOB1,DOB2,DOB3,DOB4,DOB5,DOB6,DOB7,DOB8,DOB9,deviceNo,P,T,PressType,m_lCfC12_0,m_lCfC12_1,m_lCfC12_2,m_lCfC12_3,m_lCfC12_4,m_lCfC12_5,m_lCfC13_0,m_lCfC13_1,m_lCfC13_2,m_lCfC13_3,m_lCfC13_4,m_lCfC13_5,Aver,SE,recordtime";
        }
    }
}