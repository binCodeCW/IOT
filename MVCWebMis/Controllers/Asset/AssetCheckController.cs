using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.Entity;
using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;
using WHC.Dictionary.BLL;
using YH.Security.BLL;
using Autofac;
using WHC.Common.Handler;

namespace IOT.MVCWebMis.Controllers
{
    public class AssetCheckController : BusinessController<AssetCheck, AssetCheckInfo>
    {
        public AssetCheckController() : base()
        {
        }

        #region 导入Excel数据操作

        //导入或导出的字段列表   
        string columnString = "盘点单号,盘点资产,盘点部门,盘点公司,盘点数量,已盘数量,未盘数量,单据状态,任务状态,申请日期,备注,创建人,创建时间";

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
        public List<AssetCheckInfo> GetExcelList(string guid)
        {
            List<AssetCheckInfo> list = new List<AssetCheckInfo>();
            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;
                    AssetCheckInfo info = new AssetCheckInfo();

                    info.BillNo = dr["盘点单号"].ToString();
                    info.Dept_ID = dr["盘点部门"].ToString();
                    info.Company_ID = dr["盘点公司"].ToString();
                    info.CheckQty = dr["盘点数量"].ToString().ToInt32();
                    info.DoneQty = dr["已盘数量"].ToString().ToInt32();
                    info.TodoQty = dr["未盘数量"].ToString().ToInt32();
                    info.CheckStatus = dr["单据状态"].ToString().ToInt32();
                    info.TaskStatus = dr["任务状态"].ToString().ToInt32();
                    converted = DateTime.TryParse(dr["申请日期"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                        info.ApplyDate = dt;
                    }
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
            return list;
        }

        /// <summary>
        /// 把列表批量写入数据库
        /// </summary>
        /// <param name="list">保存的列表</param>
        /// <returns></returns>
        public CommonResult SaveList(List<AssetCheckInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交
                using (DbTransaction trans = BLLFactory<AssetCheck>.Instance.CreateTransaction())
                {
                    if (trans != null)
                    {
                        try
                        {
                            foreach (AssetCheckInfo detail in list)
                            {
                                //修改部分信息
                                OnBeforeInsert(detail);

                                //在此判断插入数据条件
                                //var isExist = BLLFactory<AssetCheck>.Instance.IsExistKey("Code", detail.Code, trans);
                                //if (!isExist)
                                {
                                    BLLFactory<AssetCheck>.Instance.Insert(detail, trans);
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
        public ActionResult SaveExcelData(List<AssetCheckInfo> list)
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
            List<AssetCheckInfo> list = new List<AssetCheckInfo>();

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
                dr["盘点部门"] = list[i].Dept_ID;
                dr["盘点公司"] = list[i].Company_ID;
                dr["盘点数量"] = list[i].CheckQty;
                dr["已盘数量"] = list[i].DoneQty;
                dr["未盘数量"] = list[i].TodoQty;
                dr["单据状态"] = list[i].CheckStatus;
                dr["任务状态"] = list[i].TaskStatus;
                dr["申请日期"] = list[i].ApplyDate;
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
            string filePath = string.Format("/GenerateFiles/{0}/AssetCheck.xls", CurrentUser.Name);
            GenerateExcel(datatable, filePath);

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }

        #endregion

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(AssetCheckInfo info)
        {
            //info.ID = string.IsNullOrEmpty(info.ID) ? Guid.NewGuid().ToString() : info.ID;

            //子类对参数对象进行修改

            //盘点单据状态 未完成：0、已完成：1
            info.CheckStatus = 0;
            //任务状态 建立：0、下发：1、已回传:2
            info.TaskStatus = 0;
            info.ApplyDate = DateTime.Now;

            info.CreateTime = DateTime.Now;
            info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(AssetCheckInfo info)
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
            List<AssetCheckInfo> list = null;
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
        protected override ExpandoObject ConvertEntityToShow(AssetCheckInfo info)
        {
            dynamic obj = new ExpandoObject();

            obj.ID = info.ID;
            obj.BillNo = info.BillNo;
            obj.Dept_ID = info.Dept_ID;
            obj.DeptName = SecurityHelper.GetDeptNameByID(info.Dept_ID);
            obj.Company_ID = info.Company_ID;
            obj.CompanyName = SecurityHelper.GetDeptNameByID(info.Company_ID);
            obj.CheckQty = info.CheckQty;
            obj.DoneQty = info.DoneQty;
            obj.TodoQty = info.TodoQty;
            obj.CheckStatus = BLLFactory<DictData>.Instance.GetDictName("盘点单据状态", info.CheckStatus.ToString());//info.CheckStatus;
            obj.TaskStatus = BLLFactory<DictData>.Instance.GetDictName("盘点任务状态", info.TaskStatus.ToString()); //info.TaskStatus;
            obj.ApplyDate = info.ApplyDate;
            obj.Note = info.Note;
            obj.Creator = info.Creator;
            obj.CreateTime = info.CreateTime;
            obj.Editor = info.Editor;
            obj.EditTime = info.EditTime;
            obj.CheckPerson = string.IsNullOrEmpty(info.CheckPerson) ? "未指定" : info.CheckPerson;
            //参考转义代码
            //obj.Name = BLLFactory<AssetCheck>.Instance.GetNameByID(info.ID);

            return obj;
        }

        /// <summary>
        /// 批量生成某部门下的所有资产盘点单据
        /// </summary>
        /// <returns></returns>
        public ActionResult BatchAdd(string companyid, string deptid)
        {
            CommonResult result = new CommonResult();
            if (string.IsNullOrEmpty(companyid))
            {
                result.ErrorMessage = "公司ID不能为空";
            }
            else
            {
                //组装条件
                string condition = string.Format("Company_ID='{0}'", companyid);
                if (!string.IsNullOrEmpty(deptid))
                {
                    condition += string.Format(" AND Dept_ID='{0}'", deptid);
                }

                //获取部门列表，根据部门生成主表记录
                string sql = string.Format(@"SELECT DISTINCT Dept_ID FROM T_Asset Where {0}", condition);
                string valueList = baseBLL.SqlValueList(sql);
                List<string> deptIdList = new List<string>();
                foreach (string item in valueList.ToDelimitedList<string>(","))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        deptIdList.Add(item);
                    }
                }

                //生成任务
                result = GenerateBill(companyid, deptIdList);
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 生成任务
        /// </summary>
        /// <param name="companyid">公司ID</param>
        /// <param name="deptIdList">部门ID</param>
        /// <returns></returns>
        private CommonResult GenerateBill(string companyid, List<string> deptIdList)
        {
            CommonResult result = new CommonResult();
            DbTransaction trans = null;
            try
            {
                trans = BLLFactory<AssetCheck>.Instance.CreateTransaction();
                if (trans != null)
                {
                    foreach (string deptId in deptIdList)
                    {
                        GenerateBill(companyid, deptId, trans);
                    }

                    trans.Commit();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                    LogHelper.Error(ex);
                    result.Success = false;
                }
            }
            return result;
        }

        /// <summary>
        /// 生成一个单据
        /// </summary>
        /// <param name="companyid">公司ID</param>
        /// <param name="deptid">部门ID</param>
        /// <param name="asset">盘点资产</param>
        /// <param name="className">资产类别</param>
        private void GenerateBill(string companyid, string deptid, DbTransaction trans = null)
        {
            string condition = string.Format("Company_ID='{0}'", companyid);
            if (!string.IsNullOrEmpty(deptid))
            {
                condition += string.Format(" AND Dept_ID='{0}'", deptid);
            }

            var info = new AssetCheckInfo();
            info.Company_ID = companyid;
            info.Dept_ID = deptid;
            info.ApplyDate = DateTime.Now;
            info.BillNo = BLLFactory<AssetCheck>.Instance.GetBillNo(CurrentUser.ID, trans);
            //获取指定条件的资产总数
            info.CheckQty = BLLFactory<Asset>.Instance.GetAssetQty(condition, trans);//盘点数量
            OnBeforeInsert(info);

            bool flag = BLLFactory<AssetCheck>.Instance.Insert(info, trans);
            if (flag)
            {
                //指定公司部门下的资产
                var list = BLLFactory<Asset>.Instance.Find(condition, trans);
                foreach (var obj in list)
                {
                    var detailInfo = new AssetCheckDetailInfo();
                    detailInfo.BillNo = info.BillNo;
                    detailInfo.AssetCode = obj.Code;
                    detailInfo.AssetName = obj.Name;
                    detailInfo.UsePerson = obj.UsePerson;
                    detailInfo.KeepAddr = obj.KeepAddr;
                    detailInfo.Qty = obj.Qty;
                    detailInfo.CurrDept = obj.CurrDept;

                    detailInfo.CreateTime = DateTime.Now;
                    detailInfo.Creator = CurrentUser.ID.ToString();
                    BLLFactory<AssetCheckDetail>.Instance.Insert(detailInfo, trans);
                }
            }
        }

        /// <summary>
        /// 重载删除操作，删除关联明细（根据BillNo进行删除）
        /// </summary>
        /// <param name="ids">BillNo的字符串，多个用逗号分开</param>
        /// <returns></returns>
        public override ActionResult DeleteByIds(string ids)
        {
            CommonResult result = new CommonResult();
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    #region 事务处理删除
                    List<string> idArray = ids.ToDelimitedList<string>(",");
                    using (var trans = BLLFactory<AssetCheck>.Instance.CreateTransaction())
                    {
                        if (trans != null)
                        {
                            try
                            {
                                foreach (string billNo in idArray)
                                {
                                    if (!string.IsNullOrEmpty(billNo))
                                    {
                                        string condition = string.Format("BillNo='{0}'", billNo);
                                        //删除主表
                                        baseBLL.DeleteByCondition(condition, trans);
                                        //删除明细
                                        BLLFactory<AssetCheckDetail>.Instance.DeleteByCondition(condition, trans);
                                    }
                                }

                                trans.Commit();
                                result.Success = true;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                LogHelper.Error(ex);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    result.ErrorMessage = "单号不能为空";
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 修改盘点主表和明细表的盘点用户
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="billno">盘点单据号</param>
        /// <returns></returns>
        public ActionResult EditCheckUser(string userid, string billno)
        {
            CommonResult result = new CommonResult();
            var userInfo = BLLFactory<User>.Instance.FindByID(userid);
            if (userInfo != null && !string.IsNullOrWhiteSpace(userInfo.CorpUserId))
            {
                string condition = string.Format("BillNo='{0}'", billno);
                Hashtable ht = new Hashtable();
                ht.Add("CorpUserId", userInfo.CorpUserId);
                ht.Add("CheckPerson", userInfo.Name);

                //修改盘点主表和明细表的盘点用户
                BLLFactory<AssetCheck>.Instance.UpdateFieldsByCondition(ht, condition);
                BLLFactory<AssetCheckDetail>.Instance.UpdateFieldsByCondition(ht, condition);
                result.Success = true;
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 批量处理多个任务下发企业微信
        /// </summary>
        /// <param name="billNo">多个billno组成的列表</param>
        /// <returns></returns>
        public ActionResult SendTask(string billNoList)
        {
            CommonResult result = new CommonResult();
            try
            {
                if (!string.IsNullOrEmpty(billNoList))
                {
                    //获取对应的企业微信消息推送接口
                    var handler = AutoFactory.Instatnce.Container.Resolve<ICorpMessage>();
                    if (handler != null)
                    {
                        //把逗号分隔的字符串转换为列表
                        List<string> list = billNoList.ToDelimitedList<string>(",");
                        foreach (string billNo in list)
                        {
                            //获取盘点主表信息
                            AssetCheckInfo info = BLLFactory<AssetCheck>.Instance.FindByBillNo(billNo);
                            if (info != null)
                            {
                                //获取盘点明细~信息~
                                var detailList = BLLFactory<AssetCheckDetail>.Instance.FindByBillNo(billNo);

                                //读取配置信息
                                AppConfig config = new AppConfig();
                                handler.CorpId = config.AppConfigGet("CorpId");
                                handler.CorpSecret = config.AppConfigGet("CorpSecret");
                                handler.AgentId = config.AppConfigGet("AgentId");

                                //构建推送的消息体内容
                                string touser = info.CorpUserId;
                                string title = "您有一个盘点任务待处理";
                                StringBuilder sb = new StringBuilder();
                                sb.AppendFormat("盘点单号:{0}\r\n", info.BillNo);
                                //sb.AppendFormat("盘点公司:{0}\r\n", info.Company_ID);
                                //sb.AppendFormat("盘点部门:{0}\r\n", info.Dept_ID);
                                sb.AppendFormat("盘点数量:{0}\r\n", info.CheckQty);
                                sb.AppendFormat("指定盘点人:{0}\r\n", info.CorpUserId);
                                sb.AppendFormat("申请日期:{0}\r\n", info.ApplyDate.ToString("yyyy-MM-dd"));
                                string description = sb.ToString();
                                string url = "http://www.yh-medical.com/";

                                //调用企业微信消息接口推送消息
                                /*20200228注释
                                 *  result = handler.SendMessageTextCard(touser, title, description, url);
                                if (result.Success)
                                {
                                    //更新盘点表状态
                                    Hashtable ht = new Hashtable();
                                    ht.Add("TaskStatus", 1);//下发 1
                                    BLLFactory<AssetCheck>.Instance.UpdateFields(ht, info.ID);
                                }
                                */

                            }
                        }
                    }
                }
                else
                {
                    result.ErrorMessage = "单号为空";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                result.ErrorMessage = ex.Message;
            }

            return ToJsonContent(result);
        }
    }
}
