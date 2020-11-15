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
	/// 申请单流程管理
	/// </summary>
    public class ApplyFlow : BaseDALSQL<ApplyFlowInfo>, IApplyFlow
	{
		#region 对象实例及构造函数

		public static ApplyFlow Instance
		{
			get
			{
				return new ApplyFlow();
			}
		}
		public ApplyFlow() : base("tbapp_apply_flow","id")
		{
            this.SortField = "ORDER_NO";
            this.IsDescending = false;
		}

		#endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override ApplyFlowInfo DataReaderToEntity(IDataReader dataReader)
        {
            ApplyFlowInfo info = new ApplyFlowInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.ApplyId = reader.GetString("APPLY_ID");
            info.OrderNo = reader.GetInt32("ORDER_NO");
            info.ProcType = reader.GetInt32("PROC_TYPE");
            info.FlowName = reader.GetString("FLOW_NAME");
            info.CondVerify = reader.GetString("COND_VERIFY");
            info.ProcUser = reader.GetString("PROC_USER");
            info.MaySelproc = reader.GetInt32("MAY_SELPROC");
            info.MayAddflow = reader.GetInt32("MAY_ADDFLOW");
            info.MayMsgsend = reader.GetInt32("MAY_MSGSEND");
            info.CanBack = reader.GetInt32("CAN_BACK");
            info.CanBeBack = reader.GetInt32("CAN_BE_BACK");
            info.CanSms = reader.GetInt32("CAN_SMS");
            info.IsAdd = reader.GetInt32("IS_ADD");
            info.IsBack = reader.GetInt32("IS_BACK");
            info.IsProc = reader.GetInt32("IS_PROC");
            info.ProcUid = reader.GetInt32("PROC_UID");
            info.ProcTime = reader.GetString("PROC_TIME");
            info.Opinion = reader.GetString("OPINION");
            info.MsgSendTo = reader.GetString("MSG_SEND_TO");
            info.Deltatime = reader.GetInt32("DELTATIME");
            info.SmsProc = reader.GetInt32("SMS_PROC");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(ApplyFlowInfo obj)
        {
            ApplyFlowInfo info = obj as ApplyFlowInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("APPLY_ID", info.ApplyId);
            hash.Add("ORDER_NO", info.OrderNo);
            hash.Add("PROC_TYPE", info.ProcType);
            hash.Add("FLOW_NAME", info.FlowName);
            hash.Add("COND_VERIFY", info.CondVerify);
            hash.Add("PROC_USER", info.ProcUser);
            hash.Add("MAY_SELPROC", info.MaySelproc);
            hash.Add("MAY_ADDFLOW", info.MayAddflow);
            hash.Add("MAY_MSGSEND", info.MayMsgsend);
            hash.Add("CAN_BACK", info.CanBack);
            hash.Add("CAN_BE_BACK", info.CanBeBack);
            hash.Add("CAN_SMS", info.CanSms);
            hash.Add("IS_ADD", info.IsAdd);
            hash.Add("IS_BACK", info.IsBack);
            hash.Add("IS_PROC", info.IsProc);
            hash.Add("PROC_UID", info.ProcUid);
            hash.Add("PROC_TIME", info.ProcTime);
            hash.Add("OPINION", info.Opinion);
            hash.Add("MSG_SEND_TO", info.MsgSendTo);
            hash.Add("DELTATIME", info.Deltatime);
            hash.Add("SMS_PROC", info.SmsProc);

            return hash;
        }

        /// <summary>
        /// 获取对应表单下的所有流程，根据order_no由小到大排序
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public List<ApplyFlowInfo> GetAllByApplyId(string applyId)
        {
            string sql = string.Format("select * from {0} where apply_id='{1}' order by order_no asc", tableName, applyId);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            List<ApplyFlowInfo> list = new List<ApplyFlowInfo>();
            ApplyFlowInfo entity = null;
            using (IDataReader dr = db.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = DataReaderToEntity(dr);
                    list.Add(entity);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取第一个未被处理的申请单流程记录
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public ApplyFlowInfo GetFirstUnHandledFlow(string applyId)
        {
            string sql = string.Format(@"select top 1 * from {0} where apply_id='{1}' and is_proc=0 order by order_no asc", tableName, applyId);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            ApplyFlowInfo entity = null;
            using (IDataReader dr = db.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = DataReaderToEntity(dr);
                }
            }
            return entity;
        }    


        /// <summary>
        /// 获取第一个未被处理的申请单流程记录(下一个流程）
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="flowId">当前流程ID</param>
        /// <returns></returns>
        public ApplyFlowInfo GetNextUnHandledFlow(string applyId, string flowId)
        {
            //-- proc_type类型(0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,<0:自定义流程)
            string sql = string.Format(@"select top 1 * from tbapp_apply_flow where apply_id='{0}' and proc_type <> 5
                                       and id <> '{1}' and is_proc=0 order by order_no asc", applyId, flowId);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            ApplyFlowInfo entity = null;
            using (IDataReader dr = db.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = DataReaderToEntity(dr);
                }
            }
            return entity;
        }
        
        /// <summary>
        /// 获取最后一条完成的流程
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public ApplyFlowInfo GetLastCompletedFlow(string applyId)
        {
            string sql = string.Format(@"select top 1 * from {0} where apply_id='{1}' and is_proc=1 
                                         order by order_no desc ", tableName, applyId);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            ApplyFlowInfo entity = null;
            using (IDataReader dr = db.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = DataReaderToEntity(dr);
                }
            }
            return entity;
        }

        /// <summary>
        /// 设置流程为已处理并增加意见
        /// </summary>
        /// <param name="id">流程ID</param>
        /// <param name="apply_id">申请单ID</param>
        /// <param name="opinion">处理意见</param>
        public bool HandleFlowWithOpinion(string id, string apply_id, string opinion)
        {
            string sql = string.Format(@"update tbapp_apply_flow set proc_uid='0', proc_time='{0}', is_auth='0', is_proc='1', opinion='{3}' 
                                         where apply_id='{1}' and id='{2}' ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), apply_id, id, opinion);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            return db.ExecuteNonQuery(command) > 0;
        }

    }
}