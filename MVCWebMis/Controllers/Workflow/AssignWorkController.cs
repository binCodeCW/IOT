using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Data;
using WHC.Dictionary.BLL;
using System.Dynamic;
using YH.Pager.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class AssignWorkController : BusinessController<AssignWork, AssignWorkInfo>
    {
        public AssignWorkController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "工作类别,紧急程度,标题,内容摘要,正文,拟办意见,回复意见,回复正文,办理附件GUID,备注信息,交办单位,交办单位负责人,过期日期,附件组别ID,申请单编号,申请单日期,申请部门,创建人,创建时间,分阅人员";

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

            List<AssignWorkInfo> list = new List<AssignWorkInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;
                    AssignWorkInfo info = new AssignWorkInfo();

                    info.Category = dr["工作类别"].ToString();
                    info.Urgency = dr["紧急程度"].ToString();
                    info.Title = dr["标题"].ToString();
                    info.Abstract = dr["内容摘要"].ToString();
                    info.MainBody = dr["正文"].ToString();
                    info.InitOpinion = dr["拟办意见"].ToString();
                    info.ReplyOpinion = dr["回复意见"].ToString();
                    info.ReplyBody = dr["回复正文"].ToString();
                    info.ReplyAttachGUID = dr["办理附件GUID"].ToString();
                    info.Note = dr["备注信息"].ToString();
                    info.ToDept_ID = dr["交办单位"].ToString();
                    info.DeptManager_ID = dr["交办单位负责人"].ToString();
                    converted = DateTime.TryParse(dr["过期日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.ExpiredDate = dt;
                    }
                    info.AttachGUID = dr["附件组别ID"].ToString();
                    info.Apply_ID = dr["申请单编号"].ToString();
                    converted = DateTime.TryParse(dr["申请单日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.ApplyDate = dt;
                    }
                    info.ApplyDept = dr["申请部门"].ToString();
                    info.Creator = dr["创建人"].ToString();
                    converted = DateTime.TryParse(dr["创建时间"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.CreateTime = dt;
                    }
                    info.DispatchUsers = dr["分阅人员"].ToString();
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
        public ActionResult SaveExcelData(List<AssignWorkInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<AssignWork>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (AssignWorkInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            /*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<AssignWork>.Instance.Insert(detail, trans);
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
            List<AssignWorkInfo> list = new List<AssignWorkInfo>();

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
                dr["工作类别"] = list[i].Category;
                dr["紧急程度"] = list[i].Urgency;
                dr["标题"] = list[i].Title;
                dr["内容摘要"] = list[i].Abstract;
                dr["正文"] = list[i].MainBody;
                dr["拟办意见"] = list[i].InitOpinion;
                dr["回复意见"] = list[i].ReplyOpinion;
                dr["回复正文"] = list[i].ReplyBody;
                dr["办理附件GUID"] = list[i].ReplyAttachGUID;
                dr["备注信息"] = list[i].Note;
                dr["交办单位"] = list[i].ToDept_ID;
                dr["交办单位负责人"] = list[i].DeptManager_ID;
                dr["过期日期"] = list[i].ExpiredDate;
                dr["附件组别ID"] = list[i].AttachGUID;
                dr["申请单编号"] = list[i].Apply_ID;
                dr["申请单日期"] = list[i].ApplyDate;
                dr["申请部门"] = list[i].ApplyDept;
                dr["创建人"] = list[i].Creator;
                dr["创建时间"] = list[i].CreateTime;
                dr["分阅人员"] = list[i].DispatchUsers;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/AssignWork.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(AssignWorkInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            info.CreateTime = DateTime.Now;
            info.Creator = CurrentUser.ID.ToString();
            info.ApplyDept = CurrentUser.Dept_ID;
            info.ApplyDate = DateTime.Now;
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(AssignWorkInfo info)
        {
            //子类对参数对象进行修改
            info.Editor = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;
        }
        #endregion

        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();
            List<AssignWorkInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<AssignWorkInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (AssignWorkInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.Category = info.Category;
                obj.Urgency = info.Urgency;
                obj.Title = info.Title;
                obj.Abstract = info.Abstract;
                obj.MainBody = info.MainBody;
                obj.InitOpinion = info.InitOpinion;
                obj.ReplyOpinion = info.ReplyOpinion;
                obj.ReplyBody = info.ReplyBody;
                obj.ReplyAttachGUID = info.ReplyAttachGUID;
                obj.Note = info.Note;
                obj.ToDept_ID = info.ToDept_ID;
                obj.DeptManager_ID = info.DeptManager_ID;
                obj.ExpiredDate = info.ExpiredDate;
                obj.AttachGUID = info.AttachGUID;
                obj.Apply_ID = info.Apply_ID;
                obj.ApplyDate = info.ApplyDate;
                obj.ApplyDept = SecurityHelper.GetDeptNameByID(info.ApplyDept);
                obj.Creator = SecurityHelper.GetFullNameByID(info.Creator);
                obj.CreateTime = info.CreateTime;
                obj.Editor  = SecurityHelper.GetFullNameByID(info.Editor);
                obj.EditTime = info.EditTime;
                obj.DispatchUsers = info.DispatchUsers;
                //申请单状态
                var status = BLLFactory<Apply>.Instance.GetFieldValue(info.Apply_ID, "Status");
                obj.Status = ((ApplyStatus)status.ToInt32()).ToString();
                //参考转义代码
                //obj.Name = BLLFactory<AssignWork>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }

        /// <summary>
        /// 根据申请单ID获取对应对象信息
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FindByApplyId(string applyId)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<AssignWork>.Instance.FindByApplyId(applyId);
            return ToJsonContent(result);
        }

        /// <summary>
        /// 删除多个ID的记录
        /// </summary>
        /// <param name="ids">多个id组合，逗号分开（1,2,3,4,5）</param>
        /// <returns></returns>
        public override ActionResult DeleteByIds(string ids)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.DeleteKey);

            CommonResult result = new CommonResult();
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    List<string> idArray = ids.ToDelimitedList<string>(",");
                    foreach (string strId in idArray)
                    {
                        if (!string.IsNullOrEmpty(strId))
                        {
                            var info = BLLFactory<AssignWork>.Instance.FindByID(strId);
                            if (info != null)
                            {
                                //删除关联表单数据，包括业务表单数据
                                BLLFactory<Apply>.Instance.DeleteApplyRelated(info.Apply_ID);
                            }
                            //baseBLL.Delete(strId);
                        }
                    }
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }
    }
}
        