using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// YH_DeviceControl
    /// </summary>
	public class YH_DeviceControl : BaseBLL<YH_DeviceControlInfo>
    {
        public YH_DeviceControl() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
