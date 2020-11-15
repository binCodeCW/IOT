using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// YH01_DeviceDataRec
    /// </summary>
	public class YH01_DeviceDataRec : BaseBLL<YH01_DeviceDataRecInfo>
    {
        public YH01_DeviceDataRec() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
