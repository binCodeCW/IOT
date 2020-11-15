using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.DALSQL
{
	/// <summary>
	/// AppForm 的摘要说明。
	/// </summary>
    public class Form : BaseDALSQL<FormInfo>, IForm
	{
		#region 对象实例及构造函数

		public static Form Instance
		{
			get
			{
				return new Form();
			}
		}
		public Form() : base("tbapp_form","id")
		{
		}

		#endregion
        
        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override FormInfo DataReaderToEntity(IDataReader dataReader)
        {
            FormInfo info = new FormInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.FormName = reader.GetString("FORM_NAME");
            info.Category = reader.GetString("CATEGORY");
            info.ApplyUrl = reader.GetString("APPLY_URL");
            info.ApplyUrl2 = reader.GetString("APPLY_URL2");
            info.ApplyWin = reader.GetString("APPLY_WIN");
            info.ApplyWin2 = reader.GetString("APPLY_WIN2");
            info.ApplyWinList = reader.GetString("APPLY_WINLIST");
            info.DataTable = reader.GetString("DATA_TABLE");
            info.WhoCreate = reader.GetString("WHO_CREATE");
            info.WhoBrowse = reader.GetString("WHO_BROWSE");
            info.WhoInform = reader.GetString("WHO_INFORM");
            info.MayCancel = reader.GetInt32("MAY_CANCEL") > 0;
            info.InformFinish = reader.GetInt32("INFORM_FINISH") > 0;
            info.InformRefuse = reader.GetInt32("INFORM_REFUSE") > 0;
            info.InformCancel = reader.GetInt32("INFORM_CANCEL") > 0;
            info.SendMail = reader.GetInt32("SEND_MAIL") > 0;
            info.SendBroad = reader.GetInt32("SEND_BROAD") > 0;
            info.SendMobile = reader.GetInt32("SEND_MOBILE") > 0;
            info.FormFlag = reader.GetString("FORM_FLAG");
            info.Forbid = reader.GetInt32("FORBID") > 0;
            info.Remark = reader.GetString("REMARK");
            info.Editor = reader.GetString("EDITOR");
            info.Edittime = reader.GetDateTime("EDITTIME");
            info.Deleted = reader.GetInt32("DELETED");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(FormInfo obj)
        {
            FormInfo info = obj as FormInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("FORM_NAME", info.FormName);
            hash.Add("CATEGORY", info.Category);
            hash.Add("APPLY_URL", info.ApplyUrl);
            hash.Add("APPLY_URL2", info.ApplyUrl2);
            hash.Add("APPLY_WIN", info.ApplyWin);
            hash.Add("APPLY_WIN2", info.ApplyWin2);
            hash.Add("APPLY_WINLIST", info.ApplyWinList);
            hash.Add("DATA_TABLE", info.DataTable);
            hash.Add("WHO_CREATE", info.WhoCreate);
            hash.Add("WHO_BROWSE", info.WhoBrowse);
            hash.Add("WHO_INFORM", info.WhoInform);
            hash.Add("MAY_CANCEL", info.MayCancel ? 1 : 0);
            hash.Add("INFORM_FINISH", info.InformFinish ? 1 : 0);
            hash.Add("INFORM_REFUSE", info.InformRefuse ? 1 : 0);
            hash.Add("INFORM_CANCEL", info.InformCancel ? 1 : 0);
            hash.Add("SEND_MAIL", info.SendMail ? 1 : 0);
            hash.Add("SEND_BROAD", info.SendBroad ? 1 : 0);
            hash.Add("SEND_MOBILE", info.SendMobile ? 1 : 0);
            hash.Add("FORM_FLAG", info.FormFlag);
            hash.Add("FORBID", info.Forbid ? 1 : 0);
            hash.Add("REMARK", info.Remark);
            hash.Add("EDITOR", info.Editor);
            hash.Add("EDITTIME", info.Edittime);
            hash.Add("DELETED", info.Deleted);

            return hash;
        }

        /// <summary>
        /// 列出指定表的所有字段名称
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public List<string> ListColumns(string tableName)
        {
            if (null == tableName)
            {
                throw new ArgumentNullException("tableName");
            }

            DataTable schemaTable = GetReaderSchema(tableName);
            List<string> nameList = new List<string>();
            foreach (DataRow dr in schemaTable.Rows)
            {
                nameList.Add(dr["ColumnName"].ToString());               
            }
            return nameList;
        }

        private DataTable GetReaderSchema(string tableName)
        {
            DataTable schemaTable = null;

            string sql = string.Format("select * from {0}", tableName);
            Database db = CreateDatabase();
            using (DbConnection connnection = db.CreateConnection())
            {
                connnection.Open();

                DbCommand dbCommand = db.GetSqlStringCommand(sql);
                dbCommand.Connection = connnection;

                using (IDataReader reader = dbCommand.ExecuteReader(CommandBehavior.KeyInfo | CommandBehavior.SchemaOnly))
                {
                    schemaTable = reader.GetSchemaTable();
                }
            }

            return schemaTable;
        } 
                        
        /// <summary>
        /// 获取对应的申请单中的记录数
        /// </summary>
        /// <param name="datatable">表名称</param>
        /// <param name="apply_id">申请单ID</param>
        /// <param name="cond_verify">定义条件</param>
        /// <returns></returns>
        public int GetApplyCount(string datatable, string apply_id, string cond_verify)
        {
            if (!string.IsNullOrEmpty(cond_verify) && !cond_verify.Trim().ToLower().StartsWith("and "))
            {
                cond_verify = " and " + cond_verify;
            }

            string sql = string.Format("SELECT COUNT(*) FROM {0} WHERE apply_id='{1}' {2}", datatable, apply_id, cond_verify);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            int result = Convert.ToInt32(db.ExecuteScalar(command));
            return result;
        }
    }
}