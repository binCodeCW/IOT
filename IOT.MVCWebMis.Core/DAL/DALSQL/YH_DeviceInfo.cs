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
    /// YH_DeviceInfo
    /// </summary>
	public class YH_DeviceInfo : BaseDALSQL<YH_DeviceInfoInfo>, IYH_DeviceInfo
    {
        #region 对象实例及构造函数

        public static YH_DeviceInfo Instance
        {
            get
            {
                return new YH_DeviceInfo();
            }
        }
        public YH_DeviceInfo() : base("YH_DeviceInfo", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override YH_DeviceInfoInfo DataReaderToEntity(IDataReader dataReader)
        {
            YH_DeviceInfoInfo info = new YH_DeviceInfoInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.DeviceId = reader.GetString("DeviceId");
            info.DeviceType = reader.GetString("DeviceType");
            info.SimpleName = reader.GetString("SimpleName");
            info.Province = reader.GetString("Province");
            info.City = reader.GetString("City");
            info.District = reader.GetString("District");
            info.Area = reader.GetString("Area");
            info.Address = reader.GetString("Address");
            info.ZipCode = reader.GetString("ZipCode");
            info.Telephone = reader.GetString("Telephone");
            info.Fax = reader.GetString("Fax");
            info.Contact = reader.GetString("Contact");
            info.ContactPhone = reader.GetString("ContactPhone");
            info.ContactMobile = reader.GetString("ContactMobile");
            info.Email = reader.GetString("Email");
            info.CustomerType = reader.GetString("CustomerType");
            info.Grade = reader.GetString("Grade");
            info.CreditStatus = reader.GetString("CreditStatus");
            info.Importance = reader.GetString("Importance");
            info.Note = reader.GetString("Note");
            info.Creator = reader.GetString("Creator");
            info.CreateTime = reader.GetDateTime("CreateTime");
            info.Editor = reader.GetString("Editor");
            info.EditTime = reader.GetDateTime("EditTime");
            info.IotID = reader.GetString("IotID");
            info.Company_ID = reader.GetString("Company_ID");
            info.Longtitude = reader.GetDouble("Longtitude");
            info.Latitude = reader.GetDouble("Latitude");
            info.OlineStatus = reader.GetInt32("OlineStatus");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(YH_DeviceInfoInfo obj)
        {
            YH_DeviceInfoInfo info = obj as YH_DeviceInfoInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("DeviceId", info.DeviceId);
            hash.Add("DeviceType", info.DeviceType);
            hash.Add("SimpleName", info.SimpleName);
            hash.Add("Province", info.Province);
            hash.Add("City", info.City);
            hash.Add("District", info.District);
            hash.Add("Area", info.Area);
            hash.Add("Address", info.Address);
            hash.Add("ZipCode", info.ZipCode);
            hash.Add("Telephone", info.Telephone);
            hash.Add("Fax", info.Fax);
            hash.Add("Contact", info.Contact);
            hash.Add("ContactPhone", info.ContactPhone);
            hash.Add("ContactMobile", info.ContactMobile);
            hash.Add("Email", info.Email);
            hash.Add("CustomerType", info.CustomerType);
            hash.Add("Grade", info.Grade);
            hash.Add("CreditStatus", info.CreditStatus);
            hash.Add("Importance", info.Importance);
            hash.Add("Note", info.Note);
            hash.Add("Creator", info.Creator);
            hash.Add("CreateTime", info.CreateTime);
            hash.Add("Editor", info.Editor);
            hash.Add("EditTime", info.EditTime);
            hash.Add("IotID", info.IotID);
            hash.Add("Company_ID", info.Company_ID);
            hash.Add("Longtitude", info.Longtitude);
            hash.Add("Latitude", info.Latitude);
            hash.Add("OlineStatus", info.OlineStatus);

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
            dict.Add("DeviceId", "仪器编号");
            dict.Add("DeviceType", "仪器类型");
            dict.Add("SimpleName", "医院简称");
            dict.Add("Province", "所在省份");
            dict.Add("City", "城市");
            dict.Add("District", "所在行政区");
            dict.Add("Area", "市场分区");
            dict.Add("Address", "医院地址");
            dict.Add("ZipCode", "医院邮编");
            dict.Add("Telephone", "医院电话");
            dict.Add("Fax", "传真号码");
            dict.Add("Contact", "主联系人");
            dict.Add("ContactPhone", "联系人电话");
            dict.Add("ContactMobile", "联系人手机");
            dict.Add("Email", "电子邮件");
            dict.Add("CustomerType", "客户类别");
            dict.Add("Grade", "客户级别");
            dict.Add("CreditStatus", "信用等级");
            dict.Add("Importance", "重要级别");
            dict.Add("Note", "备注信息");
            dict.Add("Creator", "创建人");
            dict.Add("CreateTime", "创建时间");
            dict.Add("Editor", "编辑人");
            dict.Add("EditTime", "编辑时间");
            dict.Add("IotID", "是否已删除");
            dict.Add("Company_ID", "所属公司");
            dict.Add("Longtitude", "经度");
            dict.Add("Latitude", "纬度");
            dict.Add("OlineStatus", "仪器在线状态");
            #endregion

            return dict;
        }

        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,DeviceId,DeviceType,SimpleName,Province,City,District,Area,Address,ZipCode,Telephone,Fax,Contact,ContactPhone,ContactMobile,Email,CustomerType,Grade,CreditStatus,Importance,Note,Creator,CreateTime,Editor,EditTime,IotID,Company_ID,Longtitude,Latitude,OlineStatus";
        }
    }
}