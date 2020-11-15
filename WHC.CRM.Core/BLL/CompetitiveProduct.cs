using System.Data.Common;

using WHC.CRM.Entity;
using YH.Framework.ControlUtil;

namespace WHC.CRM.BLL
{
    /// <summary>
    /// 竞争对手产品信息
    /// </summary>
	public class CompetitiveProduct : BaseBLL<CompetitiveProductInfo>
    {
        public CompetitiveProduct() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 根据ID获取产品名称
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public string GetNameById(string id, DbTransaction trans = null)
        {
            return GetFieldValue(id, "ProductName", trans);
        }
    }
}
