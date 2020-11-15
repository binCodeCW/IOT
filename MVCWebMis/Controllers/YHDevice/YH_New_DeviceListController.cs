using System;
using System.Data;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.BLL;
using IOT.MVCWebMis.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class YH_New_DeviceListController : BusinessController<YH_New_DeviceList, YH_New_DeviceListInfo>
    {
        public YH_New_DeviceListController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "设备名称，型号+ 设备编号,租户ID,产品ID,IMEI号,IMSI号,firmwareVersion,deviceStatus,autoObserver,createTime,createTimeTrans,createBy,updateTime,updateTimeTrans,updateBy,netStatus,onlineAt,onlineAtTrans,offlineAt,offlineAtTrans,model,serial,最新位置,最后定位时间";

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
        public List<YH_New_DeviceListInfo> GetExcelList(string guid)
        {
            List<YH_New_DeviceListInfo> list = new List<YH_New_DeviceListInfo>();
            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;
                    YH_New_DeviceListInfo info = new YH_New_DeviceListInfo();

                    info.DeviceName = dr["设备名称，型号+ 设备编号"].ToString();
                    info.TenantId = dr["租户ID"].ToString();
                    info.ProductId = dr["产品ID"].ToString();
                    info.Imei = dr["IMEI号"].ToString();
                    info.Imsi = dr["IMSI号"].ToString();
                    info.FirmwareVersion = dr["firmwareVersion"].ToString();
                    info.DeviceStatus = dr["deviceStatus"].ToString();
                    info.AutoObserver = dr["autoObserver"].ToString();
                    info.CreateTime = dr["createTime"].ToString();
                    info.CreateTimeTrans = dr["createTimeTrans"].ToString();
                    info.CreateBy = dr["createBy"].ToString();
                    info.UpdateTime = dr["updateTime"].ToString();
                    info.UpdateTimeTrans = dr["updateTimeTrans"].ToString();
                    info.UpdateBy = dr["updateBy"].ToString();
                    info.NetStatus = dr["netStatus"].ToString();
                    info.OnlineAt = dr["onlineAt"].ToString();
                    info.OnlineAtTrans = dr["onlineAtTrans"].ToString();
                    info.OfflineAt = dr["offlineAt"].ToString();
                    info.OfflineAtTrans = dr["offlineAtTrans"].ToString();
                    info.Model = dr["model"].ToString();
                    info.Serial = dr["serial"].ToString();
                    info.Locationrecent = dr["最新位置"].ToString();
                    converted = DateTime.TryParse(dr["最后定位时间"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.Locatetime = dt;
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
        public CommonResult SaveList(List<YH_New_DeviceListInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<YH_New_DeviceList>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        foreach (YH_New_DeviceListInfo detail in list)
                        {
                            //修改部分信息
                            OnBeforeInsert(detail);

                            //在此判断插入数据条件
                            //var isExist = BLLFactory<YH_New_DeviceList>.Instance.IsExistKey("Code", detail.Code, trans);
                            //if (!isExist)
                            {
                                BLLFactory<YH_New_DeviceList>.Instance.Insert(detail, trans);
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
        public ActionResult SaveExcelData(List<YH_New_DeviceListInfo> list)
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
            List<YH_New_DeviceListInfo> list = new List<YH_New_DeviceListInfo>();

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
                dr["设备名称，型号+ 设备编号"] = list[i].DeviceName;
                dr["租户ID"] = list[i].TenantId;
                dr["产品ID"] = list[i].ProductId;
                dr["IMEI号"] = list[i].Imei;
                dr["IMSI号"] = list[i].Imsi;
                dr["firmwareVersion"] = list[i].FirmwareVersion;
                dr["deviceStatus"] = list[i].DeviceStatus;
                dr["autoObserver"] = list[i].AutoObserver;
                dr["createTime"] = list[i].CreateTime;
                dr["createTimeTrans"] = list[i].CreateTimeTrans;
                dr["createBy"] = list[i].CreateBy;
                dr["updateTime"] = list[i].UpdateTime;
                dr["updateTimeTrans"] = list[i].UpdateTimeTrans;
                dr["updateBy"] = list[i].UpdateBy;
                dr["netStatus"] = list[i].NetStatus;
                dr["onlineAt"] = list[i].OnlineAt;
                dr["onlineAtTrans"] = list[i].OnlineAtTrans;
                dr["offlineAt"] = list[i].OfflineAt;
                dr["offlineAtTrans"] = list[i].OfflineAtTrans;
                dr["model"] = list[i].Model;
                dr["serial"] = list[i].Serial;
                dr["最新位置"] = list[i].Locationrecent;
                dr["最后定位时间"] = list[i].Locatetime;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/YH_New_DeviceList.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(YH_New_DeviceListInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(YH_New_DeviceListInfo info)
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
            List<YH_New_DeviceListInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<YH_New_DeviceListInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (YH_New_DeviceListInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.DeviceId = info.DeviceId;
                obj.DeviceName = info.DeviceName;
                obj.TenantId = info.TenantId;
                obj.ProductId = info.ProductId;
                obj.Imei = info.Imei;
                obj.Imsi = info.Imsi;
                obj.FirmwareVersion = info.FirmwareVersion;
                obj.DeviceStatus = info.DeviceStatus;
                obj.AutoObserver = info.AutoObserver;
                obj.CreateTime = info.CreateTime;
                obj.CreateTimeTrans = info.CreateTimeTrans;
                obj.CreateBy = info.CreateBy;
                obj.UpdateTime = info.UpdateTime;
                obj.UpdateTimeTrans = info.UpdateTimeTrans;
                obj.UpdateBy = info.UpdateBy;
                obj.NetStatus = info.NetStatus;
                obj.OnlineAt = info.OnlineAt;
                obj.OnlineAtTrans = info.OnlineAtTrans;
                obj.OfflineAt = info.OfflineAt;
                obj.OfflineAtTrans = info.OfflineAtTrans;
                obj.Model = info.Model;
                obj.Serial = info.Serial;
                obj.Locationrecent = info.Locationrecent;
                obj.Locatetime = info.Locatetime;
                //参考转义代码
                //obj.Name = BLLFactory<YH_New_DeviceList>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }
    }
}
