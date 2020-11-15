
using WHC.WorkflowLite.Entity;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 资产领用单
    /// </summary>
	public class AssetLy : BaseBLL<AssetLyInfo>
    {
        public AssetLy() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据申请单ID获取对应对象信息
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public AssetLyInfo FindByApplyId(string applyId)
        {
            string condition = string.Format("apply_id='{0}' ", applyId);
            return baseDal.FindSingle(condition);
        }
    }
}
