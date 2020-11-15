﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;

using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Security.BLL;
using YH.Security.Entity;
using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;
using System.Data;
using System.Data.Common;

namespace IOT.MVCWebMis.Controllers
{
    public class LoginLogController : BusinessController<LoginLog, LoginLogInfo>
    {
        public LoginLogController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "登录用户ID,登录名,真实名称,所属公司ID,所属公司名称,日志描述,IP地址,Mac地址,更新时间,系统编号";

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

            List<LoginLogInfo> list = new List<LoginLogInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                int i = 1;
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;
                    LoginLogInfo info = new LoginLogInfo();

                    info.User_ID = dr["登录用户ID"].ToString();
                    info.LoginName = dr["登录名"].ToString();
                    info.FullName = dr["真实名称"].ToString();
                    info.Company_ID = dr["所属公司ID"].ToString();
                    info.CompanyName = dr["所属公司名称"].ToString();
                    info.Note = dr["日志描述"].ToString();
                    info.IPAddress = dr["IP地址"].ToString();
                    info.MacAddress = dr["Mac地址"].ToString();
                    converted = DateTime.TryParse(dr["更新时间"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.LastUpdated = dt;
                    }
                    info.SystemType_ID = dr["系统编号"].ToString();

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
        public ActionResult SaveExcelData(List<LoginLogInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                DbTransaction trans = BLLFactory<LoginLog>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (LoginLogInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            //detail.CreateTime = DateTime.Now;
                            //detail.Creator = CurrentUser.ID.ToString();
                            //detail.Editor = CurrentUser.ID.ToString();
                            //detail.EditTime = DateTime.Now;

                            BLLFactory<LoginLog>.Instance.Insert(detail, trans);
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
            List<LoginLogInfo> list = new List<LoginLogInfo>();

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
                dr["登录用户ID"] = list[i].User_ID;
                dr["登录名"] = list[i].LoginName;
                dr["真实名称"] = list[i].FullName;
                dr["所属公司ID"] = list[i].Company_ID;
                dr["所属公司名称"] = list[i].CompanyName;
                dr["日志描述"] = list[i].Note;
                dr["IP地址"] = list[i].IPAddress;
                dr["Mac地址"] = list[i].MacAddress;
                dr["更新时间"] = list[i].LastUpdated;
                dr["系统编号"] = list[i].SystemType_ID;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/LoginLog.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(LoginLogInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(LoginLogInfo info)
        {
            //子类对参数对象进行修改
            //info.Editor = CurrentUser.ID.ToString();
            //info.EditTime = DateTime.Now;
        }
        #endregion

        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();

            List<LoginLogInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //如果需要修改字段显示，则参考下面代码处理
            //foreach(LoginLogInfo info in list)
            //{
            //    info.PID = BLLFactory<LoginLog>.Instance.GetFieldValue(info.PID, "Name");
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
        /// 生成日志左侧系统列表的树结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJsTreeJson()
        {
            List<JsTreeData> treeList = new List<JsTreeData>();

            //添加一个未分类和全部客户的组别
            JsTreeData topNode = new JsTreeData("", "所有记录", "");
            treeList.Add(topNode);

            JsTreeData companyNode = new JsTreeData("", "所属公司", "");
            treeList.Add(companyNode);

            #region 获取机构列表
            List<OUInfo> companyList = new List<OUInfo>();
            if (BLLFactory<User>.Instance.UserInRole(CurrentUser.Name, RoleInfo.SuperAdminName))
            {
                List<OUInfo> list = BLLFactory<OU>.Instance.GetMyTopGroup(CurrentUser.ID);
                foreach (OUInfo groupInfo in list)
                {
                    companyList.AddRange(BLLFactory<OU>.Instance.GetAllCompany(groupInfo.ID));
                }
            }
            else
            {
                OUInfo myCompanyInfo = BLLFactory<OU>.Instance.FindByID(CurrentUser.Company_ID);
                if (myCompanyInfo != null)
                {
                    companyList.Add(myCompanyInfo);
                }
            } 
            #endregion

            string belongCompany = "-1,";
            foreach (OUInfo info in companyList)
            {
                belongCompany += string.Format("{0},", info.ID);

                //添加公司节点
                JsTreeData subNode = new JsTreeData(info.ID, info.Name, "fa fa-sitemap icon-state-warning icon-lg");
                subNode.id = string.Format("Company_ID='{0}' ", info.ID);
                companyNode.children.Add(subNode);

                //下面在添加系统类型节点
                List<SystemTypeInfo> typeList = BLLFactory<SystemType>.Instance.GetAll();
                foreach (SystemTypeInfo typeInfo in typeList)
                {
                    JsTreeData typeNode = new JsTreeData(typeInfo.OID, typeInfo.Name, "fa fa-home icon-state-danger icon-lg");
                    typeNode.id = string.Format("Company_ID='{0}' AND SystemType_ID='{1}' ", info.ID, typeInfo.OID);
                    subNode.children.Add(typeNode);
                }

                JsTreeData securityNode = new JsTreeData("Security", "权限管理系统", "fa fa-key icon-state-info icon-lg");
                securityNode.id = string.Format("Company_ID='{0}' AND SystemType_ID='{1}' ", info.ID, "Security");
                subNode.children.Add(securityNode);
            }
            //修改全部为所属公司的ID
            belongCompany = belongCompany.Trim(',');
            topNode.id = string.Format("Company_ID in ({0})", belongCompany);

            return ToJsonContent(treeList);
        }
    }
}
