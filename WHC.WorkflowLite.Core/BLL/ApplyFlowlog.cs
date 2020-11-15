using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 流程日志管理
    /// </summary>
    public class ApplyFlowlog : BaseBLL<ApplyFlowlogInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ApplyFlowlog() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
                        
        /// <summary>
        /// 获取对应表单下的所有流程日志
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public List<ApplyFlowlogInfo> GetAllByApplyId(string applyId)
        {
            string condition = string.Format("APPLY_ID='{0}' ", applyId);
            return Find(condition, "order by proc_time");
        }

        /// <summary>
        /// 根据申请单ID和流程名称获取对应的记录列表
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="flowName">流程名称</param>
        /// <returns></returns>
        public List<ApplyFlowlogInfo> GetFlowLog(string applyId, string flowName)
        {
            IApplyFlowlog flowlogDal = baseDal as IApplyFlowlog;
            return flowlogDal.GetFlowLog(applyId, flowName);
        }

        /// <summary>
        /// 根据申请单ID和流程ID删除指定的流程日志
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="flowId">流程ID</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public bool DeleteByFlowId(string applyId, string flowId, DbTransaction trans= null)
        {
            string condition = string.Format("APPLY_ID='{0}' and FLOW_ID='{0}' ", applyId, flowId);
            return DeleteByCondition(condition, trans);
        }
    }
}
