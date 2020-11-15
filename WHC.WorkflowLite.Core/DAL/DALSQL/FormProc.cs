using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using Microsoft.Practices.EnterpriseLibrary.Data;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.DALSQL
{
	/// <summary>
	/// AppProc 的摘要说明。
	/// </summary>
    public class FormProc : BaseDALSQL<FormProcInfo>, IFormProc
	{
		#region 对象实例及构造函数

		public static FormProc Instance
		{
			get
			{
				return new FormProc();
			}
		}
		public FormProc() : base("tbapp_form_proc","id")
		{
            this.SortField = "EDITTIME";
            this.IsDescending = false;
		}

		#endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override FormProcInfo DataReaderToEntity(IDataReader dataReader)
        {
            FormProcInfo info = new FormProcInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.ProcName = reader.GetString("PROC_NAME");
            info.ProcType = reader.GetInt32("PROC_TYPE");
            info.FormId = reader.GetInt32("FORM_ID");
            info.Forbid = reader.GetInt32("FORBID");
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
        protected override Hashtable GetHashByEntity(FormProcInfo obj)
        {
            FormProcInfo info = obj as FormProcInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("PROC_NAME", info.ProcName);
            hash.Add("PROC_TYPE", info.ProcType);
            hash.Add("FORM_ID", info.FormId);
            hash.Add("FORBID", info.Forbid);
            hash.Add("REMARK", info.Remark);
            hash.Add("EDITOR", info.Editor);
            hash.Add("EDITTIME", info.Edittime);
            hash.Add("DELETED", info.Deleted);

            return hash;
        }

        /// <summary>
        /// 获取目前在用的处理类型
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetAllProcType()
        {
            string sql = string.Format("Select proc_type from {0} ", tableName);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            Dictionary<string, int> list = new Dictionary<string, int>();
            using (IDataReader dr = db.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int procType = Convert.ToInt32(dr[0].ToString());
                    if (!list.ContainsKey(procType.ToString()))
                    {
                        list.Add(procType.ToString(), procType);
                    }
                }
            }
            return list;
        }
             
        /// <summary>
        /// 获取流程管理环节sql
        /// </summary>
        /// <returns></returns>
        public string GetManageProcSql()
        {
            string sql = string.Format(@"SELECT p.* ,f.form_name,
            case p.forbid when 0 then '不禁用' else '禁用' end AS sforbid,
            case p.proc_type when 1 then '审批' when 2 then '归档' when 3 then '会签' when 4 then '阅办' when 5 then '内部邮件' end as proc_type, 
            case sign(p.proc_type) when -1 then '自定义处理' + CONVERT(VARCHAR(10), -p.proc_type) else '自定义处理' end as sproctype
            from tbapp_form_proc p left join tbapp_form f on p.form_id=f.id");

            return sql;
        }
    }
}