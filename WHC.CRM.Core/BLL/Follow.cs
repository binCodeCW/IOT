using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.CRM.Entity;
using WHC.CRM.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;

namespace WHC.CRM.BLL
{
    /// <summary>
    /// 客户跟进
    /// </summary>
	public class Follow : BaseBLL<FollowInfo>
    {
        public Follow() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 当新增一条跟进记录的时候，同时更新客户的最新联系日期
        /// </summary>
        /// <param name="obj">客户跟进信息</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public override bool Insert(FollowInfo obj, DbTransaction trans = null)
        {
            bool result = base.Insert(obj, trans);
            if (result)
            {
                BLLFactory<Customer>.Instance.UpdateContactDate(obj.Customer_ID, obj.EditTime, trans);
            }
            return result;
        }

        /// <summary>
        /// 获取记录日期年度列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetYearList()
        {
            IFollow dal = baseDal as IFollow;
            return dal.GetYearList();
        }

        /// <summary>
        /// 根据记录手工编号进行查询，获取对应记录
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="handNo">手工编号</param>
        /// <returns></returns>
        public FollowInfo FindByHandNo(string customerId, string handNo)
        {
            string condition = string.Format("Customer_ID = '{0}' AND HandNo ='{1}' ", customerId, handNo);
            return FindSingle(condition);
        }
    }
}
