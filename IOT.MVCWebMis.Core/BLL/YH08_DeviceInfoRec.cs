using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// YH08_DeviceInfoRec
    /// </summary>
	public class YH08_DeviceInfoRec : BaseBLL<YH08_DeviceInfoRecInfo>
    {
        public YH08_DeviceInfoRec() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
