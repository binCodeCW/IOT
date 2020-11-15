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
    public class YH_DeviceInfoController : BusinessController<YH_DeviceInfo, YH_DeviceInfoInfo>
    {
        public YH_DeviceInfoController() : base()
        {
        }

        /// <summary>
        /// 设备类型统计
        /// </summary>
        /// <returns></returns>
        public ActionResult DeviceTypeReport()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string sql = "select '养和' as DeviceName, DeviceType, COUNT(1) as OnlineTotal, MAX(t.EditTime) as LastTime from YH_DeviceInfo t where t.OlineStatus = 1 and t.DeviceType in ('YH04D','YH04E','YH08A','YH01AS') group by t.DeviceType;";
            DataTable dt = baseBLL.SqlTable(sql: sql);
            return ToJsonContent(obj: dt);
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "仪器编号,仪器类型,医院简称,所在省份,城市,所在行政区,市场分区,医院地址,医院邮编,医院电话,传真号码,主联系人,联系人电话,联系人手机,电子邮件,客户类别,客户级别,信用等级,重要级别,备注信息,创建人,创建时间,设备ID,所属公司,经度,纬度,仪器在线状态";

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
        public List<YH_DeviceInfoInfo> GetExcelList(string guid)
        {
            List<YH_DeviceInfoInfo> list = new List<YH_DeviceInfoInfo>();
            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;
                    YH_DeviceInfoInfo info = new YH_DeviceInfoInfo();

                    info.DeviceId = dr["仪器编号"].ToString();
                    info.DeviceType = dr["仪器类型"].ToString();
                    info.SimpleName = dr["医院简称"].ToString();
                    info.Province = dr["所在省份"].ToString();
                    info.City = dr["城市"].ToString();
                    info.District = dr["所在行政区"].ToString();
                    info.Area = dr["市场分区"].ToString();
                    info.Address = dr["医院地址"].ToString();
                    info.ZipCode = dr["医院邮编"].ToString();
                    info.Telephone = dr["医院电话"].ToString();
                    info.Fax = dr["传真号码"].ToString();
                    info.Contact = dr["主联系人"].ToString();
                    info.ContactPhone = dr["联系人电话"].ToString();
                    info.ContactMobile = dr["联系人手机"].ToString();
                    info.Email = dr["电子邮件"].ToString();
                    info.CustomerType = dr["客户类别"].ToString();
                    info.Grade = dr["客户级别"].ToString();
                    info.CreditStatus = dr["信用等级"].ToString();
                    info.Importance = dr["重要级别"].ToString();
                    info.Note = dr["备注信息"].ToString();
                    info.Creator = dr["创建人"].ToString();
                    converted = DateTime.TryParse(dr["创建时间"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.CreateTime = dt;
                    }
                    info.IotID = dr["设备ID"].ToString();
                    info.Company_ID = dr["所属公司"].ToString();
                    info.Longtitude = dr["经度"].ToString().ToDouble();
                    info.Latitude = dr["纬度"].ToString().ToDouble();
                    info.OlineStatus = dr["仪器在线状态"].ToString().ToInt32();
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
        public CommonResult SaveList(List<YH_DeviceInfoInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<YH_DeviceInfo>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        foreach (YH_DeviceInfoInfo detail in list)
                        {
                            //修改部分信息
                            OnBeforeInsert(detail);

                            //在此判断插入数据条件
                            //var isExist = BLLFactory<YH_DeviceInfo>.Instance.IsExistKey("Code", detail.Code, trans);
                            //if (!isExist)
                            {
                                BLLFactory<YH_DeviceInfo>.Instance.Insert(detail, trans);
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
        public ActionResult SaveExcelData(List<YH_DeviceInfoInfo> list)
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
            List<YH_DeviceInfoInfo> list = new List<YH_DeviceInfoInfo>();

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
                dr["仪器编号"] = list[i].DeviceId;
                dr["仪器类型"] = list[i].DeviceType;
                dr["医院简称"] = list[i].SimpleName;
                dr["所在省份"] = list[i].Province;
                dr["城市"] = list[i].City;
                dr["所在行政区"] = list[i].District;
                dr["市场分区"] = list[i].Area;
                dr["医院地址"] = list[i].Address;
                dr["医院邮编"] = list[i].ZipCode;
                dr["医院电话"] = list[i].Telephone;
                dr["传真号码"] = list[i].Fax;
                dr["主联系人"] = list[i].Contact;
                dr["联系人电话"] = list[i].ContactPhone;
                dr["联系人手机"] = list[i].ContactMobile;
                dr["电子邮件"] = list[i].Email;
                dr["客户类别"] = list[i].CustomerType;
                dr["客户级别"] = list[i].Grade;
                dr["信用等级"] = list[i].CreditStatus;
                dr["重要级别"] = list[i].Importance;
                dr["备注信息"] = list[i].Note;
                dr["创建人"] = list[i].Creator;
                dr["创建时间"] = list[i].CreateTime;
                dr["设备ID"] = list[i].IotID;
                dr["所属公司"] = list[i].Company_ID;
                dr["经度"] = list[i].Longtitude;
                dr["纬度"] = list[i].Latitude;
                dr["仪器在线状态"] = list[i].OlineStatus;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/YH_DeviceInfo.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(YH_DeviceInfoInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(YH_DeviceInfoInfo info)
        {
            //子类对参数对象进行修改
            //info.Editor = CurrentUser.ID.ToString();
            //info.EditTime = DateTime.Now;
        }
        #endregion

        public override ActionResult Insert(YH_DeviceInfoInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            if (info != null)
            {
                try
                {
                    OnBeforeInsert(info);

                    var exist = baseBLL.IsExistRecord(string.Format("Deviceid='{0}'", info.DeviceId));
                    if (exist)
                    {
                        result.ErrorMessage = "仪器编号已存在！";
                    }
                    else
                    {
                        result.Success = baseBLL.Insert(info);
                    }
                }
                catch (Exception ex)
                {
                    LogTextHelper.Error(ex);//错误记录
                    result.ErrorMessage = ex.Message;
                }
            }
            return ToJsonContent(result);

        }

        protected override CommonResult Update(string id, YH_DeviceInfoInfo info)
        {
            CommonResult result = new CommonResult();
            try
            {
                OnBeforeUpdate(info);

                //非当前记录不能重复设备ID
                var exist = baseBLL.IsExistRecord(string.Format("Deviceid='{0}' AND ID <> '{1}'", info.DeviceId, info.ID));
                if (exist)
                {
                    result.ErrorMessage = "仪器编号已存在！";
                }
                else
                {
                    result.Success = baseBLL.Update(info, id);
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }

            return result;
        }


        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();
            List<YH_DeviceInfoInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<YH_DeviceInfoInfo> list)
        {
            var DeviceDataRec1 = BLLFactory<YH04_TestData>.Instance.GetAll();
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (YH_DeviceInfoInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.DeviceId = info.DeviceId;
                obj.DeviceType = info.DeviceType;
                obj.SimpleName = info.SimpleName;
                obj.Province = info.Province;
                obj.City = info.City;
                obj.District = info.District;
                obj.Area = info.Area;
                obj.Address = info.Address;
                obj.ZipCode = info.ZipCode;
                obj.Telephone = info.Telephone;
                obj.Fax = info.Fax;
                obj.Contact = info.Contact;
                obj.ContactPhone = info.ContactPhone;
                obj.ContactMobile = info.ContactMobile;
                obj.Email = info.Email;
                obj.CustomerType = info.CustomerType;
                obj.Grade = info.Grade;
                obj.CreditStatus = info.CreditStatus;
                obj.Importance = info.Importance;
                obj.Note = info.Note;
                obj.Creator = info.Creator;
                obj.CreateTime = info.CreateTime;
                obj.Editor = info.Editor;
                obj.EditTime = info.EditTime;
                obj.IotID = info.IotID;
                obj.Company_ID = info.Company_ID;
                obj.Longtitude = info.Longtitude;
                obj.Latitude = info.Latitude;
                obj.OlineStatus = info.OlineStatus;


                //TimeSpan ts;
                //TimeSpan ts = DateTime.Now - DeviceDataRec1.Stamptime;
                //DeviceDataRec1.Where(o => () <  1.0 ).FirstOrDefault()?.ErrorText;
                //obj.TotalDays = ts.TotalDays;
                //if (obj.TotalDays <= 1.0)
                //{
                //    obj.OlineStatus = 1;
                //}
                //else
                //{
                //    obj.OlineStatus = 0;
                //}

                //参考转义代码
                //obj.Name = BLLFactory<YH_DeviceInfo>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }
    }
}
