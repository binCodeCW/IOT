using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System;
using System.Data;
using System.Collections.Generic;
using System.Dynamic;

namespace IOT.MVCWebMis.Controllers
{
    public class FormFlowController : BusinessController<FormFlow, FormFlowInfo>
    {
        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "流程模板ID,流程环节名称,步骤名称,对应的条件,流程处理人,该流程对应的处理人,是否可以选择该流程的具体处理人,流程处理人是否可以增加新的流程,该流程的处理人是否可以发送通知,在该流程环节处理人是否可以回退,是否可以回退到该流程环节,是否支持短信审批,备注信息,模板流程顺序";

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

            List<FormFlowInfo> list = new List<FormFlowInfo>();

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
                    FormFlowInfo info = new FormFlowInfo();

                    info.FormId = dr["流程模板ID"].ToString();
                    info.ProcType = dr["流程环节名称"].ToString().ToInt32();
                    info.FlowName = dr["步骤名称"].ToString();
                    info.CondVerify = dr["对应的条件"].ToString();
                    info.CondUser = dr["流程处理人"].ToString();
                    info.ProcUser = dr["该流程对应的处理人"].ToString();
                    info.MaySelproc = dr["是否可以选择该流程的具体处理人"].ToString().ToInt32();
                    info.MayAddFlow = dr["流程处理人是否可以增加新的流程"].ToString().ToInt32();
                    info.MayMsgSend = dr["该流程的处理人是否可以发送通知"].ToString().ToInt32();
                    info.CanBack = dr["在该流程环节处理人是否可以回退"].ToString().ToInt32();
                    info.CanBeBack = dr["是否可以回退到该流程环节"].ToString().ToInt32();
                    info.CanSms = dr["是否支持短信审批"].ToString().ToInt32();
                    info.Remark = dr["备注信息"].ToString();
                    info.Orderbyid = dr["模板流程顺序"].ToString().ToDecimal();
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
        public ActionResult SaveExcelData(List<FormFlowInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<FormFlow>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (FormFlowInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            /*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<FormFlow>.Instance.Insert(detail, trans);
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
            List<FormFlowInfo> list = new List<FormFlowInfo>();

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
                dr["流程模板ID"] = list[i].FormId;
                dr["流程环节名称"] = list[i].ProcType;
                dr["步骤名称"] = list[i].FlowName;
                dr["对应的条件"] = list[i].CondVerify;
                dr["流程处理人"] = list[i].CondUser;
                dr["该流程对应的处理人"] = list[i].ProcUser;
                dr["是否可以选择该流程的具体处理人"] = list[i].MaySelproc;
                dr["流程处理人是否可以增加新的流程"] = list[i].MayAddFlow;
                dr["该流程的处理人是否可以发送通知"] = list[i].MayMsgSend;
                dr["在该流程环节处理人是否可以回退"] = list[i].CanBack;
                dr["是否可以回退到该流程环节"] = list[i].CanBeBack;
                dr["是否支持短信审批"] = list[i].CanSms;
                dr["备注信息"] = list[i].Remark;
                dr["模板流程顺序"] = list[i].Orderbyid;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/FormFlow.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(FormFlowInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(FormFlowInfo info)
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
            List<FormFlowInfo> list = null;
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
            foreach (FormFlowInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.FormId = info.FormId;
                obj.ProcType = info.ProcType;
                obj.FlowName = info.FlowName;
                obj.CondVerify = info.CondVerify;
                obj.CondUser = info.CondUser;
                obj.ProcUser = info.ProcUser;
                obj.MaySelproc = info.MaySelproc;
                obj.MayAddFlow = info.MayAddFlow;
                obj.MayMsgSend = info.MayMsgSend;
                obj.CanBack = info.CanBack;
                obj.CanBeBack = info.CanBeBack;
                obj.CanSms = info.CanSms;
                obj.Remark = info.Remark;
                obj.Orderbyid = info.Orderbyid;
                //转义代码
                obj.ProcTypeName = BLLFactory<FormProc>.Instance.GetProcType(info.ProcType);

                objList.Add(obj);
            }

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = objList }; //如果使用转义，请使用objList对象
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
                    result.Success = BLLFactory<FormFlow>.Instance.UpDown(id, up);
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
                result.Success = BLLFactory<FormFlow>.Instance.Delete(id);
            }
            else
            {
                result.ErrorMessage = "ID为空";
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 编辑表单的流程顺序
        /// </summary>
        /// <param name="id">表单ID</param>
        /// <param name="list">按照顺序的流程步骤ID集合</param>
        /// <returns></returns>
        public ActionResult EditFormFlow(string form_id, List<string> list)
        {
            CommonResult result = new CommonResult();
            if (list != null)
            { 
                int i = 1;
                foreach (var item in list)
                {
                    var info = BLLFactory<FormFlow>.Instance.FindByID(item);
                    if (info != null)
                    {
                        info.Orderbyid = i++;
                        BLLFactory<FormFlow>.Instance.Update(info, info.ID);
                    }

                    result.Success = true;
                }
            }
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult GetFormFlow(string form_id)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<FormFlow>.Instance.GetFormFlow(form_id);
            return ToJsonContent(result);
        }
        [HttpGet]
        public ActionResult GetFirstFormFlow(string form_id)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<FormFlow>.Instance.GetFirstFormFlow(form_id);
            return ToJsonContent(result);
        }

        [HttpPost]
        public ActionResult InsertAppFlow(JObject param)
        {
            //令牌检查,不通过则抛出异常

            dynamic obj = param;
            if (obj != null)
            {
                FormFlowInfo appFlowInfo = obj.appFlowInfo;

                bool success = BLLFactory<FormFlow>.Instance.InsertAppFlow(appFlowInfo);
                var result = new CommonResult(success);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }
        [HttpPost]
        public ActionResult UpdateAppFlow(JObject param)
        {
            //令牌检查,不通过则抛出异常

            dynamic obj = param;
            if (obj != null)
            {
                FormFlowInfo appFlowInfo = obj.appFlowInfo;
                bool isUp = obj.isUp;

                bool success = BLLFactory<FormFlow>.Instance.UpdateAppFlow(appFlowInfo, isUp);
                var result = new CommonResult(success);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        [HttpGet]
        public ActionResult MaySelectProcUser(string flowId)
        {
            //令牌检查,不通过则抛出异常

            bool success = BLLFactory<FormFlow>.Instance.MaySelectProcUser(flowId);
            var result = new CommonResult(success);
            return ToJsonContent(result);

        }

        [HttpPost]
        public ActionResult UpdateTwoSeq(JObject param)
        {
            //令牌检查,不通过则抛出异常

            dynamic obj = param;
            if (obj != null)
            {
                string sourceId = obj.sourceId;
                string targetId = obj.targetId;

                bool success = BLLFactory<FormFlow>.Instance.UpdateTwoSeq(sourceId, targetId);
                var result = new CommonResult(success);
                return ToJsonContent(result);

            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        [HttpGet]
        public ActionResult GetFlowUserJson(string formId, int step)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<FormFlow>.Instance.GetFlowUserJson(formId, step);
            return ToJsonContent(result);
        }
    }
}
        