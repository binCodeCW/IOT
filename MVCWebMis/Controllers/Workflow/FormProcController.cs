using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using WHC.Dictionary.BLL;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class FormProcController : BusinessController<FormProc, FormProcInfo>
    {
        public FormProcController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "流程环节名称,处理类型,对应表单,是否禁用,备注信息,创建日期,是否标记删除";

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

            List<FormProcInfo> list = new List<FormProcInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;
                    FormProcInfo info = new FormProcInfo();

                    info.ProcName = dr["流程环节名称"].ToString();
                    info.ProcType = dr["处理类型"].ToString().ToInt32();
                    info.FormId = dr["对应表单"].ToString().ToInt32();
                    info.Forbid = dr["是否禁用"].ToString().ToInt32();
                    info.Remark = dr["备注信息"].ToString();
                    converted = DateTime.TryParse(dr["创建日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.Edittime = dt;
                    }
                    info.Deleted = dr["是否标记删除"].ToString().ToInt32();
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
        public ActionResult SaveExcelData(List<FormProcInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<FormProc>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (FormProcInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            /*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<FormProc>.Instance.Insert(detail, trans);
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
            List<FormProcInfo> list = new List<FormProcInfo>();

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
                dr["流程环节名称"] = list[i].ProcName;
                dr["处理类型"] = list[i].ProcType;
                dr["对应表单"] = list[i].FormId;
                dr["是否禁用"] = list[i].Forbid;
                dr["备注信息"] = list[i].Remark;
                dr["创建日期"] = list[i].Edittime;
                dr["是否标记删除"] = list[i].Deleted;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/FormProc.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(FormProcInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(FormProcInfo info)
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
            var pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();
            List<FormProcInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (FormProcInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.ProcName = info.ProcName;
                obj.ProcType = info.ProcType;
                obj.Form_ID = info.FormId;
                obj.Forbid = info.Forbid;
                obj.Remark = info.Remark;
                obj.Editor = info.Editor;
                obj.Edittime = info.Edittime;
                obj.Deleted = info.Deleted;
                //参考转义代码
                obj.ProcTypeName = BLLFactory<DictData>.Instance.GetDictName("处理类型", string.Concat(info.ProcType));

                string formName = BLLFactory<Form>.Instance.GetFormName(info.FormId.ToString());//对应表单的描述
                obj.FormIdName = !string.IsNullOrEmpty(formName) ? formName : "所有表单";

                obj.EditorName = BLLFactory<YH.Security.BLL.User>.Instance.GetNameByID(info.Editor.ToInt32());

                objList.Add(obj);
            }

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = objList }; //如果使用转义，请使用objList对象
            return ToJsonContent(result);
        }

        /// <summary>
        /// 获取处理类型,用于字典绑定
        /// </summary>
        /// <returns>ListItem</returns>
        public ActionResult GetProcTypeItem(bool isAdd = false)
        {
            Dictionary<string, int> procUsedList = BLLFactory<FormProc>.Instance.GetAllProcType();

            List<CListItem> itemList = new List<CListItem>();
            Dictionary<string, string> dict = BLLFactory<DictData>.Instance.GetDictByDictType("处理类型");
            foreach (string key in dict.Keys)
            {
                itemList.Add(new CListItem(key, dict[key]));
            }
            
            List<CListItem> resultList = new List<CListItem>();
            if (itemList != null)
            {
                foreach (CListItem item in itemList)
                {
                    if (string.IsNullOrEmpty(item.Value) || !isAdd || !procUsedList.ContainsKey(item.Value))
                    {
                        resultList.Add(item);
                    }
                }
            }

            return ToJsonContent(resultList);
        }

        /// <summary>
        /// 获取处理类型CListItem
        /// </summary>
        /// <param name="form_id">表单id</param>
        /// <returns></returns>
        public ActionResult GetProcTypeItemByForm(string form_id)
        {
            string condition = string.Format(" form_id = '{0}' or form_id='0' ", form_id);
            List<FormProcInfo> list = BLLFactory<FormProc>.Instance.Find(condition);
            List<CListItem> itemList = new List<CListItem>();

            foreach (FormProcInfo entity in list)
            {
                itemList.Add(new CListItem(entity.ProcName, entity.ProcType.ToString()));
            }

            return ToJsonContent(itemList);
        }

        [HttpGet]
        public ActionResult GetProcType(int typeId)
        {
            //令牌检查,不通过则抛出异常
            
            var result = BLLFactory<FormProc>.Instance.GetProcType(typeId);
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult GetManageProcSql(string token)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<FormProc>.Instance.GetManageProcSql();
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult GetProcessFlowName(string applyId)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<FormProc>.Instance.GetProcessFlowName(applyId);
            return ToJsonContent(result);

        }

        [HttpGet]
        public ActionResult GetAllProcType(string token)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<FormProc>.Instance.GetAllProcType();
            return ToJsonContent(result);

        }
    }
}
        