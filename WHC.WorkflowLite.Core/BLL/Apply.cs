using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 申请单管理类
    /// </summary>
	public class Apply : BaseBLL<ApplyInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Apply() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 设置某个申请单结束的状态
        /// </summary>
        /// <param name="id">申请单ID</param>
        /// <param name="userId">用户ID</param>
        public void SetStatusFinished(string id, int userId)
        {
            IApply dal = baseDal as IApply;
            dal.SetStatusFinished(id);

            var logInfo = new ApplyLogInfo(id, userId, "申请单已经处理完毕。");
            BLLFactory<ApplyLog>.Instance.Insert(logInfo);
        }

        /// <summary>
        /// 判断申请单的状态
        /// </summary>
        /// <param name="id">申请单ID</param>
        /// <returns>状态(0:处理中,1:已完成,2:已退回,3:已撤消)(其它值为非法值)</returns>
        public int GetStatus(string id)
        {
            var status = baseDal.GetFieldValue(id, "status").ToInt32();
            return status;
        }

        /// <summary>
        /// 设置申请单的状态
        /// </summary>
        /// <param name="id">申请单ID</param>
        /// <param name="status">修改的状态(0:处理中,1:已完成,2:已退回,3:已撤消)(其它值为非法值)</param>
        /// <returns></returns>
        public bool SetStatus(string id, int status)
        {
            Hashtable ht = new Hashtable();
            ht.Add("status", status);

            return baseDal.UpdateFields(ht, id);
        }

        /// <summary>
        /// 删除该申请单所有相关的信息
        /// </summary>
        /// <param name="apply_id">申请单ID</param>
        /// <param name="trans">事务对象</param>
        public void DeleteApplyRelated(string apply_id, DbTransaction trans = null)
        {
            IApply dal = baseDal as IApply;
            dal.DeleteFormTableData(apply_id, trans);//必须先关联删除这个，然后删除表单数据
            dal.Delete(apply_id, trans);
            
            BLLFactory<ApplyFlow>.Instance.DeleteAllFlow(apply_id, trans);
            BLLFactory<ApplyUser>.Instance.DeleteByApplyId(apply_id, trans);

            string condition = string.Format("APPLY_ID='{0}' ", apply_id);
            BLLFactory<ApplyLog>.Instance.DeleteByCondition(condition, trans);
            BLLFactory<ApplyFlowlog>.Instance.DeleteByCondition(condition, trans);
            BLLFactory<ApplyRead>.Instance.DeleteByCondition(condition, trans);
        }

        /// <summary>
        /// 根据申请单ID和当前用户，判断申请单是否可以进行撤销操作
        /// </summary>
        /// <param name="id">申请单ID</param>
        /// <param name="userId">当前用户ID</param>
        /// <returns></returns>
        public bool IsApplyMayCancel(string id, int userId)
        {
            bool result = false;
            ApplyInfo appInfo = BLLFactory<Apply>.Instance.FindByID(id);
            if (appInfo != null)
            {
                //如果流程是可以撤销，且表单状态为处理中，那么可以“撤销”操作可用
                if (appInfo.Status == ApplyStatus.处理中)
                {
                    FormInfo formInfo = BLLFactory<Form>.Instance.FindByID(appInfo.FormId);
                    if (formInfo != null && formInfo.MayCancel && (appInfo.Editor == userId.ToString()))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据申请单ID和当前用户，判断申请单是否可退回重新编辑
        /// </summary>
        /// <param name="id">申请单ID</param>
        /// <param name="userId">当前用户ID</param>
        /// <returns></returns>
        public bool IsApplyMayBackEdit(string id, int userId)
        {
            bool result = false;
            ApplyInfo appInfo = BLLFactory<Apply>.Instance.FindByID(id);
            if (appInfo != null)
            {
                //可退回重新编辑
                if (appInfo.Status == ApplyStatus.已撤消 || appInfo.Status == ApplyStatus.已退回)
                {
                    if (appInfo.Editor == userId.ToString())
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取我的待办数量
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="formTag">表单分类标识（用来区分申请单类型），无则为null</param>
        /// <returns></returns>
        public int GetMyTodoCount(int userId, string formTag)
        {
            string applyIdString = BLLFactory<ApplyUser>.Instance.GetApplyIdByUser(userId);

            IApply dal = baseDal as IApply;
            return dal.GetMyTodoCount(applyIdString, formTag);
        }

        /// <summary>
        /// 获取我的待办数量
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="formTag">表单分类标识（用来区分申请单类型），无则为null</param>
        /// <returns></returns>
        public DataTable GetMyTodoList(int userId, string formTag)
        {
            string applyIdString = BLLFactory<ApplyUser>.Instance.GetApplyIdByUser(userId);

            IApply dal = baseDal as IApply;
            return dal.GetMyTodoList(applyIdString, formTag);
        }

        /// <summary>
        /// 获取我的已办数量
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="formTag">表单分类标识（用来区分申请单类型），无则为null</param>
        /// <returns></returns>
        public int GetMyDoneCount(int userId, string formTag)
        {           
            IApply dal = baseDal as IApply;
            return dal.GetMyDoneCount(userId, formTag);
        }

        /// <summary>
        /// 获取我发起的数量
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="formTag">表单分类标识（用来区分申请单类型），无则为null</param>
        /// <returns></returns>
        public int GetMyAddedCount(int userId, string formTag)
        {
            IApply dal = baseDal as IApply;
            return dal.GetMyAddedCount(userId, formTag);
        }


        /// <summary>
        /// 提示几天到期
        /// </summary>
        /// <param name="StartTime">开始时间</param>
        /// <param name="DueDay">办理的工作日期限</param>
        /// <returns></returns>
        public string FormatBecomeDue(DateTime StartTime, int DueDay)
        {
            int RemainingDays = GetWorkDaySpan(StartTime, DueDay);
            return GetBecomeDueString(RemainingDays);
        }

        /// <summary>
        /// 提示几天到期
        /// </summary>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public string FormatBecomeDue(DateTime StartTime, DateTime EndTime)
        {
            int RemainingDays = GetWorkDaySpan(StartTime, EndTime);
            return GetBecomeDueString(RemainingDays);
        }

        private string GetBecomeDueString(int RemainingDays)
        {
            string result = string.Empty;
            if (RemainingDays > 0)
            {
                result = string.Format("[{0}个工作日后到期]", RemainingDays);
            }
            else if (RemainingDays == 0)
            {
                result = "[今天到期]";
            }
            else
            {
                result = string.Format("[已过期{0}个工作日]", RemainingDays * -1);
            }
            return result;
        }

        /// <summary>
        /// 计算当前时间到规定时间期限截止剩余的工作日数。
        /// </summary>
        /// <param name="StartTime">开始时间</param>
        /// <param name="DueDay">办理的工作日期限</param>
        /// <returns></returns>
        private int GetWorkDaySpan(DateTime StartTime, int DueDay)
        {
            int result;
            int NaturalDaysToNow = (DateTime.Now - StartTime).Days;
            int PassWeek = NaturalDaysToNow / 7;
            int LastDays = NaturalDaysToNow - (PassWeek * 7);//NaturalDaysToNow % 7;
            result = PassWeek * 5;
            for (int i = 1; i <= LastDays; i++)
            {
                DateTime tempTime = StartTime.AddDays(i);
                if (tempTime.DayOfWeek != DayOfWeek.Saturday && tempTime.DayOfWeek != DayOfWeek.Sunday)
                    result++;
            }
            return DueDay - result;
        }

        /// <summary>
        /// 计算当前时间到规定时间期限截止剩余的工作日数。
        /// </summary>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">办理的工作日期限</param>
        /// <returns></returns>
        private int GetWorkDaySpan(DateTime StartTime, DateTime EndTime)
        {
            int result;
            int NaturalDaysToEnd = (EndTime - DateTime.Now).Days;
            int PassWeek = NaturalDaysToEnd / 7;
            int LastDays = NaturalDaysToEnd - (PassWeek * 7);//NaturalDaysToNow % 7;
            result = PassWeek * 5;
            for (int i = 1; i <= LastDays; i++)
            {
                DateTime tempTime = DateTime.Now.AddDays(i);
                if (tempTime.DayOfWeek != DayOfWeek.Saturday && tempTime.DayOfWeek != DayOfWeek.Sunday)
                    result++;
            }
            return result;
        }
    }
}
