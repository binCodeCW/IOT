using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// YH04_QualityData
    /// </summary>
	public class YH04_QualityData : BaseBLL<YH04_QualityDataInfo>
    {
        public YH04_QualityData() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
