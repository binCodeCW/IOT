using YH.Framework.ControlUtil;
using YH.Framework.Commons;

using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Dynamic;
using System.Data;

namespace IOT.MVCWebMis.Controllers
{
    public class ReimbursementDetailController : BusinessController<ReimbursementDetail, ReimbursementDetailInfo>
    {
        public ReimbursementDetailController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "申请单编号,主表单头ID,费用类型,发生时间,费用金额,费用说明,附件组别ID";

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

            List<ReimbursementDetailInfo> list = new List<ReimbursementDetailInfo>();

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
                    ReimbursementDetailInfo info = new ReimbursementDetailInfo();

                    info.Apply_ID = dr["申请单编号"].ToString();
                    info.Header_ID = dr["主表单头ID"].ToString();
                    info.FeeType = dr["费用类型"].ToString();
                    converted = DateTime.TryParse(dr["发生时间"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.OccurTime = dt;
                    }
                    info.FeeAmount = dr["费用金额"].ToString().ToDecimal();
                    info.FeeDescription = dr["费用说明"].ToString();
                    info.AttachGUID = dr["附件组别ID"].ToString();
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
        public ActionResult SaveExcelData(List<ReimbursementDetailInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<ReimbursementDetail>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (ReimbursementDetailInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            /*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<ReimbursementDetail>.Instance.Insert(detail, trans);
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
            List<ReimbursementDetailInfo> list = new List<ReimbursementDetailInfo>();

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
                dr["申请单编号"] = list[i].Apply_ID;
                dr["主表单头ID"] = list[i].Header_ID;
                dr["费用类型"] = list[i].FeeType;
                dr["发生时间"] = list[i].OccurTime;
                dr["费用金额"] = list[i].FeeAmount;
                dr["费用说明"] = list[i].FeeDescription;
                dr["附件组别ID"] = list[i].AttachGUID;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/ReimbursementDetail.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(ReimbursementDetailInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(ReimbursementDetailInfo info)
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
            List<ReimbursementDetailInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<ReimbursementDetailInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (ReimbursementDetailInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.Apply_ID = info.Apply_ID;
                obj.Header_ID = info.Header_ID;
                obj.FeeType = info.FeeType;
                obj.OccurTime = info.OccurTime;
                obj.FeeAmount = info.FeeAmount;
                obj.FeeDescription = info.FeeDescription;
                obj.AttachGUID = info.AttachGUID;
                //参考转义代码
                //obj.Name = BLLFactory<ReimbursementDetail>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }


        /// <summary>
        /// 根据主表ID获取对应明细对象信息
        /// </summary>
        /// <param name="headerId">主表ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FindByHeaderId(string headerId)
        {
            //令牌检查,不通过则抛出异常

            var list = BLLFactory<ReimbursementDetail>.Instance.FindByHeaderId(headerId);
            var objList = ConvertObjectList(list);

            var result = new { total = objList.Count, rows = objList }; //如果使用转义，请使用objList对象
            return ToJsonContent(result);
        }
    }
}
