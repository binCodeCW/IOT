using YH.Security.Entity;
using YH.Framework.ControlUtil;

namespace YH.Security.IDAL
{
    public interface ISystemType : IBaseDAL<SystemTypeInfo>
	{
		SystemTypeInfo FindByOID(string oid);
		bool VerifySystem(string serialNumber, string typeID, int authorizeAmount);
	}
}