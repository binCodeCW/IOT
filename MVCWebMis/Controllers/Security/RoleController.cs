﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web.Mvc;

using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.Entity;
using YH.Pager.Entity;
using YH.Security.BLL;
using YH.Security.Entity;

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// 角色业务操作控制器
    /// </summary>
    public class RoleController : BusinessController<Role, RoleInfo>
    {       
        public RoleController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "父ID,角色编码,角色名称,备注,排序码,角色分类";

        /// <summary>
        /// 检查Excel文件的字段是否包含了必须的字段
        /// </summary>
        /// <param name="guid">附件的GUID</param>
        /// <returns></returns>
        public ActionResult CheckExcelColumns(string guid)
        {
            CommonResult result = new CommonResult();

            try
            {
                DataTable dt = ConvertExcelFileToTable(guid);
                if (dt != null)
                {
                    //检查列表是否包含必须的字段
                    result.Success = DataTableHelper.ContainAllColumns(dt, columnString);
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);
                result.ErrorMessage = ex.Message;
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 获取服务器上的Excel文件，并把它转换为实体列表返回给客户端
        /// </summary>
        /// <param name="guid">附件的GUID</param>
        /// <returns></returns>
        public ActionResult GetExcelData(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }

            List<RoleInfo> list = new List<RoleInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;
                    RoleInfo info = new RoleInfo();

                    info.PID = dr["父ID"].ToString().ToInt32();
                    info.HandNo = dr["角色编码"].ToString();
                    info.Name = dr["角色名称"].ToString();
                    info.Note = dr["备注"].ToString();
                    info.SortCode = dr["排序码"].ToString();
                    info.Category = dr["角色分类"].ToString();

                    info.Company_ID = CurrentUser.Company_ID;
                    info.CompanyName = CurrentUser.CompanyName;
                    info.Creator = CurrentUser.FullName.ToString();
                    info.Creator_ID = CurrentUser.ID.ToString();
                    info.CreateTime = DateTime.Now;
                    info.Editor = CurrentUser.FullName.ToString();
                    info.Editor_ID = CurrentUser.ID.ToString();
                    info.EditTime = DateTime.Now;

                    list.Add(info);
                }
                #endregion
            }

            var result = new { total = list.Count, rows = list };
            return ToJsonContent(result);
        }

        /// <summary>
        /// 保存客户端上传的相关数据列表
        /// </summary>
        /// <param name="list">数据列表</param>
        /// <returns></returns>
        public ActionResult SaveExcelData(List<RoleInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                DbTransaction trans = BLLFactory<Role>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (RoleInfo info in list)
                        {
                            //detail.Seq = seq++;//增加1
                            info.Company_ID = CurrentUser.Company_ID;
                            info.CompanyName = CurrentUser.CompanyName;
                            info.Creator = CurrentUser.FullName.ToString();
                            info.Creator_ID = CurrentUser.ID.ToString();
                            info.CreateTime = DateTime.Now;
                            info.Editor = CurrentUser.FullName.ToString();
                            info.Editor_ID = CurrentUser.ID.ToString();
                            info.EditTime = DateTime.Now;

                            BLLFactory<Role>.Instance.Insert(info, trans);
                        }
                        trans.Commit();
                        result.Success = true;
                    }
                    catch (Exception ex)
                    {
                        LogTextHelper.Error(ex);
                        result.ErrorMessage = ex.Message;
                        trans.Rollback();
                    }
                }
                #endregion
            }
            else
            {
                result.ErrorMessage = "导入信息不能为空";
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 根据查询条件导出列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Export()
        {
            #region 根据参数获取List列表
            string where = GetPagerCondition();
            string CustomedCondition = Request["CustomedCondition"] ?? "";
            List<RoleInfo> list = new List<RoleInfo>();

            if (!string.IsNullOrWhiteSpace(CustomedCondition))
            {
                //如果为自定义的json参数列表，那么可以使用字典反序列化获取参数，然后处理
                //Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(CustomedCondition);

                //如果是条件的自定义，可以使用Find查找
                list = baseBLL.Find(CustomedCondition);
            }
            else
            {
                list = baseBLL.Find(where);
            }

            #endregion

            #region 把列表转换为DataTable
            DataTable datatable = DataTableHelper.CreateTable("序号|int," + columnString);
            DataRow dr;
            int j = 1;
            for (int i = 0; i < list.Count; i++)
            {
                dr = datatable.NewRow();
                dr["序号"] = j++;
                dr["父ID"] = list[i].PID;
                dr["角色编码"] = list[i].HandNo;
                dr["角色名称"] = list[i].Name;
                dr["备注"] = list[i].Note;
                dr["排序码"] = list[i].SortCode;
                dr["角色分类"] = list[i].Category;

                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/Role.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(RoleInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            //info.Company_ID = CurrentUser.Company_ID;
            //info.CompanyName = CurrentUser.CompanyName;
            info.Creator = CurrentUser.FullName.ToString();
            info.Creator_ID = CurrentUser.ID.ToString();
            info.CreateTime = DateTime.Now;
            info.Editor = CurrentUser.FullName.ToString();
            info.Editor_ID = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;
        }

        protected override void OnBeforeUpdate(RoleInfo info)
        {
            //子类对参数对象进行修改
            //info.Company_ID = CurrentUser.Company_ID;
            //info.CompanyName = CurrentUser.CompanyName;
            //info.Creator = CurrentUser.FullName.ToString();
            //info.Creator_ID = CurrentUser.ID.ToString();
            //info.CreateTime = DateTime.Now;
            info.Editor = CurrentUser.FullName.ToString();
            info.Editor_ID = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;
        }
        #endregion

        /// <summary>
        /// 重载角色分页的函数
        /// </summary>
        /// <returns></returns>
        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();

            List<RoleInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //如果需要修改字段显示，则参考下面代码处理
            //foreach(RoleInfo info in list)
            //{
            //    info.PID = BLLFactory<Role>.Instance.GetFieldValue(info.PID, "Name");
            //    if (!string.IsNullOrEmpty(info.Creator))
            //    {
            //        info.Creator = BLLFactory<User>.Instance.GetFullNameByID(info.Creator.ToInt32());
            //    }
            //}

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = list };
            return ToJsonContent(result);
        }


        /// <summary>
        /// 获取角色分类：系统角色、业务角色、应用角色...
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleCategorys()
        {
            List<CListItem> listItem = new List<CListItem>();
            string[] enumNames = EnumHelper.GetMemberNames<RoleCategoryEnum>();

            foreach (string item in enumNames)
            {
                listItem.Add(new CListItem(item));
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUsers(string roleId, string newList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsInt(roleId))
            {
                List<int> list = new List<int>();
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id.ToInt32());
                    }
                }

                result.Success = BLLFactory<Role>.Instance.EditRoleUsers(roleId.ToInt32(), list);
            }
            else
            {
                result.ErrorMessage = "参数roleId不正确";
            }
            return ToJsonContent(result);
        }

        public ActionResult EditOUs(string roleId, string newList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsInt(roleId))
            {
                List<int> list = new List<int>();
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id.ToInt32());
                    }
                }

                result.Success = BLLFactory<Role>.Instance.EditRoleOUs(roleId.ToInt32(), list);
            }
            else
            {
                result.ErrorMessage = "参数roleId不正确";
            }
            return ToJsonContent(result);
        }

        public ActionResult EditFunctions(string roleId, string newList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsInt(roleId))
            {
                List<string> list = new List<string>();
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    foreach (string id in newList.Split(','))
                    {
                        if (id != ConfigData.SystemType)
                        {
                            list.Add(id);
                        }
                    }
                }

                result.Success = BLLFactory<Role>.Instance.EditRoleFunctions(roleId.ToInt32(), list, ConfigData.SystemType);
            }
            else
            {
                result.ErrorMessage = "参数roleId不正确";
            }
            return ToJsonContent(result);
        }

        public ActionResult EditMenus(string roleId, string newList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsInt(roleId))
            {
                List<string> list = new List<string>();
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    foreach (string id in newList.Split(','))
                    {
                        if (id != ConfigData.SystemType)
                        {
                            list.Add(id);
                        }
                    }
                }

                result.Success = BLLFactory<Role>.Instance.EditRoleMenus(roleId.ToInt32(), list, ConfigData.SystemType);
            }
            else
            {
                result.ErrorMessage = "参数roleId不正确";
            }
            return ToJsonContent(result);
        }

        public ActionResult EditUserRelation(string roleId, string addList, string removeList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsInt(id))
                        {
                            BLLFactory<Role>.Instance.RemoveUser(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsInt(id))
                        {
                            BLLFactory<Role>.Instance.AddUser(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }
                result.Success = true;
            }
            else
            {
                result.ErrorMessage = "参数roleId不正确";
            }
            return ToJsonContent(result);
        }

        public ActionResult EditOURelation(string roleId, string addList, string removeList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsInt(id))
                        {
                            BLLFactory<Role>.Instance.RemoveOU(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsInt(id))
                        {
                            BLLFactory<Role>.Instance.AddOU(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }
                result.Success = true;
            }
            else
            {
                result.ErrorMessage = "参数roleId不正确";
            }
            return ToJsonContent(result);
        }

        public ActionResult EditFunctionRelation(string roleId, string addList, string removeList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            BLLFactory<Role>.Instance.RemoveFunction(id, Convert.ToInt32(roleId));
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            BLLFactory<Role>.Instance.AddFunction(id, Convert.ToInt32(roleId));
                        }
                    }
                }
                result.Success = true;
            }
            else
            {
                result.ErrorMessage = "参数roleId不正确";
            }
            return ToJsonContent(result);
        }

        public ActionResult GetRolesByUser(string userid)
        {
            if (!string.IsNullOrEmpty(userid) && ValidateUtil.IsInt(userid))
            {
                List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByUser(Convert.ToInt32(userid));
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }

            return Content("");
        }

        /// <summary>
        /// 获取用户第一个角色，用于界面绑定显示
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserRoleIds(int userid)
        {
            List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByUser(userid);
            List<int> list = new List<int>();
            roleList.ForEach(s => list.Add(s.ID));

            return Content(string.Join(",", list));
        }

        public ActionResult GetRolesByFunction(string functionId)
        {
            if (!string.IsNullOrEmpty(functionId))
            {
                List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByFunction(functionId);
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }
                   
        /// <summary>
        /// 根据公司ID（机构ID）获取对应的角色列表
        /// </summary>
        /// <param name="companyId">公司ID（机构ID）</param>
        /// <returns></returns>
        public ActionResult GetRolesByCompany()
        {            
            if (CurrentUser != null)
            {
                string companyId = CurrentUser.Company_ID;
                List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByCompany(companyId);
                List<CListItem> list = new List<CListItem>();
                roleList.ForEach(s => list.Add(new CListItem(s.Name, s.ID.ToString())));

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }

        public ActionResult GetRolesByOU(string ouid)
        {
            if (!string.IsNullOrEmpty(ouid) && ValidateUtil.IsInt(ouid))
            {
                List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByOU(Convert.ToInt32(ouid));
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }

        /// <summary>
        /// 新增和编辑同时需要修改的内容
        /// </summary>
        /// <param name="info"></param>
        private void SetCommonInfo(RoleInfo info)
        {
            info.Editor = CurrentUser.FullName;
            info.Editor_ID = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;

            OUInfo companyInfo = BLLFactory<OU>.Instance.FindByID(info.Company_ID);
            if (companyInfo != null)
            {
                info.CompanyName = companyInfo.Name;
            }
        }

        public override ActionResult Insert(RoleInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);
                       
            CommonResult result = new CommonResult();
            if (info != null)
            {
                string filter = string.Format("Name='{0}'  and Company_ID={1}", info.Name, info.Company_ID);
                bool isExist = BLLFactory<Role>.Instance.IsExistRecord(filter);
                if (isExist)
                {
                    result.ErrorMessage = "指定角色名称重复，请重新输入！";
                }
                else
                {
                    try
                    {
                        info.CreateTime = DateTime.Now;
                        info.Creator = CurrentUser.FullName;
                        info.Creator_ID = CurrentUser.ID.ToString();
                        SetCommonInfo(info);

                        result.Success = baseBLL.Insert(info);
                    }
                    catch (Exception ex)
                    {
                        LogTextHelper.Error(ex);//错误记录
                        result.ErrorMessage = ex.Message;
                    }
                }
            }

            return ToJsonContent(result);
        }

        public override ActionResult Insert2(RoleInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            int result = -1;
            if (info != null)
            {
                string filter = string.Format("Name='{0}' and Company_ID={1}", info.Name, info.Company_ID);
                bool isExist = BLLFactory<Role>.Instance.IsExistRecord(filter);
                if (isExist)
                {
                    throw new ArgumentException("指定角色名称重复，请重新输入！");
                }

                info.CreateTime = DateTime.Now;
                info.Creator = CurrentUser.FullName;
                info.Creator_ID = CurrentUser.ID.ToString();
                SetCommonInfo(info);
                result = baseBLL.Insert2(info);
            }
            return Content(result);
        }

        /// <summary>
        /// 重写方便写入公司、部门、编辑时间的名称等信息
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <param name="info">对象信息</param>
        /// <returns></returns>
        protected override CommonResult Update(string id, RoleInfo info)
        {
            string filter = string.Format("Name='{0}' and ID <>'{1}' and Company_ID={2}", info.Name, info.ID, info.Company_ID);
            bool isExist = BLLFactory<Role>.Instance.IsExistRecord(filter);
            if (isExist)
            {
                throw new ArgumentException("指定角色名称重复，请重新输入！");
            }

            SetCommonInfo(info);

            return base.Update(id, info);
        }

        /// <summary>
        /// 获取用户的部门角色树结构(分级需要）（bootstrap的JSTree)
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public ActionResult GetMyRoleJsTreeJson(int userId)
        {
            StringBuilder content = new StringBuilder();
            UserInfo userInfo = BLLFactory<User>.Instance.FindByID(userId);
            if (userInfo != null)
            {
                List<OUInfo> list = BLLFactory<OU>.Instance.GetMyTopGroup(CurrentUser.ID);
                foreach (OUInfo groupInfo in list)
                {
                    if (groupInfo != null && !groupInfo.Deleted)
                    {
                        JsTreeData topnode = new JsTreeData("dept" + groupInfo.ID, groupInfo.Name, GetBootstrapIcon(groupInfo.Category));
                        AddJsRole(groupInfo, topnode);

                        if (groupInfo.Category == "集团")
                        {
                            List<OUInfo> sublist = BLLFactory<OU>.Instance.GetAllCompany(groupInfo.ID);
                            foreach (OUInfo info in sublist)
                            {
                                if (!info.Deleted)
                                {
                                    JsTreeData companyNode = new JsTreeData("dept" + info.ID, info.Name, GetBootstrapIcon(info.Category));
                                    topnode.children.Add(companyNode);

                                    AddJsRole(info, companyNode);
                                }
                            }
                        }

                        content.Append(base.ToJson(topnode));
                    }
                }
            }

            string json = string.Format("[{0}]", content.ToString().Trim(','));
            return Content(json);
        }

        private void AddJsRole(OUInfo ouInfo, JsTreeData treeNode)
        {
            List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByCompany(ouInfo.ID.ToString());
            foreach (RoleInfo roleInfo in roleList)
            {
                JsTreeData roleNode = new JsTreeData("role" + roleInfo.ID, roleInfo.Name, "fa fa-user icon-state-info icon-lg");
                treeNode.children.Add(roleNode);
            }
        }
    }
}
