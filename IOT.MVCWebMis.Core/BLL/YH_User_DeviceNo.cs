using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using IOT.MVCWebMis.Entity;
//using WHC.TestProject.IDAL;
//using WHC.Pager.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// 用户和仪器编号配置表
    /// </summary>
	public class YH_User_DeviceNo : BaseBLL<YH_User_DeviceNoInfo>
    {
        public YH_User_DeviceNo() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据登陆用户名获取其对应的仪器编号
        /// </summary>
        /// <param name="Name">用户名/登陆名</param>
        /// <returns></returns>
        public List<YH_User_DeviceNoInfo> FindByUser(string Name)
        {
            string condition = string.Format("Name='{0}' ", Name);
            return baseDal.Find(condition);
        }
    }
}
