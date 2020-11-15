using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// YH_DeviceAlarm
    /// </summary>
	public class YH_DeviceAlarm : BaseBLL<YH_DeviceAlarmInfo>
    {
        public YH_DeviceAlarm() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
