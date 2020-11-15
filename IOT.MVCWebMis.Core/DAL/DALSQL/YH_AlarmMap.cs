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
    /// YH_AlarmMap
    /// </summary>
	public class YH_AlarmMap : BaseDALSQL<YH_AlarmMapInfo>, IYH_AlarmMap
    {
        #region 对象实例及构造函数

        public static YH_AlarmMap Instance
        {
            get
            {
                return new YH_AlarmMap();
            }
        }
        public YH_AlarmMap() : base("YH_AlarmMap", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override YH_AlarmMapInfo DataReaderToEntity(IDataReader dataReader)
        {
            YH_AlarmMapInfo info = new YH_AlarmMapInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetInt32("ID");
            info.DeviceTypeNo = reader.GetString("DeviceTypeNo");
            info.DeviceTypeName = reader.GetString("DeviceTypeName");
            info.ErrorNO = reader.GetString("ErrorNO");
            info.ErrorText = reader.GetString("ErrorText");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH_AlarmMapInfo obj)
        {
            YH_AlarmMapInfo info = obj as YH_AlarmMapInfo;
            Hashtable hash = new Hashtable();

            hash.Add("DeviceTypeNo", info.DeviceTypeNo);
            hash.Add("DeviceTypeName", info.DeviceTypeName);
            hash.Add("ErrorNO", info.ErrorNO);
            hash.Add("ErrorText", info.ErrorText);

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
            dict.Add("DeviceTypeNo", "仪器类型编号");
            dict.Add("DeviceTypeName", "仪器类型名称");
            dict.Add("ErrorNO", "故障号");
            dict.Add("ErrorText", "故障说明");
            #endregion

            return dict;
        }

        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,DeviceTypeNo,DeviceTypeName,ErrorNO,ErrorText";
        }
    }
}