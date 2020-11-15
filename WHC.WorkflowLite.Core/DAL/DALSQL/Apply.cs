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
	/// Apply 的摘要说明。
	/// </summary>
    public class Apply : BaseDALSQL<ApplyInfo>, IApply
	{
		#region 对象实例及构造函数

		public static Apply Instance
		{
			get
			{
				return new Apply();
			}
		}
		public Apply() : base("tbapp_apply","id")
		{
            this.SortField = "EDITTIME";
            this.IsDescending = true;
		}

		#endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override ApplyInfo DataReaderToEntity(IDataReader dataReader)
        {
            ApplyInfo info = new ApplyInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.FormId = reader.GetString("FORM_ID");
            info.Category = reader.GetString("CATEGORY");
            info.Title = reader.GetString("TITLE");
            info.Status = (ApplyStatus)reader.GetInt32("status");
            info.ProcType = reader.GetInt32("PROC_TYPE");
            info.ProcUser = reader.GetString("PROC_USER");
            info.ProcTime = reader.GetString("PROC_TIME");
            info.MustSelect = reader.GetInt32("MUST_SELECT") > 0;
            info.DataTable = reader.GetString("DATA_TABLE");
            info.WhoInform = reader.GetString("WHO_INFORM");
            info.WhyCancel = reader.GetString("WHY_CANCEL");
            info.InformFinish = reader.GetInt32("INFORM_FINISH") > 0;
            info.InformRefuse = reader.GetInt32("INFORM_REFUSE") > 0;
            info.InformCancel = reader.GetInt32("INFORM_CANCEL") > 0;
            info.SendMail = reader.GetInt32("SEND_MAIL") > 0;
            info.SendBroad = reader.GetInt32("SEND_BROAD") > 0;
            info.SendMobile = reader.GetInt32("SEND_MOBILE") > 0;
            info.CanBack = reader.GetInt32("CAN_BACK") > 0;
            info.MayCancel = reader.GetInt32("MAY_CANCEL") > 0;
            info.Remark = reader.GetString("REMARK");
            info.Editor = reader.GetString("EDITOR");
            info.Edittime = reader.GetDateTime("EDITTIME");
            info.Dept_ID = reader.GetString("DEPT_ID");
            info.Company_ID = reader.GetString("COMPANY_ID");
            info.Deleted = reader.GetInt32("DELETED");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(ApplyInfo obj)
        {
            ApplyInfo info = obj as ApplyInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("FORM_ID", info.FormId);
            hash.Add("CATEGORY", info.Category);
            hash.Add("TITLE", info.Title);
            hash.Add("STATUS", (int)info.Status);
            hash.Add("PROC_TYPE", info.ProcType);
            hash.Add("PROC_USER", info.ProcUser);
            hash.Add("PROC_TIME", info.ProcTime);
            hash.Add("MUST_SELECT", info.MustSelect ? 1 : 0);
            hash.Add("DATA_TABLE", info.DataTable);
            hash.Add("WHO_INFORM", info.WhoInform);
            hash.Add("WHY_CANCEL", info.WhyCancel);
            hash.Add("INFORM_FINISH", info.InformFinish ? 1 : 0);
            hash.Add("INFORM_REFUSE", info.InformRefuse ? 1 : 0);
            hash.Add("INFORM_CANCEL", info.InformCancel ? 1 : 0);
            hash.Add("SEND_MAIL", info.SendMail ? 1 : 0);
            hash.Add("SEND_BROAD", info.SendBroad ? 1 : 0);
            hash.Add("SEND_MOBILE", info.SendMobile ? 1 : 0);
            hash.Add("CAN_BACK", info.CanBack ? 1 : 0);
            hash.Add("MAY_CANCEL", info.MayCancel ? 1 : 0);
            hash.Add("REMARK", info.Remark);
            hash.Add("EDITOR", info.Editor);
            hash.Add("EDITTIME", info.Edittime);
            hash.Add("DEPT_ID", info.Dept_ID);
            hash.Add("COMPANY_ID", info.Company_ID);
            hash.Add("DELETED", info.Deleted);

            return hash;
        }


        /// <summary>
        /// 获取我的待办数量
        /// </summary>
        /// <param name="applyIdString">当前用户的申请单ID字符串</param>
        /// <param name="formTag">表单分类标识（用来区分申请单类型），无则为null</param>
        /// <returns></returns>
        public int GetMyTodoCount(string applyIdString, string formTag)
        {
            string sql = string.Format(@" SELECT count(*) FROM tbapp_apply t join tbapp_form f on t.form_id=f.id
            WHERE t.id in ({0})  ", applyIdString);
            if (string.IsNullOrEmpty(formTag))
            {
                sql += " and f.form_flag is null";
            }
            else
            {
                sql += string.Format(" and f.form_flag ='{0}' ", formTag);
            }

            string strCount = SqlValueList(sql);
            return ConvertHelper.ToInt32(strCount, 0);
        }

        /// <summary>
        /// 获取我的待办数量
        /// </summary>
        /// <param name="applyIdString">当前用户的申请单ID字符串</param>
        /// <param name="formTag">表单分类标识（用来区分申请单类型），无则为null</param>
        /// <returns></returns>
        public DataTable GetMyTodoList(string applyIdString, string formTag)
        {
            string sql = string.Format(@" SELECT t.*,f.apply_url2 FROM tbapp_apply t join tbapp_form f on t.form_id=f.id 
                                 WHERE t.id in ({0}) ", applyIdString);
            if (string.IsNullOrEmpty(formTag))
            {
                sql += " and f.form_flag is null ";
            }
            else
            {
                sql += string.Format(" and f.form_flag ='{0}' ", formTag);
            }

            DataTable dt = GetTopResult(sql, 5, " order by addtime desc");
            dt.TableName = "tableName";//增加一个表名称，防止WCF方式因为TableName为空出错

            return dt;
        }

        /// <summary>
        /// 获取我的已办数量
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="formTag">表单分类标识（用来区分申请单类型），无则为null</param>
        /// <returns></returns>
        public int GetMyDoneCount(int userId, string formTag)
        {
            string sql = string.Format(@" SELECT count(*) FROM tbapp_apply t join tbapp_form f on t.form_id=f.id
            WHERE t.id in (select distinct apply_id from tbapp_apply_flow where proc_uid={0}) ", userId);

            if (string.IsNullOrEmpty(formTag))
            {
                sql += " and f.form_flag is null";
            }
            else
            {
                sql += string.Format(" and f.form_flag ='{0}' ", formTag);
            }

            string strCount = SqlValueList(sql);
            return ConvertHelper.ToInt32(strCount, 0);
        }

        /// <summary>
        /// 获取我发起的数量
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="formTag">表单分类标识（用来区分申请单类型），无则为null</param>
        /// <returns></returns>
        public int GetMyAddedCount(int userId, string formTag)
        {
            string sql = string.Format(@" SELECT count(*) FROM tbapp_apply t join tbapp_form f on t.form_id=f.id
            WHERE t.editor='{0}' ", userId);

            if (string.IsNullOrEmpty(formTag))
            {
                sql += " and f.form_flag is null";
            }
            else
            {
                sql += string.Format(" and f.form_flag ='{0}' ", formTag);
            }

            string strCount = SqlValueList(sql);
            return ConvertHelper.ToInt32(strCount, 0);
        }

        /// <summary>
        /// 修改申请单状态为结束状态
        /// </summary>
        /// <param name="id">申请单ID</param>
        public void SetStatusFinished(string id)
        {
            //PROC_TYPE(处理类型) (0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,自定义流程)
            string sql = string.Format("update tbapp_apply set status=1, PROC_TYPE=0, PROC_USER='', must_select='0' where id='{0}' ", id);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            db.ExecuteNonQuery(command);
        }

        /// <summary>
        /// 删除业务表单中的相关数据
        /// </summary>
        /// <param name="apply_id">申请单ID</param>
        /// <returns></returns>
        public bool DeleteFormTableData(string apply_id, DbTransaction trans = null)
        {
            string sql = string.Format(@"
            SELECT f.data_table FROM tbapp_apply a INNER JOIN tbapp_form f
            ON a.form_id = f.id WHERE a.id = '{0}' ", apply_id);

            string tableName = SqlValueList(sql, trans);
            if (!string.IsNullOrEmpty(tableName))
            {
                sql = string.Format("Delete from {0} where apply_Id='{1}' ", tableName, apply_id);
                return SqlExecute(sql, trans) > 0;
            }

            return false;
        }

    }
}