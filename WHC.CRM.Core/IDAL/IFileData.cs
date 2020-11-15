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
    /// 客户资料管理
    /// </summary>
	public interface IFileData : IBaseDAL<FileDataInfo>
    {      
        /// <summary>
        /// 获取记录日期年度列表
        /// </summary>
        /// <returns></returns>
        List<string> GetYearList();
    }
}