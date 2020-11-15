using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// YH04_TestData
    /// </summary>
	public class YH04_TestData : BaseBLL<YH04_TestDataInfo>
    {
        public YH04_TestData() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
