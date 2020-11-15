using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 资产盘点主表
    /// </summary>
	public class AssetCheck : BaseBLL<AssetCheckInfo>
    {
        public AssetCheck() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
        /// <summary>
        /// 生成单据号码
        /// </summary>
        /// <param name="userId">操作人员ID</param>
        /// <returns></returns>
        public string GetBillNo(int userId, DbTransaction trans = null)
        {
            string prefix = string.Format("PD-{0}-{1}", userId, DateTime.Now.ToString("yyyyMMdd"));

            //获取当天的记录数量+1
            DateTime dt = DateTime.Now.ToString("yyyy-MM-dd").ToDateTime(); //当前日期
            SearchCondition condition = new SearchCondition();
            condition.AddCondition("CreateTime", dt, SqlOperator.MoreThanOrEqual)
                     .AddCondition("CreateTime", dt.AddDays(1), SqlOperator.LessThan);
            string conditionSql = condition.BuildConditionSql().Replace("Where", "");
            int count = baseDal.GetRecordCount(conditionSql) + 1;

            //循环检索，直到不重复的编号
            string number = string.Format("{0}-{1}", prefix, count);
            while (true)
            {
                if (CheckBillNoExist(number, trans))
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

        private bool CheckBillNoExist(string billNo, DbTransaction trans = null)
        {
            return base.IsExistKey("BillNo", billNo, trans);
        }

        /// <summary>
        /// 根据盘点单号进行查询特定的记录
        /// </summary>
        /// <param name="billNo">盘点单号</param>
        /// <returns></returns>
        public AssetCheckInfo FindByBillNo(string billNo, DbTransaction trans = null)
        {
            AssetCheckInfo info = null;
            if (!string.IsNullOrEmpty(billNo))
            {
                var condition = string.Format("BillNo='{0}'", billNo);
                info = baseDal.FindSingle(condition, trans);
            }
            return info;
        }
    }
}
