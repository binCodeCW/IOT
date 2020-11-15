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
    /// YH_DeviceControl
    /// </summary>
	public class YH_DeviceControl : BaseDALSQL<YH_DeviceControlInfo>, IYH_DeviceControl
    {
        #region 对象实例及构造函数

        public static YH_DeviceControl Instance
        {
            get
            {
                return new YH_DeviceControl();
            }
        }
        public YH_DeviceControl() : base("YH_DeviceControl", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override YH_DeviceControlInfo DataReaderToEntity(IDataReader dataReader)
        {
            YH_DeviceControlInfo info = new YH_DeviceControlInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.Ip = reader.GetString("ip");
            info.Port = reader.GetInt32("port");
            info.Appid = reader.GetString("appid");
            info.Appsecret = reader.GetString("appsecret");
            info.Serviceid = reader.GetString("serviceid");
            info.Deviceid = reader.GetString("deviceid");
            info.Cmd = reader.GetString("cmd");
            info.Attribute = reader.GetString("attribute");
            info.Ifint = reader.GetBoolean("ifint");
            info.Data = reader.GetString("data");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH_DeviceControlInfo obj)
        {
            YH_DeviceControlInfo info = obj as YH_DeviceControlInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("ip", info.Ip);
            hash.Add("port", info.Port);
            hash.Add("appid", info.Appid);
            hash.Add("appsecret", info.Appsecret);
            hash.Add("serviceid", info.Serviceid);
            hash.Add("deviceid", info.Deviceid);
            hash.Add("cmd", info.Cmd);
            hash.Add("attribute", info.Attribute);
            hash.Add("ifint", info.Ifint);
            hash.Add("data", info.Data);

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
            dict.Add("Ip", "HTTPS接入方式IP");
            dict.Add("Port", "HTTPS接入方式端口");
            dict.Add("Appid", "项目应用ID");
            dict.Add("Appsecret", "项目应用密钥");
            dict.Add("Serviceid", "服务名称");
            dict.Add("Deviceid", "设备ID");
            dict.Add("Cmd", "命令名称");
            dict.Add("Attribute", "命令字段属性名称");
            dict.Add("Ifint", "上报数据是否是数字");
            dict.Add("Data", "上报数据内容");
            #endregion

            return dict;
        }

        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,ip,port,appid,appsecret,serviceid,deviceid,cmd,attribute,ifint,data";
        }
    }
}