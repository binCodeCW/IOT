using System.Collections;
using System.Data;
using System.Collections.Generic;

using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.CRM.Entity;
using WHC.CRM.IDAL;

namespace WHC.CRM.DALSQL
{
    /// <summary>
    /// 竞争对手信息
    /// </summary>
    public class Competitor : BaseDALSQL<CompetitorInfo>, ICompetitor
    {
        #region 对象实例及构造函数

        public static Competitor Instance
        {
            get
            {
                return new Competitor();
            }
        }
        public Competitor()
            : base("T_CRM_Competitor", "ID")
        {
            this.SortField = "EditTime";
            this.IsDescending = true;
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override CompetitorInfo DataReaderToEntity(IDataReader dataReader)
        {
            CompetitorInfo info = new CompetitorInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.HandNo = reader.GetString("HandNo");
            info.Name = reader.GetString("Name");
            info.SimpleName = reader.GetString("SimpleName");
            info.Province = reader.GetString("Province");
            info.City = reader.GetString("City");
            info.District = reader.GetString("District");
            info.Address = reader.GetString("Address");
            info.ZipCode = reader.GetString("ZipCode");
            info.Telephone = reader.GetString("Telephone");
            info.Fax = reader.GetString("Fax");
            info.Contact = reader.GetString("Contact");
            info.ContactPhone = reader.GetString("ContactPhone");
            info.ContactMobile = reader.GetString("ContactMobile");
            info.Email = reader.GetString("Email");
            info.QQ = reader.GetString("QQ");
            info.WebSite = reader.GetString("WebSite");
            info.AttachGUID = reader.GetString("AttachGUID");
            info.Note = reader.GetString("Note");
            info.Creator = reader.GetString("Creator");
            info.CreateTime = reader.GetDateTime("CreateTime");
            info.Editor = reader.GetString("Editor");
            info.EditTime = reader.GetDateTime("EditTime");
            info.Dept_ID = reader.GetString("Dept_ID");
            info.Company_ID = reader.GetString("Company_ID");
            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(CompetitorInfo obj)
        {
            CompetitorInfo info = obj as CompetitorInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("HandNo", info.HandNo);
            hash.Add("Name", info.Name);
            hash.Add("SimpleName", info.SimpleName);
            hash.Add("Province", info.Province);
            hash.Add("City", info.City);
            hash.Add("District", info.District);
            hash.Add("Address", info.Address);
            hash.Add("ZipCode", info.ZipCode);
            hash.Add("Telephone", info.Telephone);
            hash.Add("Fax", info.Fax);
            hash.Add("Contact", info.Contact);
            hash.Add("ContactPhone", info.ContactPhone);
            hash.Add("ContactMobile", info.ContactMobile);
            hash.Add("Email", info.Email);
            hash.Add("QQ", info.QQ);
            hash.Add("WebSite", info.WebSite);
            hash.Add("AttachGUID", info.AttachGUID);
            hash.Add("Note", info.Note);
            hash.Add("Creator", info.Creator);
            hash.Add("CreateTime", info.CreateTime);
            hash.Add("Editor", info.Editor);
            hash.Add("EditTime", info.EditTime);
            hash.Add("Dept_ID", info.Dept_ID);
            hash.Add("Company_ID", info.Company_ID);
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
            dict.Add("ID", "编号");
            dict.Add("HandNo", "编号");
            dict.Add("Name", "对手名称");
            dict.Add("SimpleName", "对手简称");
            dict.Add("Province", "所在省份");
            dict.Add("City", "城市");
            dict.Add("District", "所在行政区");
            dict.Add("Address", "公司地址");
            dict.Add("ZipCode", "公司邮编");
            dict.Add("Telephone", "办公电话");
            dict.Add("Fax", "传真号码");
            dict.Add("Contact", "联系人");
            dict.Add("ContactPhone", "联系人电话");
            dict.Add("ContactMobile", "联系人手机");
            dict.Add("Email", "电子邮件");
            dict.Add("Qq", "QQ号码");
            dict.Add("WebSite", "单位网站");
            dict.Add("AttachGUID", "附件组别ID");
            dict.Add("Note", "备注信息");
            dict.Add("Creator", "创建人");
            dict.Add("CreateTime", "创建时间");
            dict.Add("Editor", "编辑人");
            dict.Add("EditTime", "编辑时间");
            dict.Add("Dept_ID", "所属部门");
            dict.Add("Company_ID", "所属公司");
            #endregion

            return dict;
        }

    }
}