using YH.Framework.ControlUtil;
using YH.Framework.Commons;
using YH.Pager.Entity;

using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;
using WHC.Dictionary.BLL;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Common;

namespace IOT.MVCWebMis.Controllers
{
    public class FormController : BusinessController<Form, FormInfo>
    {
        public FormController() : base()
        {
        }

        /// <summary>
        /// 列出可以创建的申请表单
        /// </summary>
        /// <returns></returns>
        public ActionResult ListCreateApply()
        {
            return View("ListCreateApply");
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "表单分类,流程模板名称,创建流程的url,显示流程处理页面url,对应的数据表,表单标识,是否禁用,备注信息,创建日期";

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

            List<FormInfo> list = new List<FormInfo>();

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
                    FormInfo info = new FormInfo();

                    info.Category = dr["表单分类"].ToString();
                    info.FormName = dr["流程模板名称"].ToString();
                    info.ApplyUrl = dr["创建流程的url"].ToString();
                    info.ApplyUrl2 = dr["显示流程处理页面url"].ToString();
                    info.DataTable = dr["对应的数据表"].ToString();
                    info.FormFlag = dr["表单标识"].ToString();
                    info.Forbid = dr["是否禁用"].ToString().ToInt32() > 0;
                    info.Remark = dr["备注信息"].ToString();
                    converted = DateTime.TryParse(dr["创建日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.Edittime = dt;
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
        public ActionResult SaveExcelData(List<FormInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                DbTransaction trans = BLLFactory<Form>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (FormInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            /*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<Form>.Instance.Insert(detail, trans);
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
            List<FormInfo> list = new List<FormInfo>();

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
                dr["表单分类"] = list[i].Category;
                dr["流程模板名称"] = list[i].FormName;
                dr["创建流程的url"] = list[i].ApplyUrl;
                dr["显示流程处理页面url"] = list[i].ApplyUrl2;
                dr["对应的数据表"] = list[i].DataTable;
                dr["表单标识"] = list[i].FormFlag;
                dr["是否禁用"] = list[i].Forbid;
                dr["备注信息"] = list[i].Remark;
                dr["创建日期"] = list[i].Edittime;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/Form.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(FormInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(FormInfo info)
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
            List<FormInfo> list = null;
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
            //foreach(FormInfo info in list)
            //{
            //    dynamic obj = new ExpandoObject();

            //    obj.ID = info.ID;
            //    obj.Category = info.Category;
            //    obj.Form_Name = info.Form_Name;
            //    obj.Apply_Url = info.Apply_Url;
            //    obj.Apply_Url2 = info.Apply_Url2;
            //    obj.Apply_Win = info.Apply_Win;
            //    obj.Apply_Win2 = info.Apply_Win2;
            //    obj.Apply_Winlist = info.Apply_Winlist;
            //    obj.Data_Table = info.Data_Table;
            //    obj.Who_Create = info.Who_Create;
            //    obj.Who_Browse = info.Who_Browse;
            //    obj.Who_Inform = info.Who_Inform;
            //    obj.May_Cancel = info.May_Cancel;
            //    obj.Inform_Finish = info.Inform_Finish;
            //    obj.Inform_Refuse = info.Inform_Refuse;
            //    obj.Inform_Cancel = info.Inform_Cancel;
            //    obj.Send_Mail = info.Send_Mail;
            //    obj.Send_Broad = info.Send_Broad;
            //    obj.Send_Mobile = info.Send_Mobile;
            //    obj.Form_Flag = info.Form_Flag;
            //    obj.Forbid = info.Forbid;
            //    obj.Remark = info.Remark;
            //    obj.Editor = info.Editor;
            //    obj.Edittime = info.Edittime;
            ////参考转义代码
            ////  obj.ProvinceName = BLLFactory<Province>.Instance.GetNameByID(info.ProvinceID);
            //    
            //    objList.Add(obj);
            //} 

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = list /*objList*/ }; //如果使用转义，请使用objList对象
            return ToJsonContent(result);
        }

        /// <summary>
        /// 获取所有表单信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFormItem()
        {
            List<CListItem> itemList = new List<CListItem>();

            List<FormInfo> list = BLLFactory<Form>.Instance.GetAll();
            foreach (FormInfo entity in list)
            {
                itemList.Add(new CListItem(entity.FormName, entity.ID.ToString()));
            }

            return ToJsonContent(itemList);
        }

        /// <summary>
        /// 获取表单分类
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFormCategory()
        {
            List<CListItem> itemList = new List<CListItem>();
            var dict = BLLFactory<DictData>.Instance.FindByDictType("表单类型");
            foreach (var info in dict)
            {
                itemList.Add(new CListItem(info.Name, info.Value));
            }

            return ToJsonContent(itemList);
        }

        /// <summary>
        /// 生成日志左侧系统列表的树结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJsTreeJson()
        {
            List<JsTreeData> treeList = new List<JsTreeData>();

            //添加一个未分类和全部客户的组别
            JsTreeData topNode = new JsTreeData("1=1", "所有记录", "");
            treeList.Add(topNode);

            JsTreeData gradeNode = new JsTreeData("", "表单类型", "");
            treeList.Add(gradeNode);

            var dict = BLLFactory<DictData>.Instance.FindByDictType("表单类型");
            foreach (var info in dict)
            {
                //添加节点
                JsTreeData subNode = new JsTreeData(info.Name, info.Name, "fa fa-sitemap icon-state-warning icon-lg");
                subNode.id = string.Format("Category='{0}' ", info.Value);
                gradeNode.children.Add(subNode);
            }

            return ToJsonContent(treeList);
        }

        [HttpGet]
        public ActionResult GetFormName(string id)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<Form>.Instance.GetFormName(id);
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult ListColumns(string tableName)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<Form>.Instance.ListColumns(tableName);
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult GetApplyCount(string datatable, string apply_id, string cond_verify)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<Form>.Instance.GetApplyCount(datatable, apply_id, cond_verify);
            return ToJsonContent(result);
        }

    }
}
        