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
    /// ���̻��ڹ���
    /// </summary>
	public class FormProc : BaseBLL<FormProcInfo>
    {
        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public FormProc() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// ���ݴ������͵�ID��ȡ��Ӧ������
        /// </summary>
        /// <param name="typeId">�������͵�ID</param>
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
        /// ��ȡ���̹�����sql
        /// </summary>
        /// <returns></returns>
        public  string GetManageProcSql()
        {
            IFormProc dal = baseDal as IFormProc;
            return dal.GetManageProcSql();
        }

        /// <summary>
        /// ��ȡ���ڴ���������
        /// </summary>
        /// <param name="applyId">���뵥id</param>
        /// <returns>string</returns>
        public string GetProcessFlowName(string applyId)
        {
            if(string.IsNullOrEmpty(applyId))
                return string.Empty;

            string retrunVal=string.Empty;
            ApplyFlowInfo flowInfo = BLLFactory<ApplyFlow>.Instance.GetFirstUnHandledFlow(applyId);
            if (flowInfo != null)
            {
                //��ȡ��������
                 return  GetProcType(flowInfo.ProcType);
            }

            return retrunVal;            
        }
               
        /// <summary>
        /// ��ȡĿǰ���õĴ�������
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetAllProcType()
        {
            IFormProc dal = baseDal as IFormProc;
            return dal.GetAllProcType();
        }
    }
}
