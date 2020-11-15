using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Web.Mvc;

using YH.Framework.Commons;
using YH.Framework.Commons.Collections;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.Entity;
using YH.Pager.Entity;
using YH.Security.BLL;
using YH.Security.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class OUController : BusinessController<OU, OUInfo>
    {
        public OUController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "父ID,机构编码,机构名称,排序码,机构分类,机构地址,外线电话,内线电话,备注";

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

            List<OUInfo> list = new List<OUInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                int i = 1;
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    OUInfo info = new OUInfo();

                    info.PID = dr["父ID"].ToString().ToInt32();
                    info.HandNo = dr["机构编码"].ToString();
                    info.Name = dr["机构名称"].ToString();
                    info.SortCode = dr["排序码"].ToString();
                    info.Category = dr["机构分类"].ToString();
                    info.Address = dr["机构地址"].ToString();
                    info.OuterPhone = dr["外线电话"].ToString();
                    info.InnerPhone = dr["内线电话"].ToString();
                    info.Note = dr["备注"].ToString();

                    info.Company_ID = CurrentUser.Company_ID;
                    info.CompanyName = CurrentUser.CompanyName;

                    info.Creator = CurrentUser.FullName.ToString();
                    info.Creator = CurrentUser.ID.ToString();
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
        public ActionResult SaveExcelData(List<OUInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                DbTransaction trans = BLLFactory<OU>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (OUInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            detail.Company_ID = CurrentUser.Company_ID;
                            detail.CompanyName = CurrentUser.CompanyName;

                            detail.Creator = CurrentUser.FullName.ToString();
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.CreateTime = DateTime.Now;
                            detail.Editor = CurrentUser.FullName.ToString();
                            detail.Editor_ID = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;

                            BLLFactory<OU>.Instance.Insert(detail, trans);
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
            List<OUInfo> list = new List<OUInfo>();

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
                dr["机构编码"] = list[i].HandNo;
                dr["机构名称"] = list[i].Name;
                dr["排序码"] = list[i].SortCode;
                dr["机构分类"] = list[i].Category;
                dr["机构地址"] = list[i].Address;
                dr["外线电话"] = list[i].OuterPhone;
                dr["内线电话"] = list[i].InnerPhone;
                dr["备注"] = list[i].Note;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/OU.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(OUInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            info.Creator = CurrentUser.FullName.ToString();
            info.Creator = CurrentUser.ID.ToString();
            info.CreateTime = DateTime.Now;
            info.Editor = CurrentUser.FullName.ToString();
            info.Editor_ID = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;
        }

        protected override void OnBeforeUpdate(OUInfo info)
        {
            //子类对参数对象进行修改
            //info.Creator = CurrentUser.FullName.ToString();
            //info.Creator = CurrentUser.ID.ToString();
            //info.CreateTime = DateTime.Now;
            info.Editor = CurrentUser.FullName.ToString();
            info.Editor_ID = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;
        }
        #endregion

        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();

            List<OUInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //如果需要修改字段显示，则参考下面代码处理
            //foreach(OUInfo info in list)
            //{
            //    info.PID = BLLFactory<OU>.Instance.GetFieldValue(info.PID, "Name");
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
        /// 获取组织机构的分类：集团、公司、部门、工作组
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOUCategorysDictJson()
        {
            List<CListItem> treeList = new List<CListItem>();
            string[] enumNames = EnumHelper.GetMemberNames<OUCategoryEnum>();

            foreach (string item in enumNames)
            {
                treeList.Add(new CListItem(item));
            }
            return ToJsonContent(treeList);
        }

        public ActionResult GetListItems()
        {
            List<CListItem> listItem = new List<CListItem>();
            List<OUInfo> list = BLLFactory<OU>.Instance.GetAll();
            foreach (OUInfo info in list)
            {
                listItem.Add(new CListItem(info.Name, info.ID.ToString()));
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTreeList()
        {
            List<OUInfo> comboList = BLLFactory<OU>.Instance.GetAll();
            comboList = CollectionHelper<OUInfo>.Fill(-1, 0, comboList, "PID", "ID", "Name");
            return Json(comboList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUserRelation(string ouid, string addList, string removeList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(ouid) && ValidateUtil.IsInt(ouid))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsInt(id))
                        {
                            BLLFactory<OU>.Instance.RemoveUser(Convert.ToInt32(id), Convert.ToInt32(ouid));
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsInt(id))
                        {
                            BLLFactory<OU>.Instance.AddUser(Convert.ToInt32(id), Convert.ToInt32(ouid));
                        }
                    }
                }

                result.Success = true;
            }
            else
            {
                result.ErrorMessage = "参数ouid不正确";
            }
            return ToJsonContent(result);
        }

        public ActionResult EditOuUsers(string ouid, string newList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(ouid) && ValidateUtil.IsInt(ouid))
            {
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    List<int> list = new List<int>();
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id.ToInt32());
                    }
                    result.Success = BLLFactory<OU>.Instance.EditOuUsers(ouid.ToInt32(), list);
                }
            }
            else
            {
                result.ErrorMessage = "参数ouid不正确";
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 根据角色获取对应的机构
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <returns></returns>
        public ActionResult GetOUsByRole(string roleid)
        {
            if (!string.IsNullOrEmpty(roleid) && ValidateUtil.IsInt(roleid))
            {
                List<OUInfo> list = BLLFactory<OU>.Instance.GetOUsByRole(Convert.ToInt32(roleid));
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }


        /// <summary>
        /// 根据用户ID获取对应的机构关系
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public ActionResult GetOUsByUser(string userid)
        {
            if (!string.IsNullOrEmpty(userid) && ValidateUtil.IsInt(userid))
            {
                List<OUInfo> ouList = BLLFactory<OU>.Instance.GetOUsByUser(Convert.ToInt32(userid));
                return Json(ouList, JsonRequestBehavior.AllowGet);
            }

            return Content("");
        }

        /// <summary>
        /// 新增和编辑同时需要修改的内容
        /// </summary>
        /// <param name="info"></param>
        private void SetCommonInfo(OUInfo info)
        {
            info.Editor = CurrentUser.FullName;
            info.Editor_ID = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;

            OUInfo pInfo = BLLFactory<OU>.Instance.FindByID(info.PID);
            if (pInfo != null)
            {
                //pInfo.Category == "集团" || "公司"
                if (pInfo.Category == "公司")
                {
                    info.Company_ID = pInfo.ID.ToString();
                    info.CompanyName = pInfo.Name;
                }
                else if (pInfo.Category == "部门" || pInfo.Category == "工作组")
                {
                    info.Company_ID = pInfo.Company_ID;
                    info.CompanyName = pInfo.CompanyName;
                }
            }
        }

        public override ActionResult Insert(OUInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            if (info != null)
            {
                try
                {
                    SetCommonInfo(info);
                    string filter = string.Format("Name='{0}' and Company_ID='{1}'", info.Name, info.Company_ID);
                    bool isExist = BLLFactory<OU>.Instance.IsExistRecord(filter);
                    if (isExist)
                    {
                        result.ErrorMessage = "指定机构名称重复，请重新输入！";
                    }
                    else
                    {
                        info.CreateTime = DateTime.Now;
                        info.Creator = CurrentUser.FullName;
                        info.Creator_ID = CurrentUser.ID.ToString();
                        SetCommonInfo(info);

                        result.Success = baseBLL.Insert(info);
                    }
                }
                catch (Exception ex)
                {
                    LogTextHelper.Error(ex);//错误记录
                    result.ErrorMessage = ex.Message;
                }
            }
            return ToJsonContent(result);
        }

        public override ActionResult Insert2(OUInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            int result = -1;
            if (info != null)
            {
                SetCommonInfo(info);
                string filter = string.Format("Name='{0}' and Company_ID='{1}'", info.Name, info.Company_ID);
                bool isExist = BLLFactory<OU>.Instance.IsExistRecord(filter);
                if (isExist)
                {
                    throw new ArgumentException("指定机构名称重复，请重新输入！");
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
        protected override CommonResult Update(string id, OUInfo info)
        {
            string filter = string.Format("Name='{0}' and ID <>{1} and Company_ID='{2}'", info.Name, info.ID, info.Company_ID);
            bool isExist = BLLFactory<OU>.Instance.IsExistRecord(filter);
            if (isExist)
            {
                throw new ArgumentException("指定机构名称重复，请重新输入！");
            }

            SetCommonInfo(info);

            var result = base.Update(id, info);
            return result;
        }

        /// <summary>
        /// 获取公司部门的数量，返回Json字符串，供图表统计
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCompanyDeptCountJson()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            List<OUInfo> ouList = BLLFactory<OU>.Instance.GetTopGroup();
            foreach (OUInfo info in ouList)
            {
                List<OUInfo> companyList = BLLFactory<OU>.Instance.GetAllCompany(info.ID);
                foreach (OUInfo companyInfo in companyList)
                {
                    string condition = string.Format("Company_ID='{0}' AND Deleted=0", companyInfo.ID);
                    int count = BLLFactory<OU>.Instance.GetRecordCount(condition);
                    if (!dict.ContainsKey(companyInfo.Name))
                    {
                        dict.Add(companyInfo.Name, count);
                    }
                }
            }

            return ToJsonContent(dict);
        }

        /// <summary>
        /// 根据机构的ID，获取机构名，并放到缓存里面
        /// </summary>
        /// <param name="deptId">机构ID</param>
        /// <returns></returns>
        public ActionResult GetDeptNameByID(string deptId)
        {
            string result = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
                string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, deptId);
                //string key = string.Format("GetDeptNameByID-{0}", deptId);

                result = MemoryCacheHelper.GetCacheItem<string>(key,
                    delegate () { return BLLFactory<OU>.Instance.GetFieldValue(deptId, "Name"); },
                    new TimeSpan(0, 30, 0));//30分钟过期
            }
            return ToJsonContent(result); ;
        }

        /// <summary>
        /// 获取当前用户的公司部门列表
        /// </summary>
        /// <returns></returns>
        //public ActionResult GetUserDepts()
        //{
        //    if(CurrentUser != null)
        //    {
        //        var list = BLLFactory<OU>.Instance.GetOusByCompany(CurrentUser.Company_ID);
        //        List<CListItem> items = new List<CListItem>();
        //        list.ForEach(s => items.Add(new CListItem(s.Name, s.ID.ToString())));

        //        return ToJsonContent(items);
        //    }
        //    return Content("");
        //}

    }
}
