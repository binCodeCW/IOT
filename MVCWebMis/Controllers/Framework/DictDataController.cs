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

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// 数据字典控制器
    /// </summary>
    public class DictDataController : BusinessController<DictData, DictDataInfo>
    {
        public DictDataController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "字典大类,字典名称,字典值,备注,排序";

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
        /// 根据文件的GUID转换为强类型对象列表
        /// </summary>
        /// <param name="guid">上传的文件GUID</param>
        /// <returns></returns>
        private List<DictDataInfo> GetExcelList(string guid)
        {
            List<DictDataInfo> list = new List<DictDataInfo>();
            if (!string.IsNullOrEmpty(guid))
            {
                DataTable table = ConvertExcelFileToTable(guid);
                if (table != null)
                {
                    #region 数据转换
                    foreach (DataRow dr in table.Rows)
                    {
                        bool converted = false;
                        DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                        DateTime dt;

                        string typeName = dr["字典大类"].ToString();
                        if (!string.IsNullOrEmpty(typeName))
                        {
                            DictTypeInfo typeInfo = BLLFactory<DictType>.Instance.FindSingle(string.Format("Name ='{0}'", typeName));
                            if (typeInfo != null)
                            {
                                DictDataInfo info = new DictDataInfo();
                                info.DictType_ID = typeInfo.ID;
                                info.Data1 = typeInfo.Name;//传递给界面显示名称

                                info.Name = dr["字典名称"].ToString();
                                info.Value = dr["字典值"].ToString();
                                info.Remark = dr["备注"].ToString();
                                info.Seq = dr["排序"].ToString();

                                info.Editor = CurrentUser.ID.ToString();
                                info.LastUpdated = DateTime.Now;

                                list.Add(info);
                            }
                        }
                    }
                    #endregion
                }
            }
            return list;
        }

        /// <summary>
        /// 获取服务器上的Excel文件，并把它转换为实体列表返回给客户端
        /// </summary>
        /// <param name="guid">附件的GUID</param>
        /// <returns></returns>
        public ActionResult GetExcelData(string guid)
        {
            var list = GetExcelList(guid);
            var result = new { total = list.Count, rows = list };
            return ToJsonContent(result);
        }

        /// <summary>
        /// 把列表批量写入数据库
        /// </summary>
        /// <param name="list">保存的列表</param>
        /// <returns></returns>
        public CommonResult SaveList(List<DictDataInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                var trans = BLLFactory<DictData>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        foreach (var detail in list)
                        {
                            //修改部分信息
                            OnBeforeInsert(detail);
                            BLLFactory<DictData>.Instance.Insert(detail, trans);
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
        /// 保存客户端上传的相关数据列表
        /// </summary>
        /// <param name="list">数据列表</param>
        /// <returns></returns>
        public ActionResult SaveExcelData(List<DictDataInfo> list)
        {
            var result = SaveList(list);
            return ToJsonContent(result);
        }

        /// <summary>
        /// 在服务端保存Excel
        /// </summary>
        /// <param name="guid">上传文件的GUID</param>
        /// <returns></returns>
        public ActionResult SaveExcelByGuid(string guid)
        {
            CommonResult result = new CommonResult();
            var list = GetExcelList(guid);
            if (list != null && list.Count > 0)
            {
                result = SaveList(list);
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
            List<DictDataInfo> list = new List<DictDataInfo>();

            if (!string.IsNullOrWhiteSpace(CustomedCondition))
            {
                //如果为自定义的json参数列表，那么可以使用字典反序列化获取参数，然后处理
                //Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(CustomedCondition);

                //如果是条件的自定义，可以使用Find查找
                list = baseBLL.Find(CustomedCondition, "order by DictType_ID, seq");
            }
            else
            {
                list = baseBLL.Find(where, "order by DictType_ID, seq");
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
                DictTypeInfo typeInfo = BLLFactory<DictType>.Instance.FindByID(list[i].DictType_ID);
                if (typeInfo != null)
                {
                    dr["字典大类"] = typeInfo.Name;
                }
                dr["字典名称"] = list[i].Name;
                dr["字典值"] = list[i].Value;
                dr["备注"] = list[i].Remark;
                dr["排序"] = list[i].Seq;

                datatable.Rows.Add(dr);
            }
            #endregion

            #region 把DataTable转换为Excel并输出

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/DictData.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion


        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(DictDataInfo info)
        {
            //留给子类对参数对象进行修改
            info.Editor = CurrentUser.ID.ToString();
            info.LastUpdated = DateTime.Now;
        }

        protected override void OnBeforeUpdate(DictDataInfo info)
        {
            //留给子类对参数对象进行修改
            info.Editor = CurrentUser.ID.ToString();
            info.LastUpdated = DateTime.Now;
        }
        #endregion

        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();

            List<DictDataInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //如果需要修改字段显示，则参考下面代码处理
            //foreach(DictDataInfo info in list)
            //{
            //    info.PID = BLLFactory<DictData>.Instance.GetFieldValue(info.PID, "Name");
            //    if (!string.IsNullOrEmpty(info.Creator))
            //    {
            //        info.Creator = BLLFactory<User>.Instance.GetFullNameByID(info.Creator.ToInt32());
            //    }
            //}

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = list };
            return ToJsonContent(result);
        }

        /// <summary>
        /// 批量添加字典数据操作
        /// </summary>
        /// <param name="DictType_ID">字典类型</param>
        /// <param name="Seq">排序开始或前缀</param>
        /// <param name="Data">批量插入的内容</param>
        /// <param name="SplitType">分开类型，分隔符分开（Split）还是行分割（Line）</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>
        public ActionResult BatchInsert(string DictType_ID, string Seq, string Data, string SplitType, string Remark)
        {
            CommonResult result = new CommonResult();
            if (string.IsNullOrEmpty(DictType_ID) || string.IsNullOrEmpty(Data))
            {
                result.ErrorMessage = "DictType_ID或Data参数为空";
                return ToJsonContent(result);
            }

            string[] arrayItems = Data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int intSeq = -1;
            int seqLength = 3;
            string strSeq = Seq;
            if (int.TryParse(strSeq, out intSeq))
            {
                seqLength = strSeq.Length;
            }

            if (arrayItems != null && arrayItems.Length > 0)
            {
                DbTransaction trans = BLLFactory<DictData>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        #region MyRegion
                        foreach (string strItem in arrayItems)
                        {
                            if (SplitType.Equals("split", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!string.IsNullOrWhiteSpace(strItem))
                                {
                                    string[] dataItems = strItem.Split(new char[] { ',', '，', ';', '；', '/', '、' });
                                    foreach (string dictData in dataItems)
                                    {
                                        #region 保存数据
                                        string seq = "";
                                        if (intSeq > 0)
                                        {
                                            seq = (intSeq++).ToString().PadLeft(seqLength, '0');
                                        }
                                        else
                                        {
                                            seq = string.Format("{0}{1}", strSeq, intSeq++);
                                        }

                                        InsertDictData(DictType_ID, dictData, seq, Remark, trans);
                                        #endregion
                                    }
                                }
                            }
                            else
                            {
                                #region 保存数据
                                if (!string.IsNullOrWhiteSpace(strItem))
                                {
                                    string seq = "";
                                    if (intSeq > 0)
                                    {
                                        seq = (intSeq++).ToString().PadLeft(seqLength, '0');
                                    }
                                    else
                                    {
                                        seq = string.Format("{0}{1}", strSeq, intSeq++);
                                    }

                                    InsertDictData(DictType_ID, strItem, seq, Remark, trans);
                                }
                                #endregion
                            }
                        }
                        #endregion

                        trans.Commit();
                        result.Success = true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        LogTextHelper.Error(ex);
                        result.ErrorMessage = ex.Message;
                    }
                }
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 使用事务参数，插入数据，最后统一提交事务处理
        /// </summary>
        /// <param name="dictData">字典数据</param>
        /// <param name="seq">排序</param>
        /// <param name="trans">事务对象</param>
        private void InsertDictData(string dictTypeId, string dictData, string seq, string note, DbTransaction trans)
        {
            if (!string.IsNullOrWhiteSpace(dictData))
            {
                DictDataInfo info = new DictDataInfo();
                info.Editor = CurrentUser.ID.ToString();
                info.LastUpdated = DateTime.Now;
                info.DictType_ID = dictTypeId;

                var array = System.Text.RegularExpressions.Regex.Split(dictData, @"\s+");
                info.Name = array[0];
                info.Value = array.Length > 1 ? array[1] : array[0];

                //info.Name = dictData.Trim();
                //info.Value = dictData.Trim();
                info.Remark = note;
                info.Seq = seq;

                bool succeed = BLLFactory<DictData>.Instance.Insert(info, trans);
            }
        }

        /// <summary>
        /// 根据字典类型获取对应的字典数据，方便UI控件的绑定
        /// </summary>
        /// <param name="dictTypeName">字典类型名称</param>
        /// <returns></returns>
        public ActionResult GetDictJson(string dictTypeName)
        {
            #region 常规处理
            //List<CListItem> treeList = new List<CListItem>();
            //CListItem pNode = new CListItem("", "");
            //treeList.Insert(0, pNode);

            //Dictionary<string, string> dict = BLLFactory<DictData>.Instance.GetDictByDictType(dictTypeName);
            //foreach (string dictKey in dict.Keys)
            //{
            //    treeList.Add(new CListItem(key, dict[dictKey]));
            //}
            //return ToJsonContent(treeList); 
            #endregion

            #region 使用MemoryCache缓存提高速度
            System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
            string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, dictTypeName);

            var result = MemoryCacheHelper.GetCacheItem<ActionResult>(key,
                delegate()
                {
                    List<CListItem> treeList = new List<CListItem>();
                    CListItem pNode = new CListItem("", "");
                    treeList.Insert(0, pNode);

                    Dictionary<string, string> dict = BLLFactory<DictData>.Instance.GetDictByDictType(dictTypeName);
                    foreach (string dictKey in dict.Keys)
                    {
                        treeList.Add(new CListItem(dictKey, dict[dictKey]));
                    }
                    return ToJsonContent(treeList);
                },
                new TimeSpan(0, 30, 0));//30分钟过期

            return result; 
            #endregion
        }

        #region BootStrap JSTree的树形列表

        /// <summary>
        /// 获取树形展示数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJsTreeJson(string dictTypeName)
        {
            #region 使用MemoryCache缓存提高速度
            System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
            string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, dictTypeName);

            var result = MemoryCacheHelper.GetCacheItem<List<JsTreeData>>(key,
                delegate()
                {
                    List<JsTreeData> treeList = new List<JsTreeData>();
                    Dictionary<string, string> dict = BLLFactory<DictData>.Instance.GetDictByDictType(dictTypeName);
                    foreach (string dictKey in dict.Keys)
                    {
                        JsTreeData node = new JsTreeData(dict[dictKey], dictKey, "fa fa-file icon-state-warning icon-lg");
                        treeList.Add(node);
                    }
                    return treeList;
                },
                new TimeSpan(0, 30, 0));//30分钟过期

            #endregion

            return ToJsonContent(result);
        }

        #endregion
    }
}
