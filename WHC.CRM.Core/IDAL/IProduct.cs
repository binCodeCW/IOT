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
    /// 产品信息
    /// </summary>
	public interface IProduct : IBaseDAL<ProductInfo>
	{        
        /// <summary>
        /// 根据产品ID，增加或减少相关产品的数量
        /// </summary>
        /// <param name="id">产品ID</param>
        /// <param name="quantity">产品数量，正数为增加，负数为减少</param>
        /// <returns></returns>
        bool ModifyQuantity(string id, int quantity, DbTransaction trans = null);
    }
}