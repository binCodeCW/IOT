using System;
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
    /// 报销明细
    /// </summary>
	public class ReimbursementDetail : BaseBLL<ReimbursementDetailInfo>
    {
        public ReimbursementDetail() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }


        /// <summary>
        /// 根据主表ID获取对应明细对象信息
        /// </summary>
        /// <param name="headerId">主表ID</param>
        /// <returns></returns>
        public List<ReimbursementDetailInfo> FindByHeaderId(string headerId)
        {
            string condition = string.Format("Header_ID='{0}' ", headerId);
            return baseDal.Find(condition);
        }
    }
}
