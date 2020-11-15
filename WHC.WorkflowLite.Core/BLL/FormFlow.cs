using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.DALSQL;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// ��ģ������̻��ڹ���
    /// </summary>
	public class FormFlow : BaseBLL<FormFlowInfo>
    {
        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public FormFlow() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// ��ȡָ����ģ������̻����б�
        /// </summary>
        /// <param name="form_id">ָ����ģ��ID</param>
        public List<FormFlowInfo> GetFormFlow(string form_id)
        {
            IFormFlow dal = baseDal as IFormFlow;
            return dal.GetFormFlow(form_id);
        }
                
        /// <summary>
        /// ��ȡָ����ģ��ĵ�һ�����̻�����Ϣ
        /// </summary>
        /// <param name="form_id">ָ����ģ��ID</param>
        public FormFlowInfo GetFirstFormFlow(string form_id)
        {
            IFormFlow dal = baseDal as IFormFlow;
            return dal.GetFirstFormFlow(form_id);
        }

        /// <summary>
        /// �������̻��ڶ���
        /// </summary>
        /// <param name="flowInfo">���̻�����Ϣ</param>
        /// <returns>�ɹ�����true,ʧ�ܷ���false</returns>
        public bool InsertAppFlow(FormFlowInfo flowInfo)
        {
            if (flowInfo == null)
                return false;

            try
            {
                IFormFlow dal = baseDal as IFormFlow;
                dal.IncreaseOrder(flowInfo.FormId, flowInfo.Orderbyid);

                baseDal.Insert(flowInfo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// �༭���̶���
        /// </summary>
        /// <param name="flowInfo">���̻�����Ϣ</param>
        /// <param name="isUp">�Ƿ�Ϊ��һ����</param>
        /// <returns>�ɹ�true,ʧ��false</returns>
        public bool UpdateAppFlow(FormFlowInfo flowInfo, bool isUp)
        {
            bool result = false;

            if (flowInfo == null)
                return false;

            string strOperator = isUp ? "<" : ">";
            string condition = string.Format(" form_id='{0}' and OrderbyId {1} {2}", 
                flowInfo.FormId, strOperator, flowInfo.Orderbyid);
            string orderBy = " order by OrderbyId desc ";

            FormFlowInfo flowCompare = baseDal.FindSingle(condition, orderBy);
            if (flowCompare != null)
            {
                try
                {
                    //�Ͳο�����������˳��
                    flowCompare.Orderbyid = flowInfo.Orderbyid;
                    baseDal.Update(flowCompare, flowCompare.ID);

                    flowInfo.Orderbyid = flowCompare.Orderbyid;
                    baseDal.Update(flowInfo, flowInfo.ID);

                    result = true;
                }
                catch(Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }
            return result;
        }

        /// <summary>
        /// ���ָ�������Ƿ����ѡ�����̴�����
        /// </summary>
        /// <param name="flowId">���̵�ID</param>
        public bool MaySelectProcUser(string flowId)
        {
            string condition = string.Format("id='{0}' and may_selproc > 0 ", flowId);
            return baseDal.GetRecordCount(condition) > 0;
        }

        /// <summary>
        /// ���µ��������ڵ��˳��
        /// </summary>
        /// <param name="sourceId">Դ��¼</param>
        /// <param name="targetId">Ŀ���¼</param>
        /// <returns></returns>
        public bool UpdateTwoSeq(string sourceId, string targetId)
        {
            bool result = false;
            FormFlowInfo sourceInfo = baseDal.FindByID(sourceId);
            FormFlowInfo targetInfo = baseDal.FindByID(targetId);

            if (sourceInfo != null && targetInfo != null)
            {
                if (sourceInfo.Orderbyid >= targetInfo.Orderbyid)
                {
                    //����
                    sourceInfo.Orderbyid = targetInfo.Orderbyid - 1;
                    baseDal.Update(sourceInfo, sourceInfo.ID);
                }
                else
                {
                    //������
                    sourceInfo.Orderbyid = targetInfo.Orderbyid + 1;
                    baseDal.Update(sourceInfo, sourceInfo.ID);
                }

                result = true;
            }
            return result;
        }

        /// <summary>
        /// �������ϻ������µ�˳��
        /// </summary>
        /// <param name="id">��¼��ID</param>
        /// <param name="moveUp">���ϣ����������ƶ���������Ϊtrue</param>
        /// <returns></returns>
        public bool UpDown(string id, bool moveUp)
        {
            //��������Ĺ���
            bool IsDescending = false;

            bool result = false;
            var info = FindByID(id);
            if (info != null)
            {
                //������ѯ������
                string condition = "";
                if (IsDescending)
                {
                    condition = string.Format("OrderbyId {0} {1}", moveUp ? ">" : "<", info.Orderbyid);
                }
                else
                {
                    condition = string.Format("OrderbyId {0} {1}", moveUp ? "<" : ">", info.Orderbyid);
                }

                var list = baseDal.Find(condition);
                decimal newSeq = 0M;
                switch (list.Count)
                {
                    case 0:
                        newSeq = info.Orderbyid;//���ڶ������ߵײ���˳��Ĭ�ϲ���
                        break;

                    case 1:
                        //�������������һ����¼
                        if (IsDescending)
                        {
                            newSeq = moveUp ? (list[0].Orderbyid + 1M) : (list[0].Orderbyid - 1M);
                        }
                        else
                        {
                            newSeq = !moveUp ? (list[0].Orderbyid + 1M) : (list[0].Orderbyid - 1M);
                        }
                        break;

                    case 2:
                        //�м�����,ȡƽ��ֵ
                        newSeq = (list[0].Orderbyid + list[1].Orderbyid) / 2M;
                        break;

                    default:
                        //�������������
                        if (moveUp)
                        {
                            newSeq = (list[list.Count - 2].Orderbyid + list[list.Count - 1].Orderbyid) / 2M;
                        }
                        else
                        {
                            newSeq = (list[0].Orderbyid + list[1].Orderbyid) / 2M;
                        }
                        break;
                }

                //ͳһ�޸�˳��
                info.Orderbyid = newSeq;
                result = Update(info, info.ID);
            }

            return result;
        }

        /// <summary>
        /// ��ȡָ�����Ͳ������Ա��ѡ�б�
        /// </summary>
        /// <param name="formId">��ID</param>
        /// <param name="step">���裬��0��ʼ����</param>
        /// <returns></returns>
        public string GetFlowUserJson(string formId, int step)
        {
            string result = "";
            string condition = string.Format("FORM_ID='{0}' ", formId);
            var list = baseDal.Find(condition);
            if (list != null && list.Count > 0)
            {
                //�޶���Χ
                if (step < 0)
                {
                    step = 0;
                }
                else if (step > list.Count)
                {
                    step = list.Count - 1;
                }

                FormFlowInfo appFlowInfo = list[step];
                if (appFlowInfo != null)
                {
                    result = appFlowInfo.ProcUser;
                }
            }
            return result;
        }
    }
}
