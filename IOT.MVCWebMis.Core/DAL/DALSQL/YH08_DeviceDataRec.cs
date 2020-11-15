﻿using System.Collections;
using System.Data;
using System.Collections.Generic;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.Entity;
using IOT.MVCWebMis.IDAL;

namespace IOT.MVCWebMis.DALSQL
{
    /// <summary>
    /// YH08_DeviceDataRec
    /// </summary>
	public class YH08_DeviceDataRec : BaseDALSQL<YH08_DeviceDataRecInfo>, IYH08_DeviceDataRec
    {
        #region 对象实例及构造函数

        public static YH08_DeviceDataRec Instance
        {
            get
            {
                return new YH08_DeviceDataRec();
            }
        }
        public YH08_DeviceDataRec() : base("YH08_DeviceDataRec", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override YH08_DeviceDataRecInfo DataReaderToEntity(IDataReader dataReader)
        {
            YH08_DeviceDataRecInfo info = new YH08_DeviceDataRecInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetInt32("ID");
            info.Timestamp = reader.GetString("timestamp");
            info.Stamptime = reader.GetDateTime("stamptime");
            info.Appid = reader.GetString("appid");
            info.Serviceid = reader.GetString("serviceid");
            info.Deviceid = reader.GetString("deviceid");
            info.Usagecount = reader.GetString("usagecount");
            info.Dob = reader.GetString("dob");
            info.M_lC12B = reader.GetString("m_lC12B");
            info.M_lC12S = reader.GetString("m_lC12S");
            info.CheckChannel = reader.GetString("checkChannel");
            info.Cbc12 = reader.GetString("cbc12");
            info.Csc12 = reader.GetString("csc12");
            info.Cbc13 = reader.GetString("cbc13");
            info.Csc13 = reader.GetString("csc13");
            info.Recordtime = reader.GetDateTime("recordtime");
            info.Deviceno = reader.GetString("deviceno");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH08_DeviceDataRecInfo obj)
        {
            YH08_DeviceDataRecInfo info = obj as YH08_DeviceDataRecInfo;
            Hashtable hash = new Hashtable();

            hash.Add("timestamp", info.Timestamp);
            hash.Add("stamptime", info.Stamptime);
            hash.Add("appid", info.Appid);
            hash.Add("serviceid", info.Serviceid);
            hash.Add("deviceid", info.Deviceid);
            hash.Add("usagecount", info.Usagecount);
            hash.Add("dob", info.Dob);
            hash.Add("m_lC12B", info.M_lC12B);
            hash.Add("m_lC12S", info.M_lC12S);
            hash.Add("checkChannel", info.CheckChannel);
            hash.Add("cbc12", info.Cbc12);
            hash.Add("csc12", info.Csc12);
            hash.Add("cbc13", info.Cbc13);
            hash.Add("csc13", info.Csc13);
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
            dict.Add("Usagecount", "耗材用量");
            dict.Add("Dob", "DOB值");
            dict.Add("M_lC12B", "初步碳12底气浓度");
            dict.Add("M_lC12S", "初步碳12样气浓度");
            dict.Add("CheckChannel", "检测通道");
            dict.Add("Cbc12", "底气碳12浓度");
            dict.Add("Csc12", "样气碳12浓度");
            dict.Add("Cbc13", "底气碳13浓度");
            dict.Add("Csc13", "样气碳13浓度");
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
            return "ID,timestamp,stamptime,appid,serviceid,deviceid,usagecount,dob,m_lC12B,m_lC12S,checkChannel,cbc12,csc12,cbc13,csc13,recordtime,deviceno";
        }
    }
}