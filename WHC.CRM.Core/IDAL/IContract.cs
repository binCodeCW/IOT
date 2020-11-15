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
    /// 客户合同信息
    /// </summary>
	public interface IContract : IBaseDAL<ContractInfo>
	{      
        /// <summary>
        /// 获取合同签约年度列表
        /// </summary>
        /// <returns></returns>
        List<string> GetSignYearList();
    }
}