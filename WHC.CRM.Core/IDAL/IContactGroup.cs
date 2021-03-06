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
    /// 联系人组别
    /// </summary>
	public interface IContactGroup : IBaseDAL<ContactGroupInfo>
	{              
        /// <summary>
        /// 根据用户，获取树形结构的分组列表
        /// </summary>
        List<ContactGroupNodeInfo> GetTree(string creator);

        /// <summary>
        /// 根据联系人ID，获取客户对应的分组集合
        /// </summary>
        /// <param name="contactId">联系人ID</param>
        /// <returns></returns>
        List<ContactGroupInfo> GetByContact(string contactId);
    }
}