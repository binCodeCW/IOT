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
using System.Data.SqlClient;

namespace IOT.MVCWebMis.Controllers
{
    public class YH08_DeviceInfoRecController : BusinessController<YH08_DeviceInfoRec, YH08_DeviceInfoRecInfo>
    {
        public YH08_DeviceInfoRecController() : base()
        {
        }

        #region 导入Excel数据操作
 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 
	    //导入或导出的字段列表   
        string columnString = "时间戳,时刻,应用ID,服务ID,设备ID,质控第一次DOB,质控第二次DOB,质控第三次DOB,质控第四次DOB,质控第五次DOB,质控第六次DOB,质控第七次DOB,质控第八次DOB,质控第九次DOB,质控第十次DOB,仪器编号,参数P,参数T,压力,碳12曲线系数1,碳12曲线系数2,碳12曲线系数3,碳12曲线系数4,碳12曲线系数5,碳12曲线系数6,碳13曲线系数1,碳13曲线系数2,碳13曲线系数3,碳13曲线系数4,碳13曲线系数5,碳13曲线系数6,平均值,标准方差,记录时间";

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
        public List<YH08_DeviceInfoRecInfo> GetExcelList(string guid)
        {
            List<YH08_DeviceInfoRecInfo> list = new List<YH08_DeviceInfoRecInfo>();
            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;                    
                    YH08_DeviceInfoRecInfo info = new YH08_DeviceInfoRecInfo();
                    
                     info.Timestamp = dr["时间戳"].ToString();
                      converted = DateTime.TryParse(dr["时刻"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                         info.Stamptime = dt;
                    }
                      info.Appid = dr["应用ID"].ToString();
                      info.Serviceid = dr["服务ID"].ToString();
                      info.Deviceid = dr["设备ID"].ToString();
                      info.Dob0 = dr["质控第一次DOB"].ToString();
                      info.Dob1 = dr["质控第二次DOB"].ToString();
                      info.Dob2 = dr["质控第三次DOB"].ToString();
                      info.Dob3 = dr["质控第四次DOB"].ToString();
                      info.Dob4 = dr["质控第五次DOB"].ToString();
                      info.Dob5 = dr["质控第六次DOB"].ToString();
                      info.Dob6 = dr["质控第七次DOB"].ToString();
                      info.Dob7 = dr["质控第八次DOB"].ToString();
                      info.Dob8 = dr["质控第九次DOB"].ToString();
                      info.Dob9 = dr["质控第十次DOB"].ToString();
                      info.DeviceNo = dr["仪器编号"].ToString();
                      info.P = dr["参数P"].ToString();
                      info.T = dr["参数T"].ToString();
                      info.PressType = dr["压力"].ToString();
                      info.M_lCfC12_0 = dr["碳12曲线系数1"].ToString();
                      info.M_lCfC12_1 = dr["碳12曲线系数2"].ToString();
                      info.M_lCfC12_2 = dr["碳12曲线系数3"].ToString();
                      info.M_lCfC12_3 = dr["碳12曲线系数4"].ToString();
                      info.M_lCfC12_4 = dr["碳12曲线系数5"].ToString();
                      info.M_lCfC12_5 = dr["碳12曲线系数6"].ToString();
                      info.M_lCfC13_0 = dr["碳13曲线系数1"].ToString();
                      info.M_lCfC13_1 = dr["碳13曲线系数2"].ToString();
                      info.M_lCfC13_2 = dr["碳13曲线系数3"].ToString();
                      info.M_lCfC13_3 = dr["碳13曲线系数4"].ToString();
                      info.M_lCfC13_4 = dr["碳13曲线系数5"].ToString();
                      info.M_lCfC13_5 = dr["碳13曲线系数6"].ToString();
                      info.Aver = dr["平均值"].ToString();
                      info.Se = dr["标准方差"].ToString();
                      converted = DateTime.TryParse(dr["记录时间"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                         info.Recordtime = dt;
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
        public CommonResult SaveList(List<YH08_DeviceInfoRecInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<YH08_DeviceInfoRec>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        foreach (YH08_DeviceInfoRecInfo detail in list)
                        {
                            //修改部分信息
                            OnBeforeInsert(detail);

                            //在此判断插入数据条件
                            //var isExist = BLLFactory<YH08_DeviceInfoRec>.Instance.IsExistKey("Code", detail.Code, trans);
                            //if (!isExist)
                            {
                                BLLFactory<YH08_DeviceInfoRec>.Instance.Insert(detail, trans);
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
        public ActionResult SaveExcelData(List<YH08_DeviceInfoRecInfo> list)
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
            List<YH08_DeviceInfoRecInfo> list = new List<YH08_DeviceInfoRecInfo>();

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
                 dr["时间戳"] = list[i].Timestamp;
                 dr["时刻"] = list[i].Stamptime;
                 dr["应用ID"] = list[i].Appid;
                 dr["服务ID"] = list[i].Serviceid;
                 dr["设备ID"] = list[i].Deviceid;
                 dr["质控第一次DOB"] = list[i].Dob0;
                 dr["质控第二次DOB"] = list[i].Dob1;
                 dr["质控第三次DOB"] = list[i].Dob2;
                 dr["质控第四次DOB"] = list[i].Dob3;
                 dr["质控第五次DOB"] = list[i].Dob4;
                 dr["质控第六次DOB"] = list[i].Dob5;
                 dr["质控第七次DOB"] = list[i].Dob6;
                 dr["质控第八次DOB"] = list[i].Dob7;
                 dr["质控第九次DOB"] = list[i].Dob8;
                 dr["质控第十次DOB"] = list[i].Dob9;
                 dr["仪器编号"] = list[i].DeviceNo;
                 dr["参数P"] = list[i].P;
                 dr["参数T"] = list[i].T;
                 dr["压力"] = list[i].PressType;
                 dr["碳12曲线系数1"] = list[i].M_lCfC12_0;
                 dr["碳12曲线系数2"] = list[i].M_lCfC12_1;
                 dr["碳12曲线系数3"] = list[i].M_lCfC12_2;
                 dr["碳12曲线系数4"] = list[i].M_lCfC12_3;
                 dr["碳12曲线系数5"] = list[i].M_lCfC12_4;
                 dr["碳12曲线系数6"] = list[i].M_lCfC12_5;
                 dr["碳13曲线系数1"] = list[i].M_lCfC13_0;
                 dr["碳13曲线系数2"] = list[i].M_lCfC13_1;
                 dr["碳13曲线系数3"] = list[i].M_lCfC13_2;
                 dr["碳13曲线系数4"] = list[i].M_lCfC13_3;
                 dr["碳13曲线系数5"] = list[i].M_lCfC13_4;
                 dr["碳13曲线系数6"] = list[i].M_lCfC13_5;
                 dr["平均值"] = list[i].Aver;
                 dr["标准方差"] = list[i].Se;
                 dr["记录时间"] = list[i].Recordtime;
                 //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            } 
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/YH08_DeviceInfoRec.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }
        
        #endregion
		
        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(YH08_DeviceInfoRecInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;
            
            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(YH08_DeviceInfoRecInfo info)
        {
            //子类对参数对象进行修改
            //info.Editor = CurrentUser.ID.ToString();
            //info.EditTime = DateTime.Now;
        }
        #endregion

        /// <summary>
        /// 指定设备编号及时间段的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DeviceInfo1(int? deviceNo, DateTime? startTime, DateTime? endTime)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            if (!deviceNo.HasValue)
            {
                throw new Exception(message: "参数 deviceNo 不能为空");
            }

            if (!startTime.HasValue)
            {
                throw new Exception(message: "参数 startTime 不能为空");
            }

            if (!endTime.HasValue)
            {
                throw new Exception(message: "参数 endTime 不能为空");
            }

            string sql = @"
select * from  YH08_DeviceInfoRec t
where 1 = 1
and t.DeviceNo = @DEVICENO
and t.Stamptime between @STARTTIME and @ENDTIME 
order by t.Stamptime asc
";
            //DataTable dt = baseBLL.SqlTable(sql: sql);
            //return ToJsonContent(obj: dt);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(item: new SqlParameter(parameterName: "DEVICENO", value: deviceNo));
            parameters.Add(item: new SqlParameter(parameterName: "STARTTIME", value: startTime));
            parameters.Add(item: new SqlParameter(parameterName: "ENDTIME", value: endTime));
            DataTable dt = baseBLL.SqlTable(sql: sql, parameters: parameters.ToArray());
            return ToJsonContent(obj: dt);
        }

        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();			
            List<YH08_DeviceInfoRecInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<YH08_DeviceInfoRecInfo> list)
        {
			//如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach(YH08_DeviceInfoRecInfo info in list)
            {
                dynamic obj = new ExpandoObject();
			
                obj.ID = info.ID;
                 obj.Timestamp = info.Timestamp;
                 obj.Stamptime = info.Stamptime;
                 obj.Appid = info.Appid;
                 obj.Serviceid = info.Serviceid;
                 obj.Deviceid = info.Deviceid;
                 obj.Dob0 = info.Dob0;
                 obj.Dob1 = info.Dob1;
                 obj.Dob2 = info.Dob2;
                 obj.Dob3 = info.Dob3;
                 obj.Dob4 = info.Dob4;
                 obj.Dob5 = info.Dob5;
                 obj.Dob6 = info.Dob6;
                 obj.Dob7 = info.Dob7;
                 obj.Dob8 = info.Dob8;
                 obj.Dob9 = info.Dob9;
                 obj.DeviceNo = info.DeviceNo;
                 obj.P = info.P;
                 obj.T = info.T;
                 obj.PressType = info.PressType;
                 obj.M_lCfC12_0 = info.M_lCfC12_0;
                 obj.M_lCfC12_1 = info.M_lCfC12_1;
                 obj.M_lCfC12_2 = info.M_lCfC12_2;
                 obj.M_lCfC12_3 = info.M_lCfC12_3;
                 obj.M_lCfC12_4 = info.M_lCfC12_4;
                 obj.M_lCfC12_5 = info.M_lCfC12_5;
                 obj.M_lCfC13_0 = info.M_lCfC13_0;
                 obj.M_lCfC13_1 = info.M_lCfC13_1;
                 obj.M_lCfC13_2 = info.M_lCfC13_2;
                 obj.M_lCfC13_3 = info.M_lCfC13_3;
                 obj.M_lCfC13_4 = info.M_lCfC13_4;
                 obj.M_lCfC13_5 = info.M_lCfC13_5;
                 obj.Aver = info.Aver;
                 obj.Se = info.Se;
                 obj.Recordtime = info.Recordtime;
 				//参考转义代码
				//obj.Name = BLLFactory<YH08_DeviceInfoRec>.Instance.GetNameByID(info.ID);
                
                objList.Add(obj);
            } 
			
            return objList;
		}
    }
}
