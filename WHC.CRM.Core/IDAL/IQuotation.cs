using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using WHC.CRM.Entity;

namespace WHC.CRM.IDAL
{
    /// <summary>
    /// 产品报价单
    /// </summary>
	public interface IQuotation : IBaseDAL<QuotationInfo>
	{
        /// <summary>
        /// 获取报价单日期年度列表
        /// </summary>
        /// <returns></returns>
        List<string> GetOrderYearList();
    }
}