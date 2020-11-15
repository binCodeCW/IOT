using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// 收到协议
    /// </summary>
	public class ReceiveProtocal : BaseBLL<ReceiveProtocalInfo>
    {
        public ReceiveProtocal() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
