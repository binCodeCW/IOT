using System;
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
    public class WebFavoriteController : BusinessController<WebFavorite, WebFavoriteInfo>
    {
        public WebFavoriteController() : base()
        {
        }

        public ActionResult FirstView()
        {
            return View("FirstView");
        }

        public ActionResult SecondView()
        {
            return View("SecondView");
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表
        string columnString = "标题,URL地址,排序,创建人,创建时间";

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

            List<WebFavoriteInfo> list = new List<WebFavoriteInfo>();

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
                    WebFavoriteInfo info = new WebFavoriteInfo();

                    info.Title = dr["标题"].ToString();
                    info.Url = dr["URL地址"].ToString();
                    info.Seq = dr["排序"].ToString().ToDecimal();
                    info.Creator = dr["创建人"].ToString();
                    converted = DateTime.TryParse(dr["创建时间"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.CreateTime = dt;
                    }
                    /*
                            info.Creator = CurrentUser.ID.ToString();
                            info.CreateTime = DateTime.Now;
                            info.Editor = CurrentUser.ID.ToString();
                            info.EditTime = DateTime.Now;
                            */

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
        public ActionResult SaveExcelData(List<WebFavoriteInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                DbTransaction trans = BLLFactory<WebFavorite>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (WebFavoriteInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            /*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<WebFavorite>.Instance.Insert(detail, trans);
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
            List<WebFavoriteInfo> list = new List<WebFavoriteInfo>();

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
                dr["标题"] = list[i].Title;
                dr["URL地址"] = list[i].Url;
                dr["排序"] = list[i].Seq;
                dr["创建人"] = list[i].Creator;
                dr["创建时间"] = list[i].CreateTime;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/WebFavorite.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(WebFavoriteInfo info)
        {
            info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            info.CreateTime = DateTime.Now;
            info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(WebFavoriteInfo info)
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

            List<WebFavoriteInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //如果需要修改字段显示，则参考下面代码处理
            //List<ExpandoObject> objList = new List<ExpandoObject>();
            //foreach(WebFavoriteInfo info in list)
            //{
            //    dynamic obj = new ExpandoObject();
            //    obj.ID = info.ID;
            //    obj.Title = info.Title;
            //    obj.Url = info.Url;
            //    obj.Seq = info.Seq;
            //    obj.Creator = info.Creator;
            //    obj.CreateTime = info.CreateTime;
            //    参考转义代码
            //    obj.ProvinceName = BLLFactory<Province>.Instance.GetNameByID(info.ProvinceID);
            //    
            //    objList.Add(obj);
            //}

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = list /*objList*/ }; //如果使用转义，请使用objList对象
            return ToJsonContent(result);
        }

        /// <summary>
        /// 添加收藏夹记录
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="url">URL地址</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFavorite(string title, string url)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(url))
            {
                var condition = string.Format("url='{0}' and Creator='{1}' ", url, CurrentUser.ID);
                bool isExist = baseBLL.IsExistRecord(condition);
                if (!isExist)
                {
                    WebFavoriteInfo info = new WebFavoriteInfo();
                    info.Title = title;
                    info.Url = url;
                    info.Seq = DateTime.Now.DateTimeToInt();
                    info.Creator = CurrentUser.ID.ToString();

                    result.Success = baseBLL.Insert(info);
                }
                else
                {
                    result.Success = true;
                }
            }
            else
            {
                result.ErrorMessage = "URL不能为空";
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 移动记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="up">向上为true，否则为false</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpDown(string id, bool up)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    result.Success = BLLFactory<WebFavorite>.Instance.UpDown(id, up);
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.Message;
                }
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 移除单条记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveItem(string id)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(id))
            {
                var userid = CurrentUser.ID.ToString();
                result.Success = BLLFactory<WebFavorite>.Instance.RemoveItem(userid, id);
            }
            else
            {
                result.ErrorMessage = "ID为空";
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 编辑记录列表
        /// </summary>
        /// <param name="list">记录列表</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditFavorite(List<CListItem> list)
        {
            var userid = CurrentUser.ID.ToString();
            CommonResult result = BLLFactory<WebFavorite>.Instance.EditFavorite(userid, list);

            return ToJsonContent(result);
        }
    }
}
