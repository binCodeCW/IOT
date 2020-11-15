using System;
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

namespace IOT.MVCWebMis.Controllers
{
    public class StoreAddressController : BusinessController<StoreAddress, StoreAddressInfo>
    {
        public StoreAddressController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "部门,存放地点,备注信息,创建人,创建时间";

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

            List<StoreAddressInfo> list = new List<StoreAddressInfo>();

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
                    StoreAddressInfo info = new StoreAddressInfo();

                    info.Dept_ID = dr["部门"].ToString();
                    info.KeepAddr = dr["存放地点"].ToString();
                    info.Note = dr["备注信息"].ToString();
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
        public ActionResult SaveExcelData(List<StoreAddressInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<StoreAddress>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (StoreAddressInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            /*
                            detail.CreateTime = DateTime.Now;
                            detail.Creator = CurrentUser.ID.ToString();
                            detail.Editor = CurrentUser.ID.ToString();
                            detail.EditTime = DateTime.Now;
							*/

                            BLLFactory<StoreAddress>.Instance.Insert(detail, trans);
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
            List<StoreAddressInfo> list = new List<StoreAddressInfo>();

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
                dr["部门"] = list[i].Dept_ID;
                dr["存放地点"] = list[i].KeepAddr;
                dr["备注信息"] = list[i].Note;
                dr["创建人"] = list[i].Creator;
                dr["创建时间"] = list[i].CreateTime;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/StoreAddress.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(StoreAddressInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            info.CreateTime = DateTime.Now;
            info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(StoreAddressInfo info)
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
            List<StoreAddressInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<StoreAddressInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (StoreAddressInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.Dept_ID = info.Dept_ID;
                obj.KeepAddr = info.KeepAddr;
                obj.Note = info.Note;
                obj.Creator = info.Creator;
                obj.CreateTime = info.CreateTime;
                //参考转义代码
                //obj.Name = BLLFactory<StoreAddress>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="Dept_ID">部门ID</param>
        /// <param name="KeepAddr">部门位置</param>
        /// <returns></returns>
        public ActionResult SaveData(string Dept_ID, string KeepAddr)
        {
            var result = new CommonResult();
            string condition = string.Format("Dept_ID='{0}'", Dept_ID);
            var info = BLLFactory<StoreAddress>.Instance.FindSingle(condition);
            if (info != null)
            {
                info.KeepAddr = KeepAddr;
                result.Success = BLLFactory<StoreAddress>.Instance.Update(info, info.ID);
            }
            else
            {
                info = new StoreAddressInfo();
                OnBeforeInsert(info);
                info.KeepAddr = KeepAddr;
                info.Dept_ID = Dept_ID;
                result.Success = BLLFactory<StoreAddress>.Instance.Insert(info);
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 根据部门ID获取对应记录
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public ActionResult FindByDeptId(string deptId)
        {
            string condition = string.Format("Dept_ID='{0}'", deptId);
            var info = BLLFactory<StoreAddress>.Instance.FindSingle(condition);
            return ToJsonContent(info);
        }

        /// <summary>
        /// 根据部门ID列表删除相关记录
        /// </summary>
        /// <param name="ids">部门ID字符串，逗号分隔</param>
        /// <returns></returns>
        public ActionResult DeleteByDeptIds(string ids)
        {
            var result = new CommonResult();
            List<string> idList = ids.ToDelimitedList<string>(",");
            foreach (var deptId in idList)
            {
                string condition = string.Format("Dept_ID='{0}'", deptId);
                result.Success = BLLFactory<StoreAddress>.Instance.DeleteByCondition(condition);
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 获取数据，在列表中选择
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public ActionResult GetDictJson(string deptId)
        {
            List<CListItem> treeList = new List<CListItem>();
            string condition = string.Format("Dept_ID='{0}'", deptId);
            var info = BLLFactory<StoreAddress>.Instance.FindSingle(condition);
            if (info != null && !string.IsNullOrEmpty(info.KeepAddr))
            {
                var list = info.KeepAddr.ToDelimitedList<string>(",");
                foreach (var item in list)
                {
                    treeList.Add(new CListItem(item));
                }
            }
            return ToJsonContent(treeList);
        }
    }
}
