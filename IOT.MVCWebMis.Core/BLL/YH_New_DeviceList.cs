using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// YH_New_DeviceList
    /// </summary>
	public class YH_New_DeviceList : BaseBLL<YH_New_DeviceListInfo>
    {
        public YH_New_DeviceList() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
