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
    /// 客户提醒设置
    /// </summary>
	public interface ICustomerAlarm : IBaseDAL<CustomerAlarmInfo>
	{
    }
}