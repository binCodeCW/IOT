using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// YH08_DeviceDataRec
    /// </summary>
	public class YH08_DeviceDataRec : BaseBLL<YH08_DeviceDataRecInfo>
    {
        public YH08_DeviceDataRec() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
