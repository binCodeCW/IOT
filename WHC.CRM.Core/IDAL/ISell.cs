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
    /// 产品销售记录
    /// </summary>
	public interface ISell : IBaseDAL<SellInfo>
	{
        /// <summary>
        /// 获取订单日期年度列表
        /// </summary>
        /// <returns></returns>
        List<string> GetOrderYearList();

        /// <summary>
        /// 获取订单统计报表的一些数据
        /// </summary>
        /// <param name="fieldName">统计字段名称，可选为ProductName, ProductType, Model</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        DataTable GetDetailStatData(string fieldName, string condition);
    }
}