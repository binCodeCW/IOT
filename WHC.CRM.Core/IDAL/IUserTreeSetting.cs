using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using WHC.CRM.Entity;

namespace WHC.CRM.IDAL
{
    /// <summary>
    /// 用户配置的系统列表集合
    /// </summary>
	public interface IUserTreeSetting : IBaseDAL<UserTreeSettingInfo>
	{
    }
}