using YH.Security.Entity;
using YH.Framework.ControlUtil;

namespace YH.Security.IDAL
{
    /// <summary>
    /// 用户登录日志信息
    /// </summary>
    public interface ILoginLog : IBaseDAL<LoginLogInfo>
    {
        /// <summary>
        /// 获取上一次（非刚刚登录）的登录日志
        /// </summary>
        /// <param name="userId">登录用户ID</param>
        /// <returns></returns>
        LoginLogInfo GetLastLoginInfo(string userId);
    }
}