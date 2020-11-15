using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// YH_DeviceInfo
    /// </summary>
	public class YH_DeviceInfo : BaseBLL<YH_DeviceInfoInfo>
    {
        public YH_DeviceInfo() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
