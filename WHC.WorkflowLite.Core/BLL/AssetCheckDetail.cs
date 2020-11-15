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
    /// 资产盘点明细
    /// </summary>
	public class AssetCheckDetail : BaseBLL<AssetCheckDetailInfo>
    {
        public AssetCheckDetail() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据盘点单号进行查询特定的记录
        /// </summary>
        /// <param name="billNo">盘点单号</param>
        /// <returns></returns>
        public List<AssetCheckDetailInfo> FindByBillNo(string billNo)
        {
            List<AssetCheckDetailInfo> list = null;
            if (!string.IsNullOrEmpty(billNo))
            {
                var condition = string.Format("BillNo='{0}'", billNo);
                list = baseDal.Find(condition);
            }
            return list;
        }
    }
}
