using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 资产处置明细
    /// </summary>
	public class AssetCzDetail : BaseBLL<AssetCzDetailInfo>
    {
        public AssetCzDetail() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据领用单号获取对应对象信息
        /// </summary>
        /// <param name="billNo">申请单ID</param>
        /// <returns></returns>
        public List<AssetCzDetailInfo> FindByBillNo(string billNo)
        {
            string condition = string.Format("BillNo='{0}' ", billNo);
            return baseDal.Find(condition);
        }
    }
}
