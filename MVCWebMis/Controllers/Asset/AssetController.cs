using System;
using System.IO;
using System.Data;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Dynamic;

using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.Entity;
using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;
using IOT.MVCWebMis.Commons;
using FastReport;
using FastReport.Export.Pdf;

namespace IOT.MVCWebMis.Controllers
{
    public class AssetController : BusinessController<Asset, AssetInfo>
    {
        public AssetController() : base()
        {
        }

        public ActionResult Report()
        {
            return View("Report");
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "资产编码,资产类别,类别名称,使用部门,管理部门,增加方式,使用状况,购置日期,使用人,规格型号,存放地点,数量,资产备注,计量单位,单价,本币原值,使用年限,使用到期日,已使用年数,是否超年限,资产动态,在用类型,保管人,盘点状态,盘点日期,盘点人,财务对账,对账日期,核对人,用户账号,自定义1,自定义2,自定义3,备注,操作人,操作日期";

        /// <summary>
        /// 检查Excel文件的字段是否包含了必须的字段
        /// </summary>
        /// <param name="guid">Excel文件的GUID</param>
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
        public List<AssetInfo> GetExcelList(string guid)
        {
            List<AssetInfo> list = new List<AssetInfo>();
            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;
                    AssetInfo info = new AssetInfo();

                    info.Code = dr["资产编码"].ToString();
                    info.Name = dr["资产类别"].ToString();
                    info.ClassName = dr["类别名称"].ToString();
                    info.CurrDept = dr["使用部门"].ToString();
                    info.ChargeDept = dr["管理部门"].ToString();
                    info.AddMethod = dr["增加方式"].ToString();
                    info.UseStatus = dr["使用状况"].ToString();
                    converted = DateTime.TryParse(dr["购置日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.PurchaseDate = dt;
                    }
                    info.UsePerson = dr["使用人"].ToString();
                    info.Spec = dr["规格型号"].ToString();
                    info.KeepAddr = dr["存放地点"].ToString();
                    info.Qty = dr["数量"].ToString().ToInt32();
                    info.Remark = dr["资产备注"].ToString();
                    info.Unit = dr["计量单位"].ToString();
                    info.Price = dr["单价"].ToString().ToDecimal();
                    info.OriginValue = dr["本币原值"].ToString().ToDecimal();
                    info.LimitYears = dr["使用年限"].ToString().ToInt32();
                    converted = DateTime.TryParse(dr["使用到期日"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.DueDate = dt;
                    }
                    info.UseYears = dr["已使用年数"].ToString();
                    info.IsAge = dr["是否超年限"].ToString();
                    info.Status = dr["资产动态"].ToString();
                    info.UseType = dr["在用类型"].ToString();
                    info.Keeper = dr["保管人"].ToString();
                    info.InvStatus = dr["盘点状态"].ToString();
                    converted = DateTime.TryParse(dr["盘点日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.InvDate = dt;
                    }
                    info.Inventory = dr["盘点人"].ToString();
                    info.Account = dr["财务对账"].ToString();
                    converted = DateTime.TryParse(dr["对账日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.AccDate = dt;
                    }
                    info.AccHolder = dr["核对人"].ToString();
                    info.UserName = dr["用户账号"].ToString();
                    info.UserDef1 = dr["自定义1"].ToString();
                    info.UserDef2 = dr["自定义2"].ToString();
                    info.UserDef3 = dr["自定义3"].ToString();
                    info.Note = dr["备注"].ToString();
                    info.Operator = dr["操作人"].ToString();
                    converted = DateTime.TryParse(dr["操作日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.OperateDate = dt;
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
        public CommonResult SaveList(List<AssetInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<Asset>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        foreach (AssetInfo detail in list)
                        {
                            //修改部分信息
                            OnBeforeInsert(detail);

                            //在此判断插入数据条件
                            var isExist = BLLFactory<Asset>.Instance.IsExistKey("Code", detail.Code, trans);
                            if (!isExist)
                            {
                                BLLFactory<Asset>.Instance.Insert(detail, trans);
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
        public ActionResult SaveExcelData(List<AssetInfo> list)
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
            List<AssetInfo> list = new List<AssetInfo>();

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
                dr["资产编码"] = list[i].Code;
                dr["资产类别"] = list[i].Name;
                dr["类别名称"] = list[i].ClassName;
                dr["使用部门"] = list[i].CurrDept;
                dr["管理部门"] = list[i].ChargeDept;
                dr["增加方式"] = list[i].AddMethod;
                dr["使用状况"] = list[i].UseStatus;
                dr["购置日期"] = list[i].PurchaseDate;
                dr["使用人"] = list[i].UsePerson;
                dr["规格型号"] = list[i].Spec;
                dr["存放地点"] = list[i].KeepAddr;
                dr["数量"] = list[i].Qty;
                dr["资产备注"] = list[i].Remark;
                dr["计量单位"] = list[i].Unit;
                dr["单价"] = list[i].Price;
                dr["本币原值"] = list[i].OriginValue;
                dr["使用年限"] = list[i].LimitYears;
                dr["使用到期日"] = list[i].DueDate;
                dr["已使用年数"] = list[i].UseYears;
                dr["是否超年限"] = list[i].IsAge;
                dr["资产动态"] = list[i].Status;
                dr["在用类型"] = list[i].UseType;
                dr["保管人"] = list[i].Keeper;
                dr["盘点状态"] = list[i].InvStatus;
                dr["盘点日期"] = list[i].InvDate;
                dr["盘点人"] = list[i].Inventory;
                dr["财务对账"] = list[i].Account;
                dr["对账日期"] = list[i].AccDate;
                dr["核对人"] = list[i].AccHolder;
                dr["用户账号"] = list[i].UserName;
                dr["自定义1"] = list[i].UserDef1;
                dr["自定义2"] = list[i].UserDef2;
                dr["自定义3"] = list[i].UserDef3;
                dr["备注"] = list[i].Note;
                dr["操作人"] = list[i].Operator;
                dr["操作日期"] = list[i].OperateDate;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/Asset.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(AssetInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            info.OperateDate = DateTime.Now;
            info.Operator = CurrentUser.FullName.ToString();

            info.Status = "待使用";
            info.UseStatus = "待使用";
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(AssetInfo info)
        {
            //子类对参数对象进行修改
            //info.Editor = CurrentUser.ID.ToString();
            //info.EditTime = DateTime.Now;
        }
        #endregion

        /// <summary>
        /// 获取使用状态为待使用
        /// </summary>
        /// <returns></returns>
        public ActionResult FindInUsedWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            if (!string.IsNullOrEmpty(where))
            {
                where += string.Format(" AND UseStatus='{0}' AND Status='{0}'", "待使用");
            }

            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();
            List<AssetInfo> list = null;
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
        /// 获取资产名称和分类，用于生成盘点单据
        /// </summary>
        /// <returns></returns>
        public ActionResult FindAsset()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string companyId = Request["Company_ID"];
            string deptId = Request["Dept_ID"];
            string condition = string.Format("a.Company_ID = '{0}'", companyId);
            if (!string.IsNullOrEmpty(deptId))
            {
                condition += string.Format(" AND a.Dept_ID ='{0}' ", deptId);
            }

            string sql = string.Format(@"SELECT DISTINCT Dept_ID, sum(Qty) as Qty,ou.Name FROM T_Asset a 
            inner join T_ACL_OU ou  on a.Dept_ID=ou.ID Where {0} group by Dept_ID, ou.Name", condition);

            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();
            DataTable dt = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                dt = baseBLL.SqlTableWithPager(sql, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                dt = baseBLL.SqlTableWithPager(sql, pagerInfo, "Name", true);
            }

            //转换ExpandoObject对象列表
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic obj = new ExpandoObject();

                    obj.Name = dr["Name"];
                    obj.Qty = dr["Qty"];
                    objList.Add(obj);
                }
            }

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = objList };
            return ToJsonContent(result);
        }

        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();
            List<AssetInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<AssetInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (AssetInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.Code = info.Code;
                obj.Name = info.Name;
                obj.ClassName = info.ClassName;
                obj.CurrDept = info.CurrDept;
                obj.ChargeDept = info.ChargeDept;
                obj.AddMethod = info.AddMethod;
                obj.UseStatus = info.UseStatus;
                obj.PurchaseDate = info.PurchaseDate;
                obj.UsePerson = info.UsePerson;
                obj.Spec = info.Spec;
                obj.KeepAddr = info.KeepAddr;
                obj.Qty = info.Qty;
                obj.Remark = info.Remark;
                obj.Unit = info.Unit;
                obj.Price = info.Price;
                obj.OriginValue = info.OriginValue;
                obj.LimitYears = info.LimitYears;
                obj.DueDate = info.DueDate;
                obj.UseYears = info.UseYears;
                obj.IsAge = info.IsAge;
                obj.Status = info.Status;
                obj.UseType = info.UseType;
                obj.Keeper = info.Keeper;
                obj.InvStatus = info.InvStatus;
                obj.InvDate = info.InvDate;
                obj.Inventory = info.Inventory;
                obj.Account = info.Account;
                obj.AccDate = info.AccDate;
                obj.AccHolder = info.AccHolder;
                obj.UserName = info.UserName;
                obj.UserDef1 = info.UserDef1;
                obj.UserDef2 = info.UserDef2;
                obj.UserDef3 = info.UserDef3;
                obj.Note = info.Note;
                obj.Operator = info.Operator;
                obj.OperateDate = info.OperateDate;
                //参考转义代码
                //obj.Name = BLLFactory<Asset>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }

        /// <summary>
        /// 获取资产分类
        /// </summary>
        /// <returns></returns>
        public ActionResult GetClassName()
        {
            List<JsTreeData> result = new List<JsTreeData>();
            var dict = BLLFactory<Asset>.Instance.GetFieldList("ClassName");
            foreach (var name in dict)
            {
                JsTreeData treeData = new JsTreeData(name, name);
                result.Add(treeData);
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 根据资产编码进行查询特定的记录
        /// </summary>
        /// <param name="code">资产编码</param>
        /// <returns></returns>
        public ActionResult FindByCode(string code)
        {
            var info = BLLFactory<Asset>.Instance.FindByCode(code);
            return ToJsonContent(info);
        }

        /// <summary>
        /// 获取Grid++的JSON格式报表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRepotData(string ids)
        {
            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(ids))
            {
                var list = BLLFactory<Asset>.Instance.FindByIDs(ids);
                //构建一个合法格式的对象，进行序列号
                var table = new
                {
                    Table = list
                };
                result = ToJsonContent(table);
            }
            return result;
        }

        /// <summary>
        /// 根据选中的ID记录，生成对应的PDF报表，返回路径
        /// </summary>
        /// <param name="ids">选中的ID记录，逗号分开</param>
        /// <returns></returns>
        public ActionResult ExportPdf(string ids)
        {
            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(ids))
            {
                //利用接口获取列表数据
                var list = BLLFactory<Asset>.Instance.FindByIDs(ids);
                //报表文件路径
                string reportPath = "/Report/barcode2.frx";
                //转换为物理路径
                reportPath = Server.MapPath(reportPath);

                //导出PDF的文件路径
                string exportPdfPath = string.Format("/GenerateFiles/{0}/AssetReport.pdf", CurrentUser.Name);
                //转换为物理路径
                string realPath = Server.MapPath(exportPdfPath);
                //确保目录生成
                string parentPath = Directory.GetParent(realPath).FullName;
                DirectoryUtil.AssertDirExist(parentPath);

                //生成PDF报表文档到具体文件
                Report report = new Report();
                report.Load(reportPath);

                //定义参数和数据格式
                var dict = new Dictionary<string, object>();
                var dt = DataTableHelper.CreateTable("ID,Name,CurrDept,Code,UsePerson,KeepAddr");
                if (list != null)
                {
                    foreach (var info in list)
                    {
                        var dr = dt.NewRow();
                        dr["ID"] = info.ID;
                        dr["Name"] = info.Name;
                        dr["CurrDept"] = info.CurrDept;
                        dr["Code"] = info.Code;
                        dr["UsePerson"] = info.UsePerson;
                        dr["KeepAddr"] = info.KeepAddr;
                        dt.Rows.Add(dr);
                    }
                }

                //刷新数据源
                foreach (string key in dict.Keys)
                {
                    report.SetParameterValue(key, dict[key]);
                }
                report.RegisterData(dt, "T_Asset");

                //运行报表
                report.Prepare();

                //导出PDF报表
                PDFExport export = new PDFExport();
                report.Export(export, realPath);
                report.Dispose();

                if(System.IO.File.Exists(realPath))
                {
                    result = Content(exportPdfPath);//返回Web相对路径
                }
            }
            return result;
        }

        /// <summary>
        /// 根据选中的ID记录，生成对应的PDF报表，返回路径
        /// </summary>
        /// <param name="ids">选中的ID记录，逗号分开</param>
        /// <returns></returns>
        public ActionResult ExportPdfWithGridPlus(string ids)
        {
            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(ids))
            {
                //利用接口获取列表数据
                var list = BLLFactory<Asset>.Instance.FindByIDs(ids);
                //报表文件路径
                string reportPath = "/Report/barcode.grf";
                //转换为物理路径
                reportPath = Server.MapPath(reportPath);

                //导出PDF的文件路径
                string exportPdfPath = string.Format("/GenerateFiles/{0}/AssetReport.pdf", CurrentUser.Name);
                //转换为物理路径
                string realPath = Server.MapPath(exportPdfPath);
                //确保目录生成
                string parentPath = Directory.GetParent(realPath).FullName;
                DirectoryUtil.AssertDirExist(parentPath);

                //生成PDF报表文档到具体文件
                GridExportHelper helper = new GridExportHelper(reportPath);
                bool success = helper.ExportPdf(list, realPath, HttpContext);
                if (success)
                {
                    result = Content(exportPdfPath);//返回Web相对路径
                }
                helper.Dispose();//销毁对象
            }
            return result;
        }
    }
}
