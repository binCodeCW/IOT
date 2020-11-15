using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using YH.Security.BLL;

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

namespace IOT.MVCWebMis.Controllers
{
    public class ApplyFlowController : BusinessController<ApplyFlow, ApplyFlowInfo>
    {
        public ApplyFlowController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "流程申请单ID,顺序,流程环节名称,步骤名称,对应的条件,该流程对应的处理人,是否处理,实际处理人ID,实际处理时间,处理意见";

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

            List<ApplyFlowInfo> list = new List<ApplyFlowInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    ApplyFlowInfo info = new ApplyFlowInfo();

                    info.ApplyId = dr["流程申请单ID"].ToString();
                    info.OrderNo = dr["顺序"].ToString().ToInt32();
                    info.ProcType = dr["流程环节名称"].ToString().ToInt32();
                    info.FlowName = dr["步骤名称"].ToString();
                    info.CondVerify = dr["对应的条件"].ToString();
                    info.ProcUser = dr["该流程对应的处理人"].ToString();
                    info.IsProc = dr["是否处理"].ToString().ToInt32();
                    info.ProcUid = dr["实际处理人ID"].ToString().ToInt32();
                    info.ProcTime = dr["实际处理时间"].ToString();
                    info.Opinion = dr["处理意见"].ToString();
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
        public ActionResult SaveExcelData(List<ApplyFlowInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<ApplyFlow>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (ApplyFlowInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            /*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<ApplyFlow>.Instance.Insert(detail, trans);
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
            List<ApplyFlowInfo> list = new List<ApplyFlowInfo>();

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
                //流程申请单ID,顺序,流程环节名称,步骤名称,对应的条件,该流程对应的处理人,是否处理,实际处理人ID,实际处理时间,处理意见
                dr = datatable.NewRow();
                dr["序号"] = j++;
                dr["流程申请单ID"] = list[i].ApplyId;
                dr["顺序"] = list[i].OrderNo;
                dr["流程环节名称"] = list[i].ProcType;
                dr["步骤名称"] = list[i].FlowName;
                dr["对应的条件"] = list[i].CondVerify;
                dr["该流程对应的处理人"] = list[i].ProcUser;
                dr["是否处理"] = list[i].IsProc;
                dr["实际处理人ID"] = list[i].ProcUid;
                dr["实际处理时间"] = list[i].ProcTime;
                dr["处理意见"] = list[i].Opinion;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/ApplyFlow.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(ApplyFlowInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(ApplyFlowInfo info)
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
            List<ApplyFlowInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<ApplyFlowInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (ApplyFlowInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.ApplyId = info.ApplyId;
                obj.OrderNo = info.OrderNo;
                obj.ProcType = BLLFactory<FormProc>.Instance.GetProcType(info.ProcType);
                obj.FlowName = info.FlowName;//流程名称
                obj.ProcUser = SecurityHelper.GetFullNameByID(info.ProcUser);//处理人
                obj.IsProc = ((ApplyIsProc)info.IsProc).ToString();//是否处理
                obj.ProcUid = SecurityHelper.GetFullNameByID(info.ProcUid.ToString());//实际处理人
                obj.ProcTime = info.ProcTime;//处理时间
                obj.Opinion = info.Opinion;//意见
                obj.Deltatime = info.Deltatime;

                //参考转义代码
                //obj.Name = BLLFactory<ApplyFlow>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }


        [HttpPost]
        public ActionResult AddAppFlowByPrevious(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                string applyId = obj.applyId;
                string selprocuser = obj.selprocuser;

                bool sucess = BLLFactory<ApplyFlow>.Instance.AddAppFlowByPrevious(applyId, selprocuser);
                var result = new CommonResult(sucess);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        [HttpPost]
        public ActionResult AddReadAppFlow(JObject param)
        {           

            dynamic obj = param;
            if (obj != null)
            {
                string applyId = obj.applyId;
                string selprocuser = obj.selprocuser;

                bool sucess = BLLFactory<ApplyFlow>.Instance.AddReadAppFlow(applyId, selprocuser);
                var result = new CommonResult(sucess);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        [HttpGet]
        public ActionResult GetLastCompletedFlow(string applyId)
        {     
            var result = BLLFactory<ApplyFlow>.Instance.GetLastCompletedFlow(applyId);
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult GetFirstUnHandledFlow(string applyId)
        {
            var result = BLLFactory<ApplyFlow>.Instance.GetFirstUnHandledFlow(applyId);
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult GetNextUnHandledFlow(string applyId, string flowId)
        {    
            var result = BLLFactory<ApplyFlow>.Instance.GetNextUnHandledFlow(applyId, flowId);
            return ToJsonContent(result);
        }

        [HttpPost]
        public ActionResult HandleFlowWithOpinion(JObject param)
        {            
            dynamic obj = param;
            if (obj != null)
            {
                string id = obj.id;
                string apply_id = obj.apply_id;
                string opinion = obj.opinion;

                bool sucess = BLLFactory<ApplyFlow>.Instance.HandleFlowWithOpinion(id, apply_id, opinion);
                var result = new CommonResult(sucess);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }


        [HttpPost]
        public ActionResult DeleteAllFlow(JObject param)
        {           
            dynamic obj = param;
            if (obj != null)
            {
                string applyId = obj.applyId;

                bool sucess = BLLFactory<ApplyFlow>.Instance.DeleteAllFlow(applyId);
                var result = new CommonResult(sucess);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        [HttpPost]
        public ActionResult ResetFlowInfo(JObject param)
        {            

            dynamic obj = param;
            if (obj != null)
            {
                ApplyFlowInfo flowInfo = obj.flowInfo;

                bool sucess = BLLFactory<ApplyFlow>.Instance.ResetFlowInfo(flowInfo);
                var result = new CommonResult(sucess);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 获取对应表单下的所有流程，根据order_no由小到大排序
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllByApplyId(string applyId)
        {
            var list = BLLFactory<ApplyFlow>.Instance.GetAllByApplyId(applyId);
            var objList = ConvertObjectList(list);

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = list.Count, rows = objList };
            return ToJsonContent(result);
        }
        
        /// <summary>
        /// 获取流程的所有审批和阅办信息
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetOpinionByApplyId(string applyId)
        { 
           var list = BLLFactory<ApplyFlow>.Instance.GetAllByApplyId(applyId);

            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (ApplyFlowInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.FlowName = info.FlowName;//流程名称
                var opinions = GetApplyFlowOpinions(applyId, info.FlowName);
                obj.Opinions = opinions;

                objList.Add(obj);
            }

            //增加阅办步骤
            {
                var opinions = GetReadFlowOpinions(applyId);
                if (opinions.Count > 0)
                {
                    dynamic obj = new ExpandoObject();
                    obj.FlowName = "阅办";//流程名称
                    obj.Opinions = opinions;

                    objList.Add(obj);
                }
            }

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = list.Count, rows = objList };
            return ToJsonContent(result);
        }
        /// <summary>
        /// 根据申请单ID和流程名称，获取审批等流程处理记录
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="flowName">流程名称</param>
        /// <returns></returns>
        private List<OpinionInfo> GetApplyFlowOpinions(string applyId, string flowName)
        {
            var opitionList = new List<OpinionInfo>();

            #region 流程处理意见

            var flowLogList = BLLFactory<ApplyFlowlog>.Instance.GetFlowLog(applyId, flowName);
            foreach (var logInfo in flowLogList)
            {
                string userFullName = BLLFactory<User>.Instance.GetFullNameByID(logInfo.ProcUser.ToInt32());
                var info = new OpinionInfo(logInfo.Opinion, userFullName, logInfo.ProcTime);
                opitionList.Add(info);
            }

            #endregion

            return opitionList;
        }

        /// <summary>
        /// 获取阅办的审批意见
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        private List<OpinionInfo> GetReadFlowOpinions(string applyId)
        {
            var opitionList = new List<OpinionInfo>();

            string flowName = "阅办";
            var readInfoList = BLLFactory<ApplyRead>.Instance.FindByApplyId(applyId);
            foreach (var readInfo in readInfoList)
            {
                string userFullName = BLLFactory<User>.Instance.GetFullNameByID(readInfo.UserId);
                var info = new OpinionInfo(readInfo.Content, userFullName, readInfo.ReadTime);
                opitionList.Add(info);
            }

            return opitionList;
        }

        /// <summary>
        /// 处理意见信息
        /// </summary>
        public class OpinionInfo
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            public OpinionInfo(string opinion, string username, DateTime procTime)
            {
                this.Opinion = opinion;
                this.UserName = username;
                this.ProcTime = procTime;
            }
            public string Opinion { get; set; }
            public string UserName { get; set; }
            public DateTime ProcTime { get; set; }
        }
    }
}
