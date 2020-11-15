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
    public class YH_DeviceControlController : BusinessController<YH_DeviceControl, YH_DeviceControlInfo>
    {
        public YH_DeviceControlController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "HTTPS接入方式IP,HTTPS接入方式端口,项目应用ID,项目应用密钥,服务名称,设备ID,命令名称,命令字段属性名称,上报数据是否是数字,上报数据内容";

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
        public List<YH_DeviceControlInfo> GetExcelList(string guid)
        {
            List<YH_DeviceControlInfo> list = new List<YH_DeviceControlInfo>();
            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    YH_DeviceControlInfo info = new YH_DeviceControlInfo();

                    info.Ip = dr["HTTPS接入方式IP"].ToString();
                    info.Port = dr["HTTPS接入方式端口"].ToString().ToInt32();
                    info.Appid = dr["项目应用ID"].ToString();
                    info.Appsecret = dr["项目应用密钥"].ToString();
                    info.Serviceid = dr["服务名称"].ToString();
                    info.Deviceid = dr["设备ID"].ToString();
                    info.Cmd = dr["命令名称"].ToString();
                    info.Attribute = dr["命令字段属性名称"].ToString();
                    info.Ifint = dr["上报数据是否是数字"].ToString().ToBoolean();
                    info.Data = dr["上报数据内容"].ToString();
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
        public CommonResult SaveList(List<YH_DeviceControlInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<YH_DeviceControl>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        foreach (YH_DeviceControlInfo detail in list)
                        {
                            //修改部分信息
                            OnBeforeInsert(detail);

                            //在此判断插入数据条件
                            //var isExist = BLLFactory<YH_DeviceControl>.Instance.IsExistKey("Code", detail.Code, trans);
                            //if (!isExist)
                            {
                                BLLFactory<YH_DeviceControl>.Instance.Insert(detail, trans);
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
        public ActionResult SaveExcelData(List<YH_DeviceControlInfo> list)
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
            List<YH_DeviceControlInfo> list = new List<YH_DeviceControlInfo>();

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
                dr["HTTPS接入方式IP"] = list[i].Ip;
                dr["HTTPS接入方式端口"] = list[i].Port;
                dr["项目应用ID"] = list[i].Appid;
                dr["项目应用密钥"] = list[i].Appsecret;
                dr["服务名称"] = list[i].Serviceid;
                dr["设备ID"] = list[i].Deviceid;
                dr["命令名称"] = list[i].Cmd;
                dr["命令字段属性名称"] = list[i].Attribute;
                dr["上报数据是否是数字"] = list[i].Ifint;
                dr["上报数据内容"] = list[i].Data;
                //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/YH_DeviceControl.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(YH_DeviceControlInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
            string t = IOT.NA.naorder.sendOrder(info.Ip, info.Port, info.Appid, info.Appsecret,
                                                info.Serviceid, info.Deviceid, info.Cmd, info.Attribute, info.Ifint, info.Data);
            string p12cert = System.AppDomain.CurrentDomain.BaseDirectory;
            string m = t;

        }

        protected override void OnBeforeUpdate(YH_DeviceControlInfo info)
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
            List<YH_DeviceControlInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<YH_DeviceControlInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (YH_DeviceControlInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.Ip = info.Ip;
                obj.Port = info.Port;
                obj.Appid = info.Appid;
                obj.Appsecret = info.Appsecret;
                obj.Serviceid = info.Serviceid;
                obj.Deviceid = info.Deviceid;
                obj.Cmd = info.Cmd;
                obj.Attribute = info.Attribute;
                obj.Ifint = info.Ifint;
                obj.Data = info.Data;
                //参考转义代码
                //obj.Name = BLLFactory<YH_DeviceControl>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }
    }
}
