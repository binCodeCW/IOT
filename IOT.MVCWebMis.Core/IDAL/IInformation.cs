using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.Entity;

namespace IOT.MVCWebMis.IDAL
{
    /// <summary>
    /// 政策法规公告动态
    /// </summary>
	public interface IInformation : IBaseDAL<InformationInfo>
	{
              
        /// <summary>
        /// 获取我的通知信息
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="infoType">信息类型</param>
        /// <returns></returns>
        DataTable GetMyInformation(int userId, InformationCategory infoType);
    }
}