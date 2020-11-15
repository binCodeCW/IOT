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
    /// 客户来电记录
    /// </summary>
    public class ComingCall : BaseDALSQL<ComingCallInfo>, IComingCall
    {
        #region 对象实例及构造函数

        public static ComingCall Instance
        {
            get
            {
                return new ComingCall();
            }
        }
        public ComingCall()
            : base("T_CRM_ComingCall", "ID")
        {
            this.SortField = "CallDate";
            this.IsDescending = true;
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override ComingCallInfo DataReaderToEntity(IDataReader dataReader)
        {
            ComingCallInfo info = new ComingCallInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetInt32("ID");
            info.Customer_ID = reader.GetString("Customer_ID");
            info.Contact = reader.GetString("Contact");
            info.CallNumber = reader.GetString("CallNumber");
            info.CallDate = reader.GetDateTime("CallDate");
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
        protected override Hashtable GetHashByEntity(ComingCallInfo obj)
        {
            ComingCallInfo info = obj as ComingCallInfo;
            Hashtable hash = new Hashtable();

            hash.Add("Customer_ID", info.Customer_ID);
            hash.Add("Contact", info.Contact);
            hash.Add("CallNumber", info.CallNumber);
            hash.Add("CallDate", info.CallDate);
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
            dict.Add("Contact", "联系人");
            dict.Add("CallNumber", "来电号码");
            dict.Add("CallDate", "来电日期");
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

    }
}