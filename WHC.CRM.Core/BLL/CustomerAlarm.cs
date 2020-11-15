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
    /// 客户提醒设置
    /// </summary>
	public class CustomerAlarm : BaseBLL<CustomerAlarmInfo>
    {
        public CustomerAlarm() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据用户ID获取其对应客户提醒记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<CustomerAlarmInfo> FindByUser(string userId)
        {
            string condition = string.Format("User_ID='{0}' ", userId);
            return baseDal.Find(condition);
        }
    }
}
