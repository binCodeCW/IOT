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
    public class ApplyFlowlogController : BusinessController<ApplyFlowlog, ApplyFlowlogInfo>
    {
        public ApplyFlowlogController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "流程申请单ID,流程ID,顺序,流程环节名称,步骤名称,该流程对应的处理人,实际处理时间,处理意见,下一流程名称,下一处理人,开始时间";

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

            List<ApplyFlowlogInfo> list = new List<ApplyFlowlogInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    //流程申请单ID,流程ID,顺序,流程环节名称,步骤名称,该流程对应的处理人,实际处理时间,处理意见,下一流程名称,下一处理人,开始时间
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;
                    ApplyFlowlogInfo info = new ApplyFlowlogInfo();

                    info.ApplyId = dr["流程申请单ID"].ToString();
                    info.FlowId = dr["流程ID"].ToString();
                    info.OrderNo = dr["顺序"].ToString().ToInt32();
                    info.ProcType = dr["流程环节名称"].ToString().ToInt32();
                    info.FlowName = dr["步骤名称"].ToString();
                    info.ProcUser = dr["该流程对应的处理人"].ToString();
                    converted = DateTime.TryParse(dr["实际处理时间"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.ProcTime = dt;
                    }
                    info.Opinion = dr["处理意见"].ToString();
                    info.NextFlowName = dr["下一流程名称"].ToString();
                    info.NextProcUser = dr["下一处理人"].ToString();
                    converted = DateTime.TryParse(dr["开始时间"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.Begtime = dt;
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
        public ActionResult SaveExcelData(List<ApplyFlowlogInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<ApplyFlowlog>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (ApplyFlowlogInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            /*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<ApplyFlowlog>.Instance.Insert(detail, trans);
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
            List<ApplyFlowlogInfo> list = new List<ApplyFlowlogInfo>();

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
                //流程申请单ID,流程ID,顺序,流程环节名称,步骤名称,该流程对应的处理人,实际处理时间,处理意见,下一流程名称,下一处理人,开始时间
                dr = datatable.NewRow();
                dr["序号"] = j++;
                dr["流程申请单ID"] = list[i].ApplyId;
                dr["流程ID"] = list[i].FlowId;
                dr["顺序"] = list[i].OrderNo;
                dr["流程环节名称"] = list[i].ProcType;
                dr["步骤名称"] = list[i].FlowName;
                dr["该流程对应的处理人"] = list[i].ProcUser;
                dr["实际处理时间"] = list[i].ProcTime;
                dr["处理意见"] = list[i].Opinion;
                dr["下一流程名称"] = list[i].NextFlowName;
                dr["下一处理人"] = list[i].NextProcUser;
                dr["开始时间"] = list[i].Begtime;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/ApplyFlowlog.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(ApplyFlowlogInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(ApplyFlowlogInfo info)
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
            List<ApplyFlowlogInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //转换ExpandoObject对象列表
            //var objList = ConvertObjectList(list);

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = list /*objList*/ }; //如果使用转义，请使用objList对象
            return ToJsonContent(result);
        }

        /// <summary>
        /// 转换集合为ExpandoObject集合,便于Web界面显示
        /// </summary>
        /// <param name="list">对象列表</param>
        /// <returns></returns>
        private List<ExpandoObject> ConvertObjectList(List<ApplyFlowlogInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (ApplyFlowlogInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.ApplyId = info.ApplyId;
                obj.FlowId = info.FlowId;
                obj.OrderNo = info.OrderNo;
                obj.ProcType = BLLFactory<FormProc>.Instance.GetProcType(info.ProcType);
                obj.FlowName = info.FlowName;
                obj.ProcUser = SecurityHelper.GetFullNameByID(info.ProcUser);//处理人
                obj.ProcTime = info.ProcTime;
                obj.Opinion = info.Opinion;
                obj.NextFlowName = info.NextFlowName;
                obj.NextProcUser = info.NextProcUser;
                obj.Begtime = info.Begtime;

                //参考转义代码
                //obj.Name = BLLFactory<ApplyFlowlog>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }

        [HttpGet]        
        public ActionResult GetAllByApplyId(string applyId)
        {
            //令牌检查,不通过则抛出异常

            var list = BLLFactory<ApplyFlowlog>.Instance.GetAllByApplyId(applyId);
            var objList = ConvertObjectList(list);

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = list.Count, rows = objList };
            return ToJsonContent(result);
        }

        [HttpGet]        
        public ActionResult GetFlowLog(string applyId, string flowName)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<ApplyFlowlog>.Instance.GetFlowLog(applyId, flowName);
            return ToJsonContent(result);
        }

        [HttpPost]
        public ActionResult DeleteByFlowId(JObject param)
        {
            //令牌检查,不通过则抛出异常
            
            dynamic obj = param;
            if (obj != null)
            {
                string applyId = obj.applyId;
                string flowId = obj.flowId;

                bool sucess = BLLFactory<ApplyFlowlog>.Instance.DeleteByFlowId(applyId, flowId);
                var result = new CommonResult(sucess);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }


    }
}
        