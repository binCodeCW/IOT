using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 存放地点
    /// </summary>
	public class StoreAddress : BaseBLL<StoreAddressInfo>
    {
        public StoreAddress() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据部门ID获取对应对象信息
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public StoreAddressInfo FindByDeptId(string deptId)
        {
            string condition = string.Format("Dept_ID='{0}' ", deptId);
            return baseDal.FindSingle(condition);
        }
    }
}
