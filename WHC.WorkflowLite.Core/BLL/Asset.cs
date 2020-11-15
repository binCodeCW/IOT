using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 实物资产表
    /// </summary>
	public class Asset : BaseBLL<AssetInfo>
    {
        public Asset() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据资产编码进行查询特定的记录
        /// </summary>
        /// <param name="code">资产编码</param>
        /// <returns></returns>
        public AssetInfo FindByCode(string code, DbTransaction trans = null)
        {
            AssetInfo info = null;
            if(!string.IsNullOrEmpty(code))
            {
                var condition = string.Format("Code='{0}'", code);
                info = baseDal.FindSingle(condition, trans);
            }
            return info;
        }


        /// <summary>
        /// 获取指定条件的资产总数
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public int GetAssetQty(string condition, DbTransaction trans = null)
        {
            string sql = string.Format("select sum(qty) from T_Asset where {0}", condition);
            int count = BLLFactory<Asset>.Instance.SqlValueList(sql).ToInt32();
            return count;
        }
    }
}
