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
    /// 客户投诉管理
    /// </summary>
    public class Complaint : BaseDALSQL<ComplaintInfo>, IComplaint
    {
        #region 对象实例及构造函数

        public static Complaint Instance
        {
            get
            {
                return new Complaint();
            }
        }
        public Complaint()
            : base("T_CRM_Complaint", "ID")
        {
            this.SortField = "CreateTime";
            this.IsDescending = true;
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override ComplaintInfo DataReaderToEntity(IDataReader dataReader)
        {
            ComplaintInfo info = new ComplaintInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.Customer_ID = reader.GetString("Customer_ID");
            info.HandNo = reader.GetString("HandNo");
            info.ComplaintDate = reader.GetDateTime("ComplaintDate");
            info.Channel = reader.GetString("Channel");
            info.Contact = reader.GetString("Contact");
            info.ContactPhone = reader.GetString("ContactPhone");
            info.ContactMobile = reader.GetString("ContactMobile");
            info.Category = reader.GetString("Category");
            info.Title = reader.GetString("Title");
            info.Content = reader.GetString("Content");
            info.Note = reader.GetString("Note");
            info.AttachGUID = reader.GetString("AttachGUID");
            info.Operator = reader.GetString("Operator");
            info.DealOpinion = reader.GetString("DealOpinion");
            info.Importance = reader.GetString("Importance");
            info.Urgency = reader.GetString("Urgency");
            info.CustomerAttitude = reader.GetString("CustomerAttitude");
            info.Status = reader.GetString("Status");
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
        protected override Hashtable GetHashByEntity(ComplaintInfo obj)
        {
            ComplaintInfo info = obj as ComplaintInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("Customer_ID", info.Customer_ID);
            hash.Add("HandNo", info.HandNo);
            hash.Add("ComplaintDate", info.ComplaintDate);
            hash.Add("Channel", info.Channel);
            hash.Add("Contact", info.Contact);
            hash.Add("ContactPhone", info.ContactPhone);
            hash.Add("ContactMobile", info.ContactMobile);
            hash.Add("Category", info.Category);
            hash.Add("Title", info.Title);
            hash.Add("Content", info.Content);
            hash.Add("Note", info.Note);
            hash.Add("AttachGUID", info.AttachGUID);
            hash.Add("Operator", info.Operator);
            hash.Add("DealOpinion", info.DealOpinion);
            hash.Add("Importance", info.Importance);
            hash.Add("Urgency", info.Urgency);
            hash.Add("CustomerAttitude", info.CustomerAttitude);
            hash.Add("Status", info.Status);
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
            dict.Add("ComplaintDate", "投诉时间");
            dict.Add("Channel", "投诉途径");
            dict.Add("Contact", "联系人");
            dict.Add("ContactPhone", "联系人电话");
            dict.Add("ContactMobile", "联系人手机");
            dict.Add("Category", "类别名称");
            dict.Add("Title", "标题");
            dict.Add("Content", "内容");
            dict.Add("Note", "备注");
            dict.Add("AttachGUID", "附件组别ID");
            dict.Add("Operator", "受理人");
            dict.Add("DealOpinion", "处理意见");
            dict.Add("Importance", "重要程度");
            dict.Add("Urgency", "紧急程度");
            dict.Add("CustomerAttitude", "客户态度");
            dict.Add("Status", "投诉状态");
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
            string sql = string.Format("Select distinct year(ComplaintDate) as OperateDate From {0} order by OperateDate desc", tableName);

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