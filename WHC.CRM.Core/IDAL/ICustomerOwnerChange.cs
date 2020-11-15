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
    /// 客户所属人员变更列表
    /// </summary>
	public interface ICustomerOwnerChange : IBaseDAL<CustomerOwnerChangeInfo>
	{
    }
}