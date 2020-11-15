using System;
using System.Data;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Dynamic;

using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;
using IOT.MVCWebMis.Entity;
using Newtonsoft.Json.Linq;

namespace IOT.MVCWebMis.Controllers
{
    public class AssetLyDetailController : BusinessController<AssetLyDetail, AssetLyDetailInfo>
    {
        public AssetLyDetailController() : base()
        {
        }

        #region 导入Excel数据操作
	    //导入或导出的字段列表
        string columnString = "领用单号,资产编号,资产名称,使用人,资产使用部门（单位）,存放地点,单位,单价,数量,金额,备注,创建人,创建时间";

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

            List<AssetLyDetailInfo> list = new List<AssetLyDetailInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;                    
                    AssetLyDetailInfo info = new AssetLyDetailInfo();
                    
                     info.BillNo = dr["领用单号"].ToString();
                      info.AssetCode = dr["资产编号"].ToString();
                      info.AssetName = dr["资产名称"].ToString();
                      info.UsePerson = dr["使用人"].ToString();
                      info.LyDept = dr["资产使用部门（单位）"].ToString();
                      info.KeepAddr = dr["存放地点"].ToString();
                      info.Unit = dr["单位"].ToString();
                      info.Price = dr["单价"].ToString().ToDecimal();
                      info.TotalQty = dr["数量"].ToString().ToInt32();
                      info.TotalAmount = dr["金额"].ToString().ToDecimal();
                      info.Note = dr["备注"].ToString();
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
        public ActionResult SaveExcelData(List<AssetLyDetailInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<AssetLyDetail>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (AssetLyDetailInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
							/*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<AssetLyDetail>.Instance.Insert(detail, trans);
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
            List<AssetLyDetailInfo> list = new List<AssetLyDetailInfo>();

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
                 dr["领用单号"] = list[i].BillNo;
                 dr["资产编号"] = list[i].AssetCode;
                 dr["资产名称"] = list[i].AssetName;
                 dr["使用人"] = list[i].UsePerson;
                 dr["资产使用部门（单位）"] = list[i].LyDept;
                 dr["存放地点"] = list[i].KeepAddr;
                 dr["单位"] = list[i].Unit;
                 dr["单价"] = list[i].Price;
                 dr["数量"] = list[i].TotalQty;
                 dr["金额"] = list[i].TotalAmount;
                 dr["备注"] = list[i].Note;
                 dr["创建人"] = list[i].Creator;
                 dr["创建时间"] = list[i].CreateTime;
                 //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            } 
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/AssetLyDetail.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }
        
        #endregion
		
        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(AssetLyDetailInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            info.CreateTime = DateTime.Now;
            info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(AssetLyDetailInfo info)
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
            List<AssetLyDetailInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<AssetLyDetailInfo> list)
        {
			//如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach(AssetLyDetailInfo info in list)
            {
                dynamic obj = new ExpandoObject();
			
                obj.ID = info.ID;
                 obj.BillNo = info.BillNo;
                 obj.AssetCode = info.AssetCode;
                 obj.AssetName = info.AssetName;
                 obj.UsePerson = info.UsePerson;
                 obj.LyDept = info.LyDept;
                 obj.KeepAddr = info.KeepAddr;
                 obj.Unit = info.Unit;
                 obj.Price = info.Price;
                 obj.TotalQty = info.TotalQty;
                 obj.TotalAmount = info.TotalAmount;
                 obj.Note = info.Note;
                 obj.Creator = info.Creator;
                 obj.CreateTime = info.CreateTime;
 				//参考转义代码
				//obj.Name = BLLFactory<AssetLyDetail>.Instance.GetNameByID(info.ID);
                
                objList.Add(obj);
            } 
			
            return objList;
		}

        /// <summary>
        /// 根据业务单号获取对应对象信息
        /// </summary>
        /// <param name="billNo">业务单号</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FindByBillNo(string billNo)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<AssetLyDetail>.Instance.FindByBillNo(billNo);
            return ToJsonContent(result);
        }

        /// <summary>
        /// 保存明细数据
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveDetail(JObject param)
        {
            dynamic obj = param;
            if (obj != null)
            {
                var result = new CommonResult();

                //转换为明细信息
                List<AssetLyDetailInfo> details = null;
                if (obj.billno != null)
                {
                    //移除已有列表数据，重新加入
                    var condition = string.Format("BillNo='{0}'", obj.billno);
                    BLLFactory<AssetLyDetail>.Instance.DeleteByCondition(condition);

                    if (obj.list != null)
                    {
                        details = (JArray.FromObject(obj.list)).ToObject<List<AssetLyDetailInfo>>();
                        if (details != null && details.Count > 0)
                        {
                            foreach (var detailInfo in details)
                            {
                                //修改部分信息
                                OnBeforeInsert(detailInfo);

                                //设置关键信息
                                detailInfo.BillNo = obj.billno;

                                BLLFactory<AssetLyDetail>.Instance.Insert(detailInfo);
                            }
                        }
                    }
                    result.Success = true;
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
