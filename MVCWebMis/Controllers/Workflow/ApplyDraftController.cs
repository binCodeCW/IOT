using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using YH.Pager.Entity;

using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using System;
using System.Dynamic;

namespace IOT.MVCWebMis.Controllers
{
    public class ApplyDraftController : BusinessController<ApplyDraft, ApplyDraftInfo>
    {
        public ApplyDraftController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "表单ID,表单分类,表单名称,草稿标题,创建人,创建时间";

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

            List<ApplyDraftInfo> list = new List<ApplyDraftInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;
                    ApplyDraftInfo info = new ApplyDraftInfo();

                    info.Form_ID = dr["表单ID"].ToString();
                    info.Category = dr["表单分类"].ToString();
                    info.FormName = dr["表单名称"].ToString();
                    info.Title = dr["草稿标题"].ToString();
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
        public ActionResult SaveExcelData(List<ApplyDraftInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<ApplyDraft>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (ApplyDraftInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            /*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<ApplyDraft>.Instance.Insert(detail, trans);
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
            List<ApplyDraftInfo> list = new List<ApplyDraftInfo>();

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
                dr["表单ID"] = list[i].Form_ID;
                dr["表单分类"] = list[i].Category;
                dr["表单名称"] = list[i].FormName;
                dr["草稿标题"] = list[i].Title;
                dr["创建人"] = list[i].Creator;
                dr["创建时间"] = list[i].CreateTime;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/ApplyDraft.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(ApplyDraftInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            info.CreateTime = DateTime.Now;
            info.Creator = CurrentUser.ID.ToString();
        }

        protected override void OnBeforeUpdate(ApplyDraftInfo info)
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
            List<ApplyDraftInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //转换ExpandoObject对象列表
            var objList = ConvertObjectList(list);

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = objList }; //如果使用转义，请使用objList对象
            return ToJsonContent(result);
        }

        /// <summary>
        /// 转换集合为ExpandoObject集合,便于Web界面显示
        /// </summary>
        /// <param name="list">对象列表</param>
        /// <returns></returns>
        private List<ExpandoObject> ConvertObjectList(List<ApplyDraftInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (ApplyDraftInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.Form_ID = info.Form_ID;
                obj.Category = info.Category;
                obj.FormName = info.FormName;
                obj.Title = info.Title;
                obj.Creator = SecurityHelper.GetFullNameByID(info.Creator);
                obj.CreateTime = info.CreateTime;
                //参考转义代码
                //obj.Name = BLLFactory<ApplyDraft>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }

        /// <summary>
        /// 保存表单数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveDraft(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                var result = new CommonResult();
                string formId = obj.formId;
                var formInfo = BLLFactory<Form>.Instance.FindByID(formId);
                if (formInfo != null)
                {
                    string title = obj.title;
                    string draftId = obj.draftId;
                    string mainJson = obj.mainJson;//主业务表单数据
                    string detailJson = obj.detailJson;//表单数据
                    string detailJson2 = obj.detailJson2;//表单数据
                    string detailJson3 = obj.detailJson3;//表单数据

                    var infoDraft = new ApplyDraftInfo();
                    if (!string.IsNullOrEmpty(draftId))
                    {
                        infoDraft.ID = draftId;//如果已有的则更新
                    }
                    infoDraft.Title = title;
                    infoDraft.BizDraftJson = mainJson;
                    infoDraft.BizDraftJson2 = detailJson;
                    infoDraft.BizDraftJson3 = detailJson2;
                    infoDraft.BizDraftJson4 = detailJson3;

                    infoDraft.Form_ID = formInfo.ID;
                    infoDraft.FormName = formInfo.FormName;
                    infoDraft.Category = formInfo.Category;
                    infoDraft.Creator = CurrentUser.ID.ToString();
                    infoDraft.CreateTime = DateTime.Now;

                    var flag = BLLFactory<ApplyDraft>.Instance.InsertUpdate(infoDraft, infoDraft.ID);
                    result.Success = flag;
                    result.ErrorMessage = "保存草稿-" + (flag ? "成功" : "失败");
                }
                else
                {
                    result.ErrorMessage = "formId 无法找到表单";
                }
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }
    }
}
        