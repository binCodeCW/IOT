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
    /// 资产盘点主表
    /// </summary>
	public interface IAssetCheck : IBaseDAL<AssetCheckInfo>
	{
    }
}