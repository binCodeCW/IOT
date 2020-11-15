using System.Data;

using IOT.MVCWebMis.Entity;
using IOT.MVCWebMis.IDAL;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// 政策法规公告动态
    /// </summary>
	public class Information : BaseBLL<InformationInfo>
    {
        public Information() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 获取我的通知信息
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="infoType">信息类型</param>
        /// <returns></returns>
        public DataTable GetMyInformation(int userId, InformationCategory infoType)
        {
            IInformation dal = baseDal as IInformation;
            return dal.GetMyInformation(userId, InformationCategory.通知公告);
        }

    }
}
