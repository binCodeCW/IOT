using System;
using System.Data;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.Entity;
using Newtonsoft.Json.Linq;
using IOT.MVCWebMis.Common;
using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class AssetCzController : BusinessController<AssetCz, AssetCzInfo>
    {
        public AssetCzController() : base()
        {
        }

        #region 导入Excel数据操作
 		 		 		 		 		 		 		 		 		 		 		   
	    //导入或导出的字段列表   
        string columnString = "处置单号,处置资产,资产管理部门（单位）,部门资产管理员,申请单编号,申请单日期,申请部门,备注信息,附件组别ID,创建人,创建时间";

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

            List<AssetCzInfo> list = new List<AssetCzInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;                    
                    AssetCzInfo info = new AssetCzInfo();
                    
                     info.BillNo = dr["处置单号"].ToString();
                      info.AssetDesc = dr["处置资产"].ToString();
                      info.ChargeDept = dr["资产管理部门（单位）"].ToString();
                      info.DeptAdmin = dr["部门资产管理员"].ToString();
                      info.Apply_ID = dr["申请单编号"].ToString();
                      converted = DateTime.TryParse(dr["申请单日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                         info.ApplyDate = dt;
                    }
                      info.ApplyDept = dr["申请部门"].ToString();
                      info.Note = dr["备注信息"].ToString();
                      info.AttachGUID = dr["附件组别ID"].ToString();
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
        public ActionResult SaveExcelData(List<AssetCzInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<AssetCz>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (AssetCzInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
							/*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<AssetCz>.Instance.Insert(detail, trans);
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
            List<AssetCzInfo> list = new List<AssetCzInfo>();

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
                 dr["处置单号"] = list[i].BillNo;
                 dr["处置资产"] = list[i].AssetDesc;
                 dr["资产管理部门（单位）"] = list[i].ChargeDept;
                 dr["部门资产管理员"] = list[i].DeptAdmin;
                 dr["申请单编号"] = list[i].Apply_ID;
                 dr["申请单日期"] = list[i].ApplyDate;
                 dr["申请部门"] = list[i].ApplyDept;
                 dr["备注信息"] = list[i].Note;
                 dr["附件组别ID"] = list[i].AttachGUID;
                 dr["创建人"] = list[i].Creator;
                 dr["创建时间"] = list[i].CreateTime;
                   //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            } 
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/AssetCz.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }
        
        #endregion
		
        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(AssetCzInfo info)
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

        protected override void OnBeforeUpdate(AssetCzInfo info)
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
            List<AssetCzInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<AssetCzInfo> list)
        {
			//如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (AssetCzInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.BillNo = info.BillNo;
                obj.AssetDesc = info.AssetDesc;
                obj.ChargeDept = SecurityHelper.GetDeptNameByID(info.ChargeDept);
                obj.DeptAdmin = SecurityHelper.GetFullNameByID(info.DeptAdmin);
                obj.Apply_ID = info.Apply_ID;
                obj.ApplyDate = info.ApplyDate;
                obj.ApplyDept = SecurityHelper.GetDeptNameByID(info.ApplyDept);
                obj.Note = info.Note;
                obj.AttachGUID = info.AttachGUID;
                obj.Creator = SecurityHelper.GetFullNameByID(info.Creator);
                obj.CreateTime = info.CreateTime;
                obj.Editor = SecurityHelper.GetFullNameByID(info.Editor);
                obj.EditTime = info.EditTime;
                //申请单状态
                var status =BLLFactory<Apply>.Instance.GetFieldValue(info.Apply_ID, "Status");
                obj.Status = ((ApplyStatus)status.ToInt32()).ToString(); 
                //参考转义代码
                //obj.Name = BLLFactory<AssetCz>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }
			
            return objList;
		}
        
        /// <summary>
        /// 获取单号
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBillNo()
        {
            var conditoin = new SearchCondition();
            //conditoin.AddDateCondition("CreateTime", DateTime.Now.ToString("yyyy-MM-dd"));//20200228
            var where = conditoin.BuildConditionSql().Replace("Where", "");

            int count = baseBLL.GetRecordCount(where) + 1;//获取当前的记录+1
            var prefix = "CZ";
            var result = string.Format("{0}{1}{2}", prefix, DateTime.Now.ToString("yyyyMMdd"), count.ToString().PadLeft(4, '0'));
            return Content(result);
        }

        /// <summary>
        /// 保存申请单主从表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveApply(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                var result = new CommonResult();

                if (obj.info != null)
                {
                    //获取主信息
                    AssetCzInfo info = (JObject.FromObject(obj.info)).ToObject<AssetCzInfo>();

                    //转换为明细信息
                    List<AssetCzDetailInfo> details = null;
                    if (obj.details != null)
                    {
                        details = (JArray.FromObject(obj.details)).ToObject<List<AssetCzDetailInfo>>();
                    }

                    if (info != null)
                    {
                        //修改部分信息
                        OnBeforeInsert(info);
                        bool succeed = BLLFactory<AssetCz>.Instance.InsertUpdate(info, info.ID);
                        if (succeed)
                        {
                            //移除已有列表数据，重新加入
                            var condition = string.Format("BillNo='{0}'", info.BillNo);
                            BLLFactory<AssetCzDetail>.Instance.DeleteByCondition(condition);

                            if (details != null)
                            {
                                foreach (var detailInfo in details)
                                {
                                    //设置关键信息
                                    detailInfo.BillNo = info.BillNo;
                                    detailInfo.CreateTime = DateTime.Now;
                                    detailInfo.Creator = CurrentUser.ID.ToString();

                                    //客户端提交LyDept和UsePerson需要解析，把Name|ID 转换为ID
                                    //detailInfo.CzDept = HtmlHelpers.GetSplitId(detailInfo.CzDept);
                                    //detailInfo.UsePerson = HtmlHelpers.GetSplitId(detailInfo.UsePerson);

                                    BLLFactory<AssetCzDetail>.Instance.InsertUpdate(detailInfo, detailInfo.ID);
                                }
                            }
                            result.Success = succeed;
                        }
                    }
                }
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
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

            var result = BLLFactory<AssetCz>.Instance.FindByApplyId(applyId);
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
                            var info = BLLFactory<AssetCz>.Instance.FindByID(strId);
                            if (info != null)
                            {
                                //由于明细没有进行级联删除，此处删除明细
                                var condition = string.Format("BillNo='{0}'", info.BillNo);
                                BLLFactory<AssetCzDetail>.Instance.DeleteByCondition(condition);

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

        /// <summary>
        /// 审批处理后的执行操作.
        /// 该操作一般发生在申请单完成后，需要修改业务表单数据的情况
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public ActionResult ToCompletedForm(string applyId)
        {
            CommonResult result = new CommonResult();
            //获取申请单信息
            var applyInfo = BLLFactory<Apply>.Instance.FindByID(applyId);
            if (applyInfo != null)
            {
                if (applyInfo.Status == ApplyStatus.已完成)
                {
                    var info = BLLFactory<AssetCz>.Instance.FindByApplyId(applyId);
                    if (info != null)
                    {
                        var list = BLLFactory<AssetCzDetail>.Instance.FindByBillNo(info.BillNo);
                        if (list != null && list.Count > 0)
                        {
                            foreach (var detail in list)
                            {
                                var asset = BLLFactory<Asset>.Instance.FindByCode(detail.AssetCode);
                                if (asset != null)
                                {
                                    //成功后将资产主表T_Asset资产动态 Status 更新为“处置”	,使用状况 UseStatus 更新为“报废/转让”	
                                    //CurrDept/ChargeDept/Operator/OperateDate/UsePerson/KeepAddr/Unit/Qty/OriginValue/Status/UseStatus
                                    asset.CurrDept = SecurityHelper.GetDeptNameByID(info.ChargeDept);
                                    asset.ChargeDept = SecurityHelper.GetDeptNameByID(info.ChargeDept);
                                    asset.Operator = SecurityHelper.GetFullNameByID(info.DeptAdmin);
                                    asset.OperateDate = DateTime.Now;
                                    asset.UsePerson = HtmlHelpers.GetSplitName(detail.UsePerson);
                                    asset.KeepAddr = detail.KeepAddr;
                                    asset.Unit = detail.Unit;
                                    asset.Qty = detail.TotalQty;
                                    asset.OriginValue = detail.TotalAmount;
                                    asset.Status = "处置";
                                    asset.UseStatus = detail.DisposeType;
                                    BLLFactory<Asset>.Instance.Update(asset, asset.ID);

                                    result.Success = true;//一个成功则标记成功
                                }
                            }
                        }
                    }
                }
            }

            return ToJsonContent(result);
        }
    }
}
