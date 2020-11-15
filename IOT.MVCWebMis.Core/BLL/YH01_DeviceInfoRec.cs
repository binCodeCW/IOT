using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// YH01_DeviceInfoRec
    /// </summary>
	public class YH01_DeviceInfoRec : BaseBLL<YH01_DeviceInfoRecInfo>
    {
        public YH01_DeviceInfoRec() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
