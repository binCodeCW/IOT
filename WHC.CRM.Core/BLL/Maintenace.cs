using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.CRM.Entity;
using WHC.CRM.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;

namespace WHC.CRM.BLL
{
    /// <summary>
    /// 客户维修维护
    /// </summary>
	public class Maintenace : BaseBLL<MaintenaceInfo>
    {
        public Maintenace() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 生成单据号码
        /// </summary>
        /// <param name="userId">操作人员ID</param>
        /// <returns></returns>
        public string GetHandNo(int userId)
        {
            string prefix = string.Format("WH-{0}-{1}", userId, DateTime.Now.ToString("yyyyMMdd"));

            //获取当天的记录数量+1
            DateTime dt = DateTime.Now.ToString("yyyy-MM-dd").ToDateTime(); //当前日期
            SearchCondition condition = new SearchCondition();
            condition.AddCondition("EditTime", dt, SqlOperator.MoreThanOrEqual)
                     .AddCondition("EditTime", dt.AddDays(1), SqlOperator.LessThan);
            string conditionSql = condition.BuildConditionSql().Replace("Where", "");
            int count = baseDal.GetRecordCount(conditionSql) + 1;

            //循环检索，直到不重复的编号
            string number = string.Format("{0}-{1}", prefix, count);
            while (true)
            {
                if (CheckNumberExist(number))
                {
                    number = string.Format("{0}-{1}", prefix, count++);
                }
                else
                {
                    break;
                }
            }

            return number;
        }

        private bool CheckNumberExist(string handNo)
        {
            return base.IsExistKey("HandNo", handNo);
        }

        /// <summary>
        /// 获取记录日期年度列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetYearList()
        {
            IMaintenace dal = baseDal as IMaintenace;
            return dal.GetYearList();
        }
    }
}
