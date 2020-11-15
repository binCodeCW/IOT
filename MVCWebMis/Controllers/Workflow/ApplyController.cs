using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using YH.Pager.Entity;
using System.Collections.Generic;
using System.Data;
using System;
using WHC.Dictionary.BLL;
using System.Dynamic;
using YH.Security.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class ApplyController : BusinessController<Apply, ApplyInfo>
    {
        public ApplyController() : base()
        {
        }

        /// <summary>
        ///列出所有申请表单
        /// </summary>
        /// <returns></returns>
        public ActionResult ListAll()
        {
            return View("ListAll");
        }


        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "表单类型ID,表单分类,申请单标题,当前状态,当前处理类型,当前处理人,最近处理时间,撤消理由,备注信息,创建人,创建日期";

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

            List<ApplyInfo> list = new List<ApplyInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;
                    ApplyInfo info = new ApplyInfo();

                    info.FormId = dr["表单类型ID"].ToString();
                    info.Category = dr["表单分类"].ToString();
                    info.Title = dr["申请单标题"].ToString();
                    info.Status = (ApplyStatus)dr["当前状态"].ToString().ToInt32();
                    info.ProcType = dr["当前处理类型"].ToString().ToInt32();
                    info.ProcUser = dr["当前处理人"].ToString();
                    info.ProcTime = dr["最近处理时间"].ToString();
                    //info.DataTable = dr["对应的数据表"].ToString();
                    info.WhyCancel = dr["撤消理由"].ToString();
                    info.Remark = dr["备注信息"].ToString();
                    converted = DateTime.TryParse(dr["创建日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.Edittime = dt;
                    }
                    info.Editor = CurrentUser.ID.ToString();
                    /*
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
        public ActionResult SaveExcelData(List<ApplyInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<Apply>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (ApplyInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            /*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<Apply>.Instance.Insert(detail, trans);
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
            List<ApplyInfo> list = new List<ApplyInfo>();

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
                dr["表单类型ID"] = list[i].FormId;
                dr["表单分类"] = list[i].Category;
                dr["申请单标题"] = list[i].Title;
                dr["当前状态"] = list[i].Status;
                dr["当前处理类型"] = list[i].ProcType;
                dr["当前处理人"] = list[i].ProcUser;
                dr["最近处理时间"] = list[i].ProcTime;
                dr["撤消理由"] = list[i].WhyCancel;
                dr["备注信息"] = list[i].Remark;
                dr["创建人"] = list[i].Editor;
                dr["创建日期"] = list[i].Edittime;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/Apply.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(ApplyInfo info)
        {
            //子类对参数对象进行修改
            info.Edittime = DateTime.Now;
            info.Editor = CurrentUser.ID.ToString();
            info.Company_ID = CurrentUser.Company_ID;
            info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(ApplyInfo info)
        {
            //子类对参数对象进行修改
            info.Editor = CurrentUser.ID.ToString();
            info.Edittime = DateTime.Now;
        }
        #endregion

        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();
            List<ApplyInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<ApplyInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (ApplyInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.Form_Id = BLLFactory<Form>.Instance.GetFormName(info.FormId);
                obj.Category = info.Category;
                obj.Title = info.Title;
                obj.Status = info.Status.ToString();
                obj.Proc_Type = BLLFactory<FormProc>.Instance.GetProcType(info.ProcType);
                obj.Proc_User = SecurityHelper.GetFullNameByID(info.ProcUser);
                obj.ProcTime = info.ProcTime;
                //obj.MustSelect = info.MustSelect;
                //obj.DataTable = info.DataTable;
                //obj.WhoInform = info.WhoInform;
                //obj.WhyCancel = info.WhyCancel;
                //obj.InformFinish = info.InformFinish;
                //obj.InformRefuse = info.InformRefuse;
                //obj.InformCancel = info.InformCancel;
                //obj.SendMail = info.SendMail;
                //obj.SendBroad = info.SendBroad;
                //obj.SendMobile = info.SendMobile;
                //obj.CanBack = info.CanBack;
                //obj.MayCancel = info.MayCancel;
                obj.Remark = info.Remark;
                obj.Editor = SecurityHelper.GetFullNameByID(info.Editor);
                obj.Edittime = info.Edittime;
                //obj.Deleted = info.Deleted;
                //参考转义代码
                //  obj.ProvinceName = BLLFactory<Province>.Instance.GetNameByID(info.ProvinceID);

                var lastCompletedFlow = BLLFactory<ApplyFlow>.Instance.GetLastCompletedFlow(info.ID);
                if(lastCompletedFlow != null)
                {
                    obj.LastReciever = SecurityHelper.GetFullNameByID(lastCompletedFlow.ProcUser);
                    obj.LastProcTime = lastCompletedFlow.ProcTime;
                }
                else
                {
                    //如果没有最后完成的流程记录，那么用创始人和时间
                    obj.LastReciever = obj.Editor;
                    obj.LastProcTime = obj.Edittime;
                }

                objList.Add(obj);
            }
            return objList;
        }

        public ActionResult FindApplyByTag()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            //解析自定义的CustomedCondition条件
            string where = GetPagerCondition();
            string condition = where;//存储另外一个条件
            //有三个条件 add: todo: done:
            if(!string.IsNullOrEmpty(where))
            {
                string value = where.Substring(where.IndexOf(':') + 1);
                if(where.StartsWith("add:"))
                {//我发起的
                    condition = string.Format("Editor='{0}'", CurrentUser.ID);
                    
                    if (!string.IsNullOrEmpty(value))
                    {
                        condition += string.Format(" AND Status={0}", value);//加上状态
                    }
                }
                else if(where.StartsWith("todo:"))
                {//我的待办
                    string applyIdString = BLLFactory<ApplyUser>.Instance.GetApplyIdByUser(CurrentUser.ID);//我的待办
                    condition = string.Format("ID IN ({0}) AND Status=0", applyIdString);
                    
                    if (!string.IsNullOrEmpty(value))
                    {
                        condition += string.Format(" AND Category='{0}'", value);//加上状态
                    }
                }
                else if(where.StartsWith("done:"))
                { //我的已办
                    string applyIdString = BLLFactory<ApplyUser>.Instance.GetApplyIdDone(CurrentUser.ID);//我的待办
                    condition = string.Format("ID IN ({0})", applyIdString);

                    if (!string.IsNullOrEmpty(value))
                    {
                        condition += string.Format(" AND Category='{0}'", value);//加上状态
                    }
                }
            }

            PagerInfo pagerInfo = GetPagerInfo();

            var sort = GetSortOrder();
            List<ApplyInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(condition, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(condition, pagerInfo);
            }

            //转换ExpandoObject对象列表
            var objList = ConvertObjectList(list);

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = objList }; //如果使用转义，请使用objList对象
            return ToJsonContent(result);
        }

        public ActionResult GetMyApplyJson()
        {
            List<JsTreeData> treeList = new List<JsTreeData>();

            //添加一个未分类和全部客户的组别
            var condition = "";
            JsTreeData addNode = new JsTreeData("add:", "我发起的", "");
            treeList.Add(addNode);
            var dict_status = EnumHelper.GetMemberKeyValue<ApplyStatus>();
            foreach (string key in dict_status.Keys)
            {
                ApplyStatus status = (ApplyStatus)dict_status[key];

                condition = string.Format("Editor='{0}' AND Status={1}", CurrentUser.ID, (int)status);
                var count = BLLFactory<Apply>.Instance.GetRecordCount(condition);

                var id = string.Format("add:{0}", (int)status);
                var name = string.Format("{0}({1})", key, count);
                var subNode = new JsTreeData(id, name, "");
                addNode.children.Add(subNode);
            }

            var dict = BLLFactory<DictData>.Instance.FindByDictType("表单类型");

            JsTreeData todoNode = new JsTreeData("todo:", "我的待办", "");
            treeList.Add(todoNode);
            foreach (var info in dict)
            {
                var applyIdString = BLLFactory<ApplyUser>.Instance.GetApplyIdByUser(CurrentUser.ID);
                condition = string.Format("Category='{0}' AND ID IN ({1})", info.Value, applyIdString);
                var count = BLLFactory<Apply>.Instance.GetRecordCount(condition);

                var id = string.Format("todo:{0}", info.Value);
                var name = string.Format("{0}({1})", info.Name, count);
                var subNode = new JsTreeData(id, name, "");
                todoNode.children.Add(subNode);
            }
            
            JsTreeData doneNode = new JsTreeData("done:", "我的已办", "");
            treeList.Add(doneNode);
            foreach (var info in dict)
            {
                var applyIdString = BLLFactory<ApplyUser>.Instance.GetApplyIdDone(CurrentUser.ID);
                condition = string.Format("Category='{0}' AND ID IN ({1})", info.Value, applyIdString);
                var count = BLLFactory<Apply>.Instance.GetRecordCount(condition);

                var id = string.Format("done:{0}", info.Value);
                var name = string.Format("{0}({1})", info.Name, count);
                var subNode = new JsTreeData(id, name, "");
                doneNode.children.Add(subNode);
            }

            return ToJsonContent(treeList);
        }

        public ActionResult GetAllApplyJson()
        {
            List<JsTreeData> treeList = new List<JsTreeData>();
            JsTreeData topNode = new JsTreeData("1=1", "所有申请单", "");
            treeList.Add(topNode);

            var dict = BLLFactory<DictData>.Instance.FindByDictType("表单类型");
            var dict_status = EnumHelper.GetMemberKeyValue<ApplyStatus>();
            var condition = "";

            //表单类型
            foreach (var info in dict)
            {
                condition = string.Format("Category='{0}'", info.Value);
                var count = BLLFactory<Apply>.Instance.GetRecordCount(condition);

                var id = condition;
                var name = string.Format("{0}({1})", info.Name, count);
                var subNode = new JsTreeData(id, name, "");
                topNode.children.Add(subNode);

                //表单状态
                foreach (string key in dict_status.Keys)
                {
                    ApplyStatus status = (ApplyStatus)dict_status[key];

                    var subCondition = string.Format("{0} AND Status={1}", condition, (int)status);
                    count = BLLFactory<Apply>.Instance.GetRecordCount(subCondition);

                    id = subCondition;
                    name = string.Format("{0}({1})", key, count);
                    var statusNode = new JsTreeData(id, name, "");
                    subNode.children.Add(statusNode);
                }
            }

            return ToJsonContent(treeList);
        }

        /// <summary>
        /// 获取申请单状态的名称
        /// </summary>
        /// <param name="status">申请状态，整形</param>
        /// <returns></returns>
        public ActionResult GetStatusName(int status)
        {
            string statusName = ((ApplyStatus)status).ToString();
            return ToJsonContent(statusName);
        }

        /// <summary>
        /// 保存申请单数据(在保存业务表单数据后，再保存该申请单信息）
        /// </summary>
        /// <param name="title">申请单标题</param>
        /// <param name="note">申请单备注</param>
        /// <param name="newApplyId">新建申请单的GUID</param>
        [HttpPost]
        public ActionResult SendApply(JObject param)
        {
            CommonResult result = new CommonResult();
            try
            {
                dynamic obj = param;
                if (obj != null)
                {
                    string applyid = obj.applyid;
                    string formid = obj.formid;
                    string title = obj.title;
                    string note = obj.note;
                    string procuserid = obj.procuserid;
                    string newapplyid = obj.newapplyid;
                    string draftid = obj.draftid;

                    if (string.IsNullOrEmpty(procuserid))
                    {
                        result.ErrorMessage = "流程处理人不能为空";
                    }
                    else
                    {
                        var loginUser = ConvertToLoginUser(CurrentUser);
                        if (!string.IsNullOrEmpty(applyid))
                        {
                            AdminApply.Instance.ReCreateApplyFlow(loginUser, applyid, title, procuserid, note);
                        }
                        else
                        {
                            //新建状态下，tempInfo.Apply_ID的初始化值为Guid.NewGuid().ToString()
                            bool success = AdminApply.Instance.CreateApply(loginUser, newapplyid, formid, title, procuserid, note);
                            if (success)
                            {
                                //删除草稿
                                if (!string.IsNullOrEmpty(draftid))
                                {
                                    BLLFactory<ApplyDraft>.Instance.Delete(draftid);
                                }
                            }
                        }

                        result.Success = true;
                    }
                }
                else
                {
                    throw new MyApiException("传递参数错误");
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);
                throw new MyApiException(ex.Message);
            }

            return ToJsonContent(result);
        }

        [HttpPost]
        public ActionResult SetStatusFinished(JObject param)
        {            
            //令牌检查,不通过则抛出异常
            
            dynamic obj = param;
            if (obj != null)
            {
                string id = obj.id;
                int userId = obj.userId;

                BLLFactory<Apply>.Instance.SetStatusFinished(id, userId);
                var result = new CommonResult(true);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        [HttpPost]
        public ActionResult DeleteApplyRelated(JObject param)
        { 
            //令牌检查,不通过则抛出异常
            
            dynamic obj = param;
            if (obj != null)
            {
                string apply_id = obj.apply_id;

                BLLFactory<Apply>.Instance.DeleteApplyRelated(apply_id);
                var result = new CommonResult(true);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        [HttpPost]
        public ActionResult IsApplyMayCancel(JObject param)
        {
            //令牌检查,不通过则抛出异常
            
            dynamic obj = param;
            if (obj != null)
            {
                string id = obj.id;
                int userId = obj.userId;

                var sucess = BLLFactory<Apply>.Instance.IsApplyMayCancel(id, userId);
                var result = new CommonResult(sucess);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        [HttpPost]
        public ActionResult IsApplyMayBackEdit(JObject param)
        {
            //令牌检查,不通过则抛出异常
            
            dynamic obj = param;
            if (obj != null)
            {
                string id = obj.id;
                int userId = obj.userId;

                var sucess = BLLFactory<Apply>.Instance.IsApplyMayBackEdit(id, userId);
                var result = new CommonResult(sucess);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        [HttpGet]        
        public ActionResult GetMyTodoCount(int userId, string formTag)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<Apply>.Instance.GetMyTodoCount(userId, formTag);
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult GetMyTodoList(int userId, string formTag)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<Apply>.Instance.GetMyTodoList(userId, formTag);
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult GetMyDoneCount(int userId, string formTag)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<Apply>.Instance.GetMyDoneCount(userId, formTag);
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult GetMyAddedCount(int userId, string formTag)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<Apply>.Instance.GetMyAddedCount(userId, formTag);
            return ToJsonContent(result);
        }

        /// <summary>
        /// 判断申请单的状态
        /// </summary>
        /// <param name="id">申请单ID</param>
        /// <returns>状态(0:处理中,1:已完成,2:已退回,3:已撤消)(其它值为非法值)</returns>
        public ActionResult GetStatus(string id)
        {
            int result = BLLFactory<Apply>.Instance.GetStatus(id);
            return ToJsonContent(result);
        }
    }
}
        