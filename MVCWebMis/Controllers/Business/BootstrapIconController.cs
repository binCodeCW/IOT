﻿using System;
using System.Data;
using System.Data.Common;
using System.Web.Mvc;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.BLL;
using IOT.MVCWebMis.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class BootstrapIconController : BusinessController<BootstrapIcon, BootstrapIconInfo>
    {
        public BootstrapIconController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "显示名称,样式名称,来源,创建时间";

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

            List<BootstrapIconInfo> list = new List<BootstrapIconInfo>();

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
                    BootstrapIconInfo info = new BootstrapIconInfo();

                    info.DisplayName = dr["显示名称"].ToString();
                    info.ClassName = dr["样式名称"].ToString();
                    info.SourceType = dr["来源"].ToString();
                    converted = DateTime.TryParse(dr["创建时间"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.CreateTime = dt;
                    }

                    info.CreateTime = DateTime.Now;

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
        public ActionResult SaveExcelData(List<BootstrapIconInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                DbTransaction trans = BLLFactory<BootstrapIcon>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (BootstrapIconInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            detail.CreateTime = DateTime.Now;

                            BLLFactory<BootstrapIcon>.Instance.Insert(detail, trans);
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
                    finally
                    {
                        trans.Connection.Close();
                        trans.Connection.Dispose();
                        trans.Dispose();
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
            List<BootstrapIconInfo> list = new List<BootstrapIconInfo>();

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
                dr["显示名称"] = list[i].DisplayName;
                dr["样式名称"] = list[i].ClassName;
                dr["来源"] = list[i].SourceType;
                dr["创建时间"] = list[i].CreateTime;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/BootstrapIcon.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(BootstrapIconInfo info)
        {
            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(BootstrapIconInfo info)
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

            List<BootstrapIconInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //如果需要修改字段显示，则参考下面代码处理
            //foreach(BootstrapIconInfo info in list)
            //{
            //    info.PID = BLLFactory<BootstrapIcon>.Instance.GetFieldValue(info.PID, "Name");
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
        /// 生成图标文件
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateIconCSS()
        {
            CommonResult result = new CommonResult();
            string regex = "^\\.(?<name>.*?):before\\s*\\{";
            List<string> filePathList = new List<string>()
            {
                "~/Content/metronic/assets/global/plugins/bootstrap/css/bootstrap.css",
                "~/Content/metronic/assets/global/plugins/font-awesome/css/font-awesome.css",
                "~/Content/metronic/assets/global/plugins/simple-line-icons/simple-line-icons.css",
            };
            //图标类型
            List<string> sourceTypeList = new List<string>()
            {
                "Glyphicons",
                "FontAwesome",
                "SimpleLine",
            };
            List<string> prefixList = new List<string>()
            {
                "glyphicon ",
                "fa ",
                "",
            };

            //对每个文件进行匹配并存储
            int i = 0;
            try
            {
                foreach (string filePath in filePathList)
                {
                    string realPath = Server.MapPath(filePath);
                    if (FileUtil.FileIsExist(realPath))
                    {
                        string sourceType = sourceTypeList[i];
                        BLLFactory<BootstrapIcon>.Instance.DeleteBySourceType(sourceType);

                        string prefix = prefixList[i];
                        string content = FileUtil.FileToString(realPath);
                        List<string> matchList = CRegex.GetList(content, regex, 1);
                        foreach (string item in matchList)
                        {
                            //包含特殊的字符，去掉即可
                            if (item.IndexOfAny(new char[] { '.', '>', '+' }) > 0)
                                continue;

                            BootstrapIconInfo info = new BootstrapIconInfo()
                            {
                                DisplayName = item,
                                ClassName = prefix + item,
                                CreateTime = DateTime.Now,
                                SourceType = sourceType,
                            };

                            BLLFactory<BootstrapIcon>.Instance.Insert(info);
                        }
                        result.Success = true;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                LogTextHelper.Error(ex);
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 获取图标的类型列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSourceTypeJsTree()
        {
            List<JsTreeData> list = new List<JsTreeData>();
            List<string> typeList = baseBLL.GetFieldList("SourceType");
            foreach (string type in typeList)
            {
                list.Add(new JsTreeData(type, type, "fa fa-file icon-state-warning icon-lg"));
            }
            return ToJsonContent(list);
        }
    }
}
