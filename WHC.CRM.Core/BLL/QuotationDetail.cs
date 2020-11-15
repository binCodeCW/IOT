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
    /// 报价单明细
    /// </summary>
	public class QuotationDetail : BaseBLL<QuotationDetailInfo>
    {
        public QuotationDetail() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 根据报价单编号获取报价单明细
        /// </summary>
        /// <param name="orderNo">报价单编号</param>
        /// <returns></returns>
        public List<QuotationDetailInfo> FindByOrderNo(string orderNo, DbTransaction trans = null)
        {
            string condition = string.Format("OrderNo='{0}' ", orderNo);
            return Find(condition, trans);
        }
    }
}
