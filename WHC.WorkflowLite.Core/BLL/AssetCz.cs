﻿using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 资产处置单
    /// </summary>
	public class AssetCz : BaseBLL<AssetCzInfo>
    {
        public AssetCz() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据申请单ID获取对应对象信息
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public AssetCzInfo FindByApplyId(string applyId)
        {
            string condition = string.Format("apply_id='{0}' ", applyId);
            return baseDal.FindSingle(condition);
        }
    }
}
