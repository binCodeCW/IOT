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
	/// ApplySign 的摘要说明。
	/// </summary>
    public class ApplySign : BaseDALSQL<ApplySignInfo>, IApplySign
	{
		#region 对象实例及构造函数

		public static ApplySign Instance
		{
			get
			{
				return new ApplySign();
			}
		}
		public ApplySign() : base("tbapp_apply_sign","id")
		{
            this.SortField = "PROC_TIME";
            this.IsDescending = true;
		}

		#endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override ApplySignInfo DataReaderToEntity(IDataReader dataReader)
        {
            ApplySignInfo info = new ApplySignInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.ApplyId = reader.GetString("APPLY_ID");
            info.FlowId = reader.GetString("FLOW_ID");
            info.UserId = reader.GetInt32("USER_ID");
            info.IsProc = reader.GetInt32("IS_PROC");
            info.ProcTime = reader.GetDateTime("PROC_TIME");
            info.Opinion = reader.GetString("OPINION");
            info.MsgSendTo = reader.GetString("MSG_SEND_TO");
            info.Deltatime = reader.GetInt32("DELTATIME");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(ApplySignInfo obj)
        {
            ApplySignInfo info = obj as ApplySignInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("APPLY_ID", info.ApplyId);
            hash.Add("FLOW_ID", info.FlowId);
            hash.Add("USER_ID", info.UserId);
            hash.Add("IS_PROC", info.IsProc);
            hash.Add("PROC_TIME", info.ProcTime);
            hash.Add("OPINION", info.Opinion);
            hash.Add("MSG_SEND_TO", info.MsgSendTo);
            hash.Add("DELTATIME", info.Deltatime);

            return hash;
        }

        /// <summary>
        /// 获取指定流程的会签步骤数量
        /// </summary>
        /// <param name="flowId">流程ID</param>
        /// <returns></returns>
        public int GetCountByFlowId(string flowId)
        {
            string sql = string.Format("select count(*) from {0} where flow_id='{1}'", tableName, flowId);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            return Convert.ToInt32(db.ExecuteScalar(command).ToString());
        }
    }
}