using YH.Security.Entity;
using YH.Framework.ControlUtil;

namespace YH.Security.BLL
{
    /// <summary>
    /// 字段权限域对象
    /// </summary>
	public class FieldDomain : BaseBLL<FieldDomainInfo>
    {
        public FieldDomain() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
