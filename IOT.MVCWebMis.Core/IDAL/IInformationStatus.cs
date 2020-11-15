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
    /// 用户对指定内容的操作状态记录
    /// </summary>
	public interface IInformationStatus : IBaseDAL<InformationStatusInfo>
	{
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="InfoType">信息类型</param>
        /// <param name="InfoID">信息主键ID</param>
        /// <param name="Status">状态</param>
        bool SetStatus(string UserID, InformationCategory InfoType, string InfoID, int Status);

        /// <summary>
        /// 匹配状态
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="InfoType">信息类型</param>
        /// <param name="InfoID">信息主键ID</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        bool CheckStatus(string UserID, InformationCategory InfoType, string InfoID, int Status);
    }
}