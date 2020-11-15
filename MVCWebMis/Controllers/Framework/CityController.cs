using System;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Aspose.Cells;
using Newtonsoft.Json;
using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.BLL;
using IOT.MVCWebMis.Entity;
using WHC.Dictionary.BLL;
using WHC.Dictionary.Entity;
using System.Dynamic;

namespace IOT.MVCWebMis.Controllers
{
    public class CityController : BusinessController<City, CityInfo>
    {
        public CityController() : base()
        {
        }

        #region 导入Excel数据操作
 		 		 		 
	    //导入或导出的字段列表   
        string columnString = "城市名称,邮编,省份ID";

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

            List<CityInfo> list = new List<CityInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    CityInfo info = new CityInfo();
                    
                     info.CityName = dr["城市名称"].ToString();
                      info.ZipCode = dr["邮编"].ToString();
                      info.ProvinceID = dr["省份ID"].ToString().ToInt32();
  
                    //info.Creator = CurrentUser.ID.ToString();
                    //info.CreateTime = DateTime.Now;
                    //info.Editor = CurrentUser.ID.ToString();
                    //info.EditTime = DateTime.Now;

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
        public ActionResult SaveExcelData(List<CityInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                DbTransaction trans = BLLFactory<City>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (CityInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            //detail.CreateTime = DateTime.Now;
                            //detail.Creator = CurrentUser.ID.ToString();
                            //detail.Editor = CurrentUser.ID.ToString();
                            //detail.EditTime = DateTime.Now;

                            BLLFactory<City>.Instance.Insert(detail, trans);
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
            List<CityInfo> list = new List<CityInfo>();

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
                 dr["城市名称"] = list[i].CityName;
                 dr["邮编"] = list[i].ZipCode;
                 dr["省份ID"] = list[i].ProvinceID;
                 //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            } 
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/City.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }
        
        #endregion
		
		#region 写入数据前修改部分属性
        protected override void OnBeforeInsert(CityInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;
            
            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(CityInfo info)
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
            List<CityInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //使用动态对象获得数据，返回给页面使用
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (CityInfo info in list)
            {
                dynamic obj = new ExpandoObject();
                obj.CityName = info.CityName;
                obj.ID = info.ID;
                obj.ZipCode = info.ZipCode;
                obj.ProvinceID = info.ProvinceID;
                obj.ProvinceName = BLLFactory<Province>.Instance.GetNameByID(info.ProvinceID);

                objList.Add(obj);
            }

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = objList };
            return ToJsonContent(result);
        }

        #region 基于Bootstrap的树及字典列表
        /// <summary>
        /// 获取所有的省份和城市列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllProvinceCityJsTree()
        {
            List<JsTreeData> treeList = new List<JsTreeData>();
            JsTreeData pNode = new JsTreeData("", "选择记录", "fa fa-home icon-state-warning icon-lg");
            treeList.Add(pNode);

            List<ProvinceInfo> provinceList = BLLFactory<Province>.Instance.GetAll();
            foreach (ProvinceInfo info in provinceList)
            {
                JsTreeData item = new JsTreeData("", info.ProvinceName, "fa fa-file icon-state-warning icon-lg");

                List<CityInfo> cityList = BLLFactory<City>.Instance.GetCitysByProvinceID(info.ID.ToString());
                foreach (CityInfo cityInfo in cityList)
                {
                    JsTreeData subItem = new JsTreeData(cityInfo.ID, cityInfo.CityName, "fa fa-file icon-state-warning icon-lg");
                    item.children.Add(subItem);
                }

                pNode.children.Add(item);
            }
            return ToJsonContent(treeList);
        }
        /// <summary>
        /// 获取所有的省份和城市列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllProvinceCityDictJson()
        {
            List<CListItem> treeList = new List<CListItem>();
            CListItem pNode = new CListItem("选择记录", "");
            treeList.Add(pNode);

            List<ProvinceInfo> provinceList = BLLFactory<Province>.Instance.GetAll();
            foreach (ProvinceInfo info in provinceList)
            {
                CListItem item = new CListItem(info.ProvinceName, "");
                treeList.Add(item);

                List<CityInfo> cityList = BLLFactory<City>.Instance.GetCitysByProvinceID(info.ID.ToString());
                foreach (CityInfo cityInfo in cityList)
                {
                    CListItem subItem = new CListItem(cityInfo.CityName, cityInfo.ID.ToString());
                    treeList.Add(subItem);
                }

            }
            return ToJsonContent(treeList);
        }

        /// <summary>
        /// 根据省份ID获取对应的城市列表
        /// </summary>
        /// <param name="provinceId">省份ID</param>
        /// <returns></returns>
        public ActionResult GetCitysByProvinceIdDictJson(string provinceId)
        {
            #region 常规处理
            //List<CListItem> treeList = new List<CListItem>();
            //CListItem pNode = new CListItem("选择记录", "");
            //treeList.Add(pNode);

            //if (!string.IsNullOrEmpty(provinceId))
            //{
            //    List<CityInfo> cityList = BLLFactory<City>.Instance.GetCitysByProvinceID(provinceId);
            //    foreach (CityInfo info in cityList)
            //    {
            //        CListItem item = new CListItem(info.CityName, info.ID.ToString());
            //        treeList.Add(item);
            //    }
            //}

            //return ToJsonContent(treeList);
            #endregion

            #region 使用MemoryCache缓存提高速度
            System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
            string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, provinceId);

            var result = MemoryCacheHelper.GetCacheItem<ActionResult>(key,
                delegate()
                {
                    List<CListItem> treeList = new List<CListItem>();
                    CListItem pNode = new CListItem("选择记录", "");
                    treeList.Add(pNode);

                    if (!string.IsNullOrEmpty(provinceId))
                    {
                        List<CityInfo> cityList = BLLFactory<City>.Instance.GetCitysByProvinceID(provinceId);
                        foreach (CityInfo info in cityList)
                        {
                            CListItem item = new CListItem(info.CityName, info.ID.ToString());
                            treeList.Add(item);
                        }
                    }

                    return ToJsonContent(treeList);
                },
                new TimeSpan(0, 30, 0));//30分钟过期

            return result;
            #endregion
        }

        /// <summary>
        /// 根据省份名称获取对应的城市列表
        /// </summary>
        /// <param name="provinceName">省份名称</param>
        /// <returns></returns>
        public ActionResult GetCitysByProvinceNameDictJson(string provinceName)
        {
            List<CListItem> treeList = new List<CListItem>();
            CListItem pNode = new CListItem("选择记录", "");
            treeList.Add(pNode);

            if (!string.IsNullOrEmpty(provinceName))
            {
                List<CityInfo> cityList = BLLFactory<City>.Instance.GetCitysByProvinceName(provinceName);
                foreach (CityInfo info in cityList)
                {
                    CListItem item = new CListItem(info.CityName, info.CityName.ToString());
                    treeList.Add(item);
                }
            }

            return ToJsonContent(treeList);
        }  
        #endregion

        /// <summary>
        /// 获取城市名称
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns></returns>
        public ActionResult GetName(string id)
        {
            string CityName = baseBLL.GetFieldValue(id, "CityName");
            return ToJsonContent(CityName);
        }
    }
}
