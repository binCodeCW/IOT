using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// YH_AlarmMap
    /// </summary>
	public class YH_AlarmMap : BaseBLL<YH_AlarmMapInfo>
    {
        public YH_AlarmMap() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
