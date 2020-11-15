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
    /// 销售阶段跟进记录
    /// </summary>
	public interface ISaleChanceFollow : IBaseDAL<SaleChanceFollowInfo>
	{
    }
}