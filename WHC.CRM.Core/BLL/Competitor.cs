using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.CRM.Entity;
using WHC.CRM.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;

namespace WHC.CRM.BLL
{
    /// <summary>
    /// 竞争对手信息
    /// </summary>
	public class Competitor : BaseBLL<CompetitorInfo>
    {
        public Competitor() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 根据ID获取竞争对手名称
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public string GetNameById(string id, DbTransaction trans = null)
        {
            return GetFieldValue(id, "Name", trans);
        }

        /// <summary>
        /// 根据竞争对手名称获取实体信息
        /// </summary>
        /// <param name="name">竞争对手名称</param>
        /// <returns></returns>
        public CompetitorInfo FindByName(string name)
        {
            string condition = string.Format("Name='{0}' ", name);
            return FindSingle(condition);
        }
    }
}
