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
    /// ApplyFlowlog 的摘要说明。
	/// </summary>
    public class ApplyFlowlog : BaseDALSQL<ApplyFlowlogInfo>, IApplyFlowlog
	{
		#region 对象实例及构造函数

		public static ApplyFlowlog Instance
		{
			get
			{
				return new ApplyFlowlog();
			}
		}
		public ApplyFlowlog() : base("tbapp_apply_flowlog","id")
		{
            SortField = "Proc_time";
            IsDescending = false;
		}

		#endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dataReader">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override ApplyFlowlogInfo DataReaderToEntity(IDataReader dataReader)
        {
            ApplyFlowlogInfo info = new ApplyFlowlogInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.ApplyId = reader.GetString("APPLY_ID");
            info.FlowId = reader.GetString("FLOW_ID");
            info.OrderNo = reader.GetInt32("ORDER_NO");
            info.ProcType = reader.GetInt32("PROC_TYPE");
            info.FlowName = reader.GetString("FLOW_NAME");
            info.ProcUser = reader.GetString("PROC_USER");
            info.ProcTime = reader.GetDateTime("PROC_TIME");
            info.Opinion = reader.GetString("OPINION");
            info.NextFlowName = reader.GetString("NEXT_FLOW_NAME");
            info.NextProcUser = reader.GetString("NEXT_PROC_USER");
            info.Begtime = reader.GetDateTime("BEGTIME");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(ApplyFlowlogInfo obj)
        {
            ApplyFlowlogInfo info = obj as ApplyFlowlogInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("APPLY_ID", info.ApplyId);
            hash.Add("FLOW_ID", info.FlowId);
            hash.Add("ORDER_NO", info.OrderNo);
            hash.Add("PROC_TYPE", info.ProcType);
            hash.Add("FLOW_NAME", info.FlowName);
            hash.Add("PROC_USER", info.ProcUser);
            hash.Add("PROC_TIME", info.ProcTime);
            hash.Add("OPINION", info.Opinion);
            hash.Add("NEXT_FLOW_NAME", info.NextFlowName);
            hash.Add("NEXT_PROC_USER", info.NextProcUser);
            hash.Add("BEGTIME", info.Begtime);

            return hash;
        }

        /// <summary>
        /// 根据申请单ID和流程名称获取对应的记录列表
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="flowName">流程名称</param>
        /// <returns></returns>
        public List<ApplyFlowlogInfo> GetFlowLog(string applyId, string flowName)
        {
            string condition = string.Format("apply_Id ='{0}' and flow_name='{1}'", applyId, flowName);
            return Find(condition);
        }

    }
}