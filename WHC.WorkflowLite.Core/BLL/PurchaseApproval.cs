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
    /// 采购申请单
    /// </summary>
	public class PurchaseApproval : BaseBLL<PurchaseApprovalInfo>
    {
        public PurchaseApproval() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据申请单ID获取对应对象信息
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public PurchaseApprovalInfo FindByApplyId(string applyId)
        {
            string condition = string.Format("apply_id='{0}' ", applyId);
            return baseDal.FindSingle(condition);
        }
    }
}
