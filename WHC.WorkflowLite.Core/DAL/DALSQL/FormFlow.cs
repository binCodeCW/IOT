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
	/// AppFlow 的摘要说明。
	/// </summary>
    public class FormFlow : BaseDALSQL<FormFlowInfo>, IFormFlow
	{
		#region 对象实例及构造函数

		public static FormFlow Instance
		{
			get
			{
				return new FormFlow();
			}
		}
		public FormFlow() : base("tbapp_form_flow","id")
		{
            SortField = "OrderbyId";
            IsDescending = false;
		}

		#endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override FormFlowInfo DataReaderToEntity(IDataReader dataReader)
        {
            FormFlowInfo info = new FormFlowInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.FormId = reader.GetString("FORM_ID");
            info.ProcType = reader.GetInt32("PROC_TYPE");
            info.FlowName = reader.GetString("FLOW_NAME");
            info.CondVerify = reader.GetString("COND_VERIFY");
            info.CondUser = reader.GetString("COND_USER");
            info.ProcUser = reader.GetString("PROC_USER");
            info.MaySelproc = reader.GetInt32("MAY_SELPROC");
            info.MayAddFlow = reader.GetInt32("MAY_ADD_FLOW");
            info.MayMsgSend = reader.GetInt32("MAY_MSG_SEND");
            info.CanBack = reader.GetInt32("CAN_BACK");
            info.CanBeBack = reader.GetInt32("CAN_BE_BACK");
            info.CanSms = reader.GetInt32("CAN_SMS");
            info.Remark = reader.GetString("REMARK");
            info.Orderbyid = reader.GetDecimal("ORDERBYID");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(FormFlowInfo obj)
        {
            FormFlowInfo info = obj as FormFlowInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("FORM_ID", info.FormId);
            hash.Add("PROC_TYPE", info.ProcType);
            hash.Add("FLOW_NAME", info.FlowName);
            hash.Add("COND_VERIFY", info.CondVerify);
            hash.Add("COND_USER", info.CondUser);
            hash.Add("PROC_USER", info.ProcUser);
            hash.Add("MAY_SELPROC", info.MaySelproc);
            hash.Add("MAY_ADD_FLOW", info.MayAddFlow);
            hash.Add("MAY_MSG_SEND", info.MayMsgSend);
            hash.Add("CAN_BACK", info.CanBack);
            hash.Add("CAN_BE_BACK", info.CanBeBack);
            hash.Add("CAN_SMS", info.CanSms);
            hash.Add("REMARK", info.Remark);
            hash.Add("ORDERBYID", info.Orderbyid);

            return hash;
        }

        /// <summary>
        /// 获取指定流程模板的流程环节列表
        /// </summary>
        public List<FormFlowInfo> GetFormFlow(string form_id)
        {
            string sql = string.Format("select * from {0} where form_id='{1}' order by OrderbyId asc", tableName, form_id);
            FormFlowInfo entity = null;
            List<FormFlowInfo> list = new List<FormFlowInfo>();

            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

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
        /// 获取指定流程模板的第一个流程环节信息
        /// </summary>
        public FormFlowInfo GetFirstFormFlow(string form_id)
        {
            string sql = string.Format(@"Select top 1 * from {0} where form_id='{1}'  order by orderbyid asc ", tableName, form_id);
            Database db = CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);

            FormFlowInfo entity = null;
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
        /// 为指定模板和顺序的后续流程（包含当前）的顺序+1，用于插入新的流程
        /// </summary>
        /// <param name="form_Id">模板</param>
        /// <param name="orderId">流程顺序</param>
        /// <returns></returns>
        public bool IncreaseOrder(string form_Id, decimal orderId)
        {
            string updateSql = string.Format(@"update tbapp_form_flow set OrderbyId=OrderbyId+1
                                            where form_id='{0}' and OrderbyId>={1}", form_Id, orderId);
            return base.SqlExecute(updateSql) > 0;
        }

    }
}