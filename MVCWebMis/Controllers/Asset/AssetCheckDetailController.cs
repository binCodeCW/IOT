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

namespace IOT.MVCWebMis.Controllers
{
    public class AssetCheckDetailController : BusinessController<AssetCheckDetail, AssetCheckDetailInfo>
    {
        public AssetCheckDetailController() : base()
        {
        }

        #region 导入Excel数据操作
 		
	    //导入或导出的字段列表   
        string columnString = "盘点单号,资产代码,资产名称,资产图片,数量,使用人,使用部门,存放地点,盘点结果,盘点结果表达,备注,创建人,创建时间,企业号UserID,盘点人,盘点日期";

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
        /// 获取服务器上的Excel文件，并把它转换为实体列表
        /// </summary>
        /// <param name="guid">Excel文件的GUID</param>
        /// <returns></returns>
        public List<AssetCheckDetailInfo> GetExcelList(string guid)
        {
            List<AssetCheckDetailInfo> list = new List<AssetCheckDetailInfo>();
            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;                    
                    AssetCheckDetailInfo info = new AssetCheckDetailInfo();
                    
                     info.BillNo = dr["盘点单号"].ToString();
                      info.AssetCode = dr["资产代码"].ToString();
                      info.AssetName = dr["资产名称"].ToString();
                      info.AttachGUID = dr["资产图片"].ToString();
                      info.Qty = dr["数量"].ToString().ToInt32();
                      info.UsePerson = dr["使用人"].ToString();
                      info.CurrDept = dr["使用部门"].ToString();
                      info.KeepAddr = dr["存放地点"].ToString();
                      info.CheckResult = dr["盘点结果"].ToString().ToInt32();
                      info.CheckDisplay = dr["盘点结果表达"].ToString();
                      info.Note = dr["备注"].ToString();
                      info.Creator = dr["创建人"].ToString();
                      converted = DateTime.TryParse(dr["创建时间"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                         info.CreateTime = dt;
                    }
                      info.CorpUserId = dr["企业号UserID"].ToString();
                      info.CheckPerson = dr["盘点人"].ToString();
                      converted = DateTime.TryParse(dr["盘点日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                         info.CheckDate = dt;
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
            return list;
        }

        /// <summary>
        /// 把列表批量写入数据库
        /// </summary>
        /// <param name="list">保存的列表</param>
        /// <returns></returns>
        public CommonResult SaveList(List<AssetCheckDetailInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<AssetCheckDetail>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        foreach (AssetCheckDetailInfo detail in list)
                        {
                            //修改部分信息
                            OnBeforeInsert(detail);

                            //在此判断插入数据条件
                            //var isExist = BLLFactory<AssetCheckDetail>.Instance.IsExistKey("Code", detail.Code, trans);
                            //if (!isExist)
                            {
                                BLLFactory<AssetCheckDetail>.Instance.Insert(detail, trans);
                            }
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
            return result;
        }

        /// <summary>
        /// 获取服务器上的Excel文件，并把它转换为实体列表返回给客户端
        /// </summary>
        /// <param name="guid">Excel文件的GUID</param>
        /// <returns></returns>
        public ActionResult GetExcelData(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }
            var list = GetExcelList(guid);

            var result = new { total = list.Count, rows = list };
            return ToJsonContent(result);
        }

        /// <summary>
        /// 在服务端解析Excel数据，并批量写入数据库
        /// </summary>
        /// <param name="guid">Excel文件的GUID</param>
        /// <returns></returns>
        public ActionResult SaveExcelByGuid(string guid)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(guid))
            {
                var list = GetExcelList(guid);
                result = SaveList(list);
            }
            else
            {
                result.ErrorMessage = "导入信息不能为空";
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 保存客户端上传的相关数据列表
        /// </summary>
        /// <param name="list">数据列表</param>
        /// <returns></returns>
        public ActionResult SaveExcelData(List<AssetCheckDetailInfo> list)
        {
            var result = SaveList(list);
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
            List<AssetCheckDetailInfo> list = new List<AssetCheckDetailInfo>();

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
                 dr["盘点单号"] = list[i].BillNo;
                 dr["资产代码"] = list[i].AssetCode;
                 dr["资产名称"] = list[i].AssetName;
                 dr["资产图片"] = list[i].AttachGUID;
                 dr["数量"] = list[i].Qty;
                 dr["使用人"] = list[i].UsePerson;
                 dr["使用部门"] = list[i].CurrDept;
                 dr["存放地点"] = list[i].KeepAddr;
                 dr["盘点结果"] = list[i].CheckResult;
                 dr["盘点结果表达"] = list[i].CheckDisplay;
                 dr["备注"] = list[i].Note;
                 dr["创建人"] = list[i].Creator;
                 dr["创建时间"] = list[i].CreateTime;
                 dr["企业号UserID"] = list[i].CorpUserId;
                 dr["盘点人"] = list[i].CheckPerson;
                 dr["盘点日期"] = list[i].CheckDate;
                 //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            } 
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/AssetCheckDetail.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }
        
        #endregion
		
        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(AssetCheckDetailInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;
            
            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(AssetCheckDetailInfo info)
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
            List<AssetCheckDetailInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = ConvertListToShow(list) }; //如果使用转义，请使用objList对象
            return ToJsonContent(result);
        }

        /// <summary>
        /// 将实体类对象转换为页面显示的信息，包括转义部分字段，以方便显示使用
        /// </summary>
        /// <param name="info">实体类信息</param>
        /// <returns></returns>
        protected override ExpandoObject ConvertEntityToShow(AssetCheckDetailInfo info)
        {
            dynamic obj = new ExpandoObject();

            obj.ID = info.ID;
            obj.BillNo = info.BillNo;
            obj.AssetCode = info.AssetCode;
            obj.AssetName = info.AssetName;
            obj.AttachGUID = info.AttachGUID;
            obj.Qty = info.Qty;
            obj.UsePerson = info.UsePerson;
            obj.CurrDept = info.CurrDept;
            obj.KeepAddr = info.KeepAddr;
            obj.CheckResult = info.CheckResult;
            obj.CheckDisplay = info.CheckDisplay;
            obj.Note = info.Note;
            obj.Creator = info.Creator;
            obj.CreateTime = info.CreateTime;
            obj.CorpUserId = info.CorpUserId;
            obj.CheckPerson = info.CheckPerson;
            obj.CheckDate = info.CheckDate;

            //参考转义代码
            //obj.Name = BLLFactory<AssetCheckDetail>.Instance.GetNameByID(info.ID);
            return obj;
        }
    }
}
