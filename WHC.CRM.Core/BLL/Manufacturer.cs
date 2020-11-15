using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.CRM.Entity;
using WHC.CRM.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;


namespace WHC.CRM.BLL
{
    /// <summary>
    /// 供应商信息
    /// </summary>
	public class Manufacturer : BaseBLL<ManufacturerInfo>
    {
        public Manufacturer() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 根据ID获取供应商名称
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public string GetNameById(string id, DbTransaction trans = null)
        { 
            System.Reflection.MethodBase method =System.Reflection.MethodBase.GetCurrentMethod();
            string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, id);
            string name = MemoryCacheHelper.GetCacheItem<string>(key, delegate()
            {
                return GetFieldValue(id, "Name", trans);
            },
            new TimeSpan(0, 30, 0));
            return name;
        }

        /// <summary>
        /// 根据名称获取实体信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public ManufacturerInfo FindByName(string name)
        {
            string condition = string.Format("Name='{0}' ", name);
            return FindSingle(condition);
        }
    }
}
