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
    /// YH_DeviceAlarm
    /// </summary>
	public class YH_DeviceAlarm : BaseDALSQL<YH_DeviceAlarmInfo>, IYH_DeviceAlarm
    {
        #region 对象实例及构造函数

        public static YH_DeviceAlarm Instance
        {
            get
            {
                return new YH_DeviceAlarm();
            }
        }
        public YH_DeviceAlarm() : base("YH_DeviceAlarm", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override YH_DeviceAlarmInfo DataReaderToEntity(IDataReader dataReader)
        {
            YH_DeviceAlarmInfo info = new YH_DeviceAlarmInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetInt32("ID");
            info.Timestamp = reader.GetString("timestamp");
            info.Stamptime = reader.GetDateTime("stamptime");
            info.Appid = reader.GetString("appid");
            info.Serviceid = reader.GetString("serviceid");
            info.Deviceid = reader.GetString("deviceid");
            info.ErrorNum = reader.GetString("ErrorNum");
            info.DeviceStyle = reader.GetString("DeviceStyle");
            info.Recordtime = reader.GetDateTime("recordtime");
            info.Deviceno = reader.GetString("deviceno");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH_DeviceAlarmInfo obj)
        {
            YH_DeviceAlarmInfo info = obj as YH_DeviceAlarmInfo;
            Hashtable hash = new Hashtable();

            hash.Add("timestamp", info.Timestamp);
            hash.Add("stamptime", info.Stamptime);
            hash.Add("appid", info.Appid);
            hash.Add("serviceid", info.Serviceid);
            hash.Add("deviceid", info.Deviceid);
            hash.Add("ErrorNum", info.ErrorNum);
            hash.Add("DeviceStyle", info.DeviceStyle);
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
            dict.Add("ErrorNum", "报警信息");
            dict.Add("DeviceStyle", "仪器型号");
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
            return "ID,timestamp,stamptime,appid,serviceid,deviceid,ErrorNum,DeviceStyle,recordtime,deviceno";
        }
    }
}