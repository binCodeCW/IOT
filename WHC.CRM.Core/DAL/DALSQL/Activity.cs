using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.CRM.Entity;
using WHC.CRM.IDAL;

namespace WHC.CRM.DALSQL
{
    /// <summary>
    /// 客户活动管理
    /// </summary>
    public class Activity : BaseDALSQL<ActivityInfo>, IActivity
    {
        #region 对象实例及构造函数

        public static Activity Instance
        {
            get
            {
                return new Activity();
            }
        }
        public Activity()
            : base("T_CRM_Activity", "ID")
        {
            this.SortField = "StartTime";
            this.IsDescending = true;
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override ActivityInfo DataReaderToEntity(IDataReader dataReader)
        {
            ActivityInfo info = new ActivityInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.Customer_ID = reader.GetString("Customer_ID");
            info.HandNo = reader.GetString("HandNo");
            info.Category = reader.GetString("Category");
            info.Title = reader.GetString("Title");
            info.Content = reader.GetString("Content");
            info.AttachGUID = reader.GetString("AttachGUID");
            info.StartTime = reader.GetDateTime("StartTime");
            info.EndTime = reader.GetDateTime("EndTime");
            info.Contact = reader.GetString("Contact");
            info.ContactPhone = reader.GetString("ContactPhone");
            info.ContactMobile = reader.GetString("ContactMobile");
            info.Place = reader.GetString("Place");
            info.PlaceAddress = reader.GetString("PlaceAddress");
            info.PlacePhone = reader.GetString("PlacePhone");
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
        protected override Hashtable GetHashByEntity(ActivityInfo obj)
        {
            ActivityInfo info = obj as ActivityInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("Customer_ID", info.Customer_ID);
            hash.Add("HandNo", info.HandNo);
            hash.Add("Category", info.Category);
            hash.Add("Title", info.Title);
            hash.Add("Content", info.Content);
            hash.Add("AttachGUID", info.AttachGUID);
            hash.Add("StartTime", info.StartTime);
            hash.Add("EndTime", info.EndTime);
            hash.Add("Contact", info.Contact);
            hash.Add("ContactPhone", info.ContactPhone);
            hash.Add("ContactMobile", info.ContactMobile);
            hash.Add("Place", info.Place);
            hash.Add("PlaceAddress", info.PlaceAddress);
            hash.Add("PlacePhone", info.PlacePhone);
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
            dict.Add("Customer_ID", "客户名称");
            dict.Add("HandNo", "编号");
            dict.Add("Category", "活动类别");
            dict.Add("Title", "活动标题");
            dict.Add("Content", "活动内容");
            dict.Add("AttachGUID", "附件组别ID");
            dict.Add("StartTime", "开始时间");
            dict.Add("EndTime", "结束时间");
            dict.Add("Contact", "联系人");
            dict.Add("ContactPhone", "联系人电话");
            dict.Add("ContactMobile", "联系人手机");
            dict.Add("Place", "活动场所");
            dict.Add("PlaceAddress", "活动场所地址");
            dict.Add("PlacePhone", "活动场所电话");
            dict.Add("Note", "备注");
            dict.Add("Creator", "创建人");
            dict.Add("CreateTime", "创建时间");
            dict.Add("Editor", "编辑人");
            dict.Add("EditTime", "编辑时间");
            dict.Add("Dept_ID", "所属部门");
            dict.Add("Company_ID", "所属公司");
            #endregion

            return dict;
        }

        /// <summary>
        /// 获取记录日期年度列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetYearList()
        {
            List<string> list = new List<string>();
            string sql = string.Format("Select distinct year(StartTime) as StartTime From {0} order by StartTime desc", tableName);

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            using (IDataReader dr = db.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    string number = dr[0].ToString();
                    if (!string.IsNullOrEmpty(number))
                    {
                        list.Add(number);
                    }
                }
            }
            return list;
        }
    }
}