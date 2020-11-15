using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Web;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 流程环节管理
    /// </summary>
	public class FormProc : BaseBLL<FormProcInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FormProc() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据处理类型的ID获取对应的名称
        /// </summary>
        /// <param name="typeId">处理类型的ID</param>
        /// <returns></returns>
        public string GetProcType(int typeId)
        {
            string procName = "";

            string condition = string.Format("proc_type = {0}", typeId);
            FormProcInfo procInfo = baseDal.FindSingle(condition);
            if (procInfo != null)
            {
                procName = procInfo.ProcName;
            }

            return procName;
        }

        /// <summary>
        /// 获取流程管理环节sql
        /// </summary>
        /// <returns></returns>
        public  string GetManageProcSql()
        {
            IFormProc dal = baseDal as IFormProc;
            return dal.GetManageProcSql();
        }

        /// <summary>
        /// 获取正在处理环节名称
        /// </summary>
        /// <param name="applyId">申请单id</param>
        /// <returns>string</returns>
        public string GetProcessFlowName(string applyId)
        {
            if(string.IsNullOrEmpty(applyId))
                return string.Empty;

            string retrunVal=string.Empty;
            ApplyFlowInfo flowInfo = BLLFactory<ApplyFlow>.Instance.GetFirstUnHandledFlow(applyId);
            if (flowInfo != null)
            {
                //获取环节名称
                 return  GetProcType(flowInfo.ProcType);
            }

            return retrunVal;            
        }
               
        /// <summary>
        /// 获取目前在用的处理类型
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetAllProcType()
        {
            IFormProc dal = baseDal as IFormProc;
            return dal.GetAllProcType();
        }
    }
}
