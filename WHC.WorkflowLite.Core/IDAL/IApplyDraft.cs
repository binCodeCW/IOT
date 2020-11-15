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
    /// 申请单草稿（通用存储）
    /// </summary>
	public interface IApplyDraft : IBaseDAL<ApplyDraftInfo>
	{
    }
}