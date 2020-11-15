﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;

using YH.Framework.Commons;
using YH.Security.BLL;
using YH.Security.Entity;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// 角色可访问数据（组织机构）的控制器类
    /// </summary>
    public class RoleDataController : BusinessController<RoleData, RoleDataInfo>
    {
        public RoleDataController() : base()
        {
        }

        /// <summary>
        /// 保存角色的数据权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="ouList">所属公司或者部门，逗号分开</param>
        /// <returns></returns>
        public ActionResult UpdateData(int roleId, string ouList)
        {
            List<int> companyList = new List<int>();
            List<int> deptList = new List<int>();

            foreach(int id in ouList.ToDelimitedList<int>(","))
            {
                OUInfo info = BLLFactory<OU>.Instance.FindByID(id);
                if(info != null && info.Category == OUCategoryEnum.公司.ToString())
                {
                    companyList.Add(id);
                }
                else
                {
                    deptList.Add(id);
                }
            }

            string belongCompanys = string.Join(",", companyList);
            string belongDepts = string.Join(",", deptList);

            CommonResult result = new CommonResult();
            try
            {
                result.Success = BLLFactory<RoleData>.Instance.UpdateRoleData(roleId, belongCompanys, belongDepts);
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);
                result.ErrorMessage = ex.Message;
            }

            return ToJsonContent(result);
        }


        /// <summary>
        /// 保存角色的数据权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="belongCompanys">所属公司，逗号分开</param>
        /// <param name="belongDepts">所属机构，逗号分开</param>
        /// <returns></returns>
        public ActionResult UpdateRoleData(int roleId, string belongCompanys, string belongDepts)
        {
            CommonResult result = new CommonResult();
            try
            {
                result.Success = BLLFactory<RoleData>.Instance.UpdateRoleData(roleId, belongCompanys, belongDepts);
            }
            catch(Exception ex)
            {
                LogTextHelper.Error(ex);
                result.ErrorMessage = ex.Message;
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 获取角色包含的数据权限（组织机构ID列表）
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public ActionResult GetRoleDataList(int roleId)
        {
            Dictionary<int,int> dict = BLLFactory<RoleData>.Instance.GetRoleDataDict(roleId);

            List<int> list = new List<int>(); 
            list.AddRange(dict.Keys);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
