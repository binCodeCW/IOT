using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using WHC.WorkflowLite.Entity;

namespace WHC.WorkflowLite.IDAL
{
    /// <summary>
    /// 出差申请单
    /// </summary>
	public interface IBusinessTrip : IBaseDAL<BusinessTripInfo>
	{
    }
}