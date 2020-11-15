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
    /// 表单模板的流程环节管理
    /// </summary>
	public class FormFlow : BaseBLL<FormFlowInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FormFlow() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 获取指定表单模板的流程环节列表
        /// </summary>
        /// <param name="form_id">指定表单模板ID</param>
        public List<FormFlowInfo> GetFormFlow(string form_id)
        {
            IFormFlow dal = baseDal as IFormFlow;
            return dal.GetFormFlow(form_id);
        }
                
        /// <summary>
        /// 获取指定表单模板的第一个流程环节信息
        /// </summary>
        /// <param name="form_id">指定表单模板ID</param>
        public FormFlowInfo GetFirstFormFlow(string form_id)
        {
            IFormFlow dal = baseDal as IFormFlow;
            return dal.GetFirstFormFlow(form_id);
        }

        /// <summary>
        /// 插入流程环节定义
        /// </summary>
        /// <param name="flowInfo">流程环节信息</param>
        /// <returns>成功返回true,失败返回false</returns>
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
        /// 编辑流程定义
        /// </summary>
        /// <param name="flowInfo">流程环节信息</param>
        /// <param name="isUp">是否为上一流程</param>
        /// <returns>成功true,失败false</returns>
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
                    //和参考流程做交换顺序
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
        /// 检查指定流程是否可以选择流程处理人
        /// </summary>
        /// <param name="flowId">流程的ID</param>
        public bool MaySelectProcUser(string flowId)
        {
            string condition = string.Format("id='{0}' and may_selproc > 0 ", flowId);
            return baseDal.GetRecordCount(condition) > 0;
        }

        /// <summary>
        /// 更新调整两个节点的顺序
        /// </summary>
        /// <param name="sourceId">源记录</param>
        /// <param name="targetId">目标记录</param>
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
                    //上移
                    sourceInfo.Orderbyid = targetInfo.Orderbyid - 1;
                    baseDal.Update(sourceInfo, sourceInfo.ID);
                }
                else
                {
                    //往下拖
                    sourceInfo.Orderbyid = targetInfo.Orderbyid + 1;
                    baseDal.Update(sourceInfo, sourceInfo.ID);
                }

                result = true;
            }
            return result;
        }

        /// <summary>
        /// 更新向上或者向下的顺序
        /// </summary>
        /// <param name="id">记录的ID</param>
        /// <param name="moveUp">往上，还是往下移动，往上则为true</param>
        /// <returns></returns>
        public bool UpDown(string id, bool moveUp)
        {
            //设置排序的规则
            bool IsDescending = false;

            bool result = false;
            var info = FindByID(id);
            if (info != null)
            {
                //构建查询的条件
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
                        newSeq = info.Orderbyid;//已在顶部或者底部，顺序默认不变
                        break;

                    case 1:
                        //上面或者下面有一个记录
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
                        //中间区域,取平均值
                        newSeq = (list[0].Orderbyid + list[1].Orderbyid) / 2M;
                        break;

                    default:
                        //多于两个的情况
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

                //统一修改顺序
                info.Orderbyid = newSeq;
                result = Update(info, info.ID);
            }

            return result;
        }

        /// <summary>
        /// 获取指定表单和步骤的人员候选列表
        /// </summary>
        /// <param name="formId">表单ID</param>
        /// <param name="step">步骤，从0开始计数</param>
        /// <returns></returns>
        public string GetFlowUserJson(string formId, int step)
        {
            string result = "";
            string condition = string.Format("FORM_ID='{0}' ", formId);
            var list = baseDal.Find(condition);
            if (list != null && list.Count > 0)
            {
                //限定范围
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
