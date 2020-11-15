using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.CRM.Entity;
using WHC.CRM.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;
using System.Drawing;
using WHC.Dictionary.BLL;
using WHC.Dictionary.Entity;

namespace WHC.CRM.BLL
{
    /// <summary>
    /// 客户基本资料
    /// </summary>
	public class Customer : BaseBLL<CustomerInfo>
    {
        public Customer() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 清空共享人员记录
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <returns></returns>
        public bool ClearShareUser(string id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ShareUsers", "");
            return baseDal.UpdateFields(ht, id);
        }

        /// <summary>
        /// 在现有基础上增加共享人员列表。
        /// 如果已有人员，则不影响，记录增加差异部分。
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <param name="userList">用户ID列表</param>
        /// <returns></returns>
        public bool AddShareUser(string id, List<string> userList)
        {
            bool result = false;
            if (userList != null && userList.Count > 0)
            {
                List<string> list = userList;//默认为加入的
                string shareUsers = baseDal.GetFieldValue(id, "ShareUsers");
                List<string> oldList = shareUsers.ToDelimitedList<string>(",");

                if (!string.IsNullOrEmpty(shareUsers))
                {
                    list = oldList;//数据库有，则以数据库为准
                    foreach (string userId in userList)
                    {
                        //如果原列表没有包含，则加入
                        if (!oldList.Contains(userId))
                        {
                            list.Add(userId);
                        }
                    }
                }

                //串联为新的用户列表，前后逗号分隔
                string newUsers = string.Join(",", list);
                newUsers = string.Format(",{0},", newUsers);

                //更新整个记录
                Hashtable ht = new Hashtable();
                ht.Add("ShareUsers", newUsers);
                result = baseDal.UpdateFields(ht, id);
            }
            return result;
        }

        /// <summary>
        /// 获取标记颜色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetMarkColor(string id)
        {
            return baseDal.GetFieldValue(id, "MarkColor");
        }

        /// <summary>
        /// 更新标记颜色
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <param name="color">Color字符串表示颜色</param>
        /// <returns></returns>
        public bool MarkColor(string id, string color)
        {
            Hashtable ht = new Hashtable();
            ht.Add("MarkColor", color);
            return baseDal.UpdateFields(ht, id);
        }

        /// <summary>
        /// 修改客户的所属人员/创建人员
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <param name="userId">所属人员ID</param>
        /// <param name="companyId">所属人员的公司ID</param>
        /// <returns></returns>
        public bool ChangeOwner(string id, string userId, string companyId)
        {
            ICustomer dal = baseDal as ICustomer;
            return dal.ChangeOwner(id, userId, companyId);
        }

        /// <summary>
        /// 根据客户分组的名称，搜索属于该分组的客户列表
        /// </summary>
        /// <param name="ownerUser">客户所属用户</param>
        /// <param name="groupName">客户分组的名称,如果客户分组为空，那么返回未分组客户列表</param>
        /// <param name="condition">过滤条件，可以根据区域进行过滤</param>
        /// <param name="pagerInfo">分页条件</param>
        /// <returns></returns>
        public List<CustomerInfo> FindByGroupName(string ownerUser, string groupName, string condition, PagerInfo pagerInfo = null)
        {
            ICustomer dal = baseDal as ICustomer;
            return dal.FindByGroupName(ownerUser, groupName, condition, pagerInfo);
        }

        /// <summary>
        /// 设置删除标志
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <param name="deleted">是否删除</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public bool SetDeletedFlag(string id, bool deleted = true, DbTransaction trans= null)
        {
            ICustomer dal = baseDal as ICustomer;
            return dal.SetDeletedFlag(id, deleted, trans);
        }

        public override bool Delete(object key, DbTransaction trans = null)
        {
            bool deleted = base.Delete(key, trans);
            if (deleted)
            {
                string customerID = key.ToString();
                string condition = string.Format("Customer_ID='{0}' ", customerID);
                BLLFactory<Follow>.Instance.DeleteByCondition(condition);
                BLLFactory<FileData>.Instance.DeleteByCondition(condition);

                this.RemoveSupplier(customerID);
            }
            return deleted;
        }

        /// <summary>
        /// 获取客户的名称
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <returns></returns>
        public string GetCustomerName(string id, DbTransaction trans = null)
        {
            //使用缓存减轻数据库查询压力
            System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
            string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, id);
            string name = MemoryCacheHelper.GetCacheItem<string>(key, delegate()
            {
                return GetFieldValue(id, "Name", trans);
            },
            new TimeSpan(0, 30, 0));//30分钟后过期
            return name;
        }
                      
        /// <summary>
        /// 调整客户的组别
        /// </summary>
        /// <param name="customerId">客户ID</param>
        /// <param name="groupIdList">客户分组Id集合</param>
        /// <returns></returns>
        public bool ModifyCustomerGroup(string customerId, List<string> groupIdList)
        {
            ICustomer dal = baseDal as ICustomer;
            return dal.ModifyCustomerGroup(customerId, groupIdList);
        }

        /// <summary>
        /// 获取客户的省份列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetCustomersProvince(string condition)
        {
            List<string> list = GetFieldListByCondition("Province", condition);
            return list;
        }

        /// <summary>
        /// 获取客户的城市列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetCustomersCity(string condition)
        {
            List<string> list = GetFieldListByCondition("City", condition);
            return list;
        }

        /// <summary>
        /// 根据客户名称获取客户信息
        /// </summary>
        /// <param name="customerName">客户名称</param>
        /// <returns></returns>
        public CustomerInfo FindByName(string customerName)
        {
            string condition = string.Format("Name='{0}' ", customerName);
            return FindSingle(condition);
        }

        /// <summary>
        /// 给SearchLookup控件提供数据源，返回部分客户字段信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllForLookup()
        {
            string sql = string.Format("Select ID,Name,SimpleName,HandNo from T_CRM_Customer order by HandNo");
            return baseDal.SqlTable(sql);
        }

        /// <summary>
        /// 更新客户的状态信息
        /// </summary>
        /// <param name="id">客户Id</param>
        /// <param name="orderDate">订单日期</param>
        /// <param name="orderCount">交易次数</param>
        /// <param name="orderMoney">交易金额</param>
        /// <returns></returns>
        public bool UpdateTransactionStatus(string id, DateTime orderDate, int orderCount, decimal orderMoney, DbTransaction trans = null)
        {
            ICustomer dal = baseDal as ICustomer;
            return dal.UpdateTransactionStatus(id, orderDate, orderCount, orderMoney, trans);
        }

        /// <summary>
        /// 更新客户的最后联系日期
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <param name="lastContactDate">最后联系日期</param>
        /// <returns></returns>
        public bool UpdateContactDate(string id, DateTime lastContactDate, DbTransaction trans = null)
        {
            ICustomer dal = baseDal as ICustomer;
            return dal.UpdateContactDate(id, lastContactDate, trans);
        }
                        
        /// <summary>
        /// 根据供应商ID，分页获取客户列表（关联客户列表）
        /// </summary>
        /// <param name="supplierID">供应商ID</param>
        /// <param name="pagerInfo">分页条件</param>
        /// <returns></returns>
        public List<CustomerInfo> FindBySupplier(string supplierID, string condition, PagerInfo pagerInfo = null)
        {
            ICustomer dal = baseDal as ICustomer;
            return dal.FindBySupplier(supplierID, condition, pagerInfo);
        }

        /// <summary>
        /// 如果没有建立关系，则创建供应商和客户关系
        /// </summary>
        /// <param name="customerID">客户ID</param>
        /// <param name="supplierID">供应商ID</param>
        /// <returns></returns>
        public bool AddSupplier(string customerID, string supplierID)
        {
            ICustomer dal = baseDal as ICustomer;
            return dal.AddSupplier(customerID, supplierID);
        }

        /// <summary>
        /// 如果建立关系，则移除供应商和客户关系
        /// </summary>
        /// <param name="customerID">客户ID</param>
        /// <param name="supplierID">供应商ID</param>
        /// <returns></returns>
        public bool RemoveSupplier(string customerID, string supplierID)
        {
            ICustomer dal = baseDal as ICustomer;
            return dal.RemoveSupplier(customerID, supplierID);
        }
                
        /// <summary>
        /// 如果建立关系，则移除供应商和客户的所有关系
        /// </summary>
        /// <param name="customerID">客户ID</param>
        /// <returns></returns>
        public bool RemoveSupplier(string customerID)
        {
            ICustomer dal = baseDal as ICustomer;
            return dal.RemoveSupplier(customerID);
        }     

        /// <summary>
        /// 获得指定间隔时间内未联系的客户列表
        /// </summary>
        /// <param name="unContactDays">和最后联系日期的间隔天数</param>
        /// <param name="userId">当前用户</param>
        /// <returns></returns>
        public List<CustomerInfo> GetUnContactList(int unContactDays, string userId, string condition)
        {
            List<CustomerInfo> listAll = new List<CustomerInfo>();

            //根据用户配置的信息进行逐条处理，然后合并记录
            List<CustomerAlarmInfo> alarmList = BLLFactory<CustomerAlarm>.Instance.FindByUser(userId);
            foreach (CustomerAlarmInfo alarmInfo in alarmList)
            {
                //如果存在高级查询对象信息，则使用高级查询条件，否则使用主表条件查询
                SearchCondition search = new SearchCondition();
                DateTime today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                int FollowExpireDays = alarmInfo.Days;
                if (FollowExpireDays < 1)
                {
                    FollowExpireDays = 1;
                }

                search.AddCondition("Grade", alarmInfo.Grade, SqlOperator.Equal);
                search.AddCondition("LastContactDate", today.AddDays(-1 * FollowExpireDays), SqlOperator.LessThanOrEqual);
                search.AddCondition("Deleted", 0, SqlOperator.Equal);//不显示删除的
                search.AddCondition("Creator", userId, SqlOperator.Equal);//仅仅选择该用户的记录

                string where = search.BuildConditionSql().Replace("Where", "");

                //数据权限的过滤：过滤规则，如果指定公司，以公司过滤，如果进一步指定部门，以公司+部门进行过滤；否则以个人的数据展示
                //如果过滤条件不为空，那么需要进行过滤
                if (!string.IsNullOrEmpty(condition))
                {
                    where = string.Format(" {0} AND {1}", condition, where);
                }

                List<CustomerInfo> list = baseDal.Find(where);
                foreach (CustomerInfo info in list)
                {
                    bool readed = BLLFactory<InformationStatus>.Instance.IsReadedStatus(userId, InformationCategory.客户联系, info.ID);
                    info.Data1 = readed ? "已读" : "未读";
                }

                listAll.AddRange(list);
            }
            return listAll;
        }

        /// <summary>
        /// 把客户分类列表，以树结构的方式提供
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <param name="dataFilter">数据过滤条件</param>
        /// <param name="shareUserCondition">分配用户过滤</param>
        /// <returns></returns>
        public List<TreeNodeInfo> GetCustomerTree(string userId, string companyId, string dataFilter, string shareUserCondition)
        {
            CustomerHelper helper = new CustomerHelper();
            var result = helper.GetCustomerTree(userId, companyId, dataFilter, shareUserCondition);
            return result;
        }
    }

    /// <summary>
    /// 对客户端树列表进行处理的辅助类
    /// </summary>
    internal class CustomerHelper
    {
        private const string treeCategory = "客户分类";
        private const string pageCategory = "客户选项卡";
        private List<string> userTreeList = new List<string>();//用户的树列表的数据库保存列表
        private string ShareUserCondition { get; set; }
        private string DataFilter { get; set; }

        /// <summary>
        /// 如果列表为空或包含指定ID，则认为包含
        /// </summary>
        /// <param name="id">树ID节点</param>
        /// <returns></returns>
        private bool ContainTree(string id)
        {
            bool result = false;
            if (userTreeList == null || userTreeList.Count == 0 || userTreeList.Contains(id))
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 根据用户ID，获取对应的客户树列表分类JSON
        /// </summary>
        /// <param name="userid">当前用户ID</param>
        /// <returns></returns>
        public List<TreeNodeInfo> GetCustomerTree(string userId, string companyId, string dataFilter, string shareUserCondition)
        {
            this.DataFilter = dataFilter;
            this.ShareUserCondition = shareUserCondition;

            //用户配置的列表
            userTreeList = BLLFactory<UserTreeSetting>.Instance.GetTreeSetting(treeCategory, userId, companyId);

            List<TreeNodeInfo> list = new List<TreeNodeInfo>();

            TreeNodeInfo pNode = new TreeNodeInfo("全部客户", 0);
            list.Add(pNode);

            string category = "客户属性分类";
            List<SystemTreeNodeInfo> propList = BLLFactory<SystemTree>.Instance.GetTree(category);
            foreach (SystemTreeNodeInfo nodeInfo in propList)
            {
                if (ContainTree(nodeInfo.ID))
                {
                    TreeNodeInfo subNode = new TreeNodeInfo(nodeInfo.TreeName, 1);
                    AddSystemTree2(nodeInfo.Children, subNode, 2);
                    list.Add(subNode);
                }
            }

            //字典子列表
            for (int i = 0; i < list.Count; i++)
            {
                TreeNodeInfo node = list[i];
                AddDictData2(node, 3);
            }


            //标记颜色的树形列表展示
            var colorNode = new TreeNodeInfo("标记颜色", 0);
            list.Add(colorNode);
            var dict = ColorHelper.ColorDict;
            foreach (string key in dict.Keys)
            {
                TreeNodeInfo subNode = new TreeNodeInfo(key, 9);
                var color = ColorTranslator.ToHtml(dict[key]);
                string filter = "";
                if (string.IsNullOrEmpty(color))
                {
                    filter += "(MarkColor ='' or MarkColor is null) ";
                }
                else
                {
                    filter = string.Format("{0}='{1}' ", "MarkColor", color);
                }
                subNode.Tag = filter;

                //增加数值
                //如果过滤条件不为空，那么需要进行过滤
                if (!string.IsNullOrEmpty(shareUserCondition))
                {
                    filter = string.Format(" {0} AND {1}", shareUserCondition, filter);
                }
                int count = BLLFactory<Customer>.Instance.GetRecordCount(filter);
                subNode.Text += string.Format("({0})", count);
                //避免透明不显示字体
                subNode.ForeColor = color;
                colorNode.Nodes.Add(subNode);
            }

            category = "客户状态分类";
            List<SystemTreeNodeInfo> statusList = BLLFactory<SystemTree>.Instance.GetTree(category);
            foreach (SystemTreeNodeInfo nodeInfo in statusList)
            {
                if (ContainTree(nodeInfo.ID))
                {
                    TreeNodeInfo subNode = new TreeNodeInfo(nodeInfo.TreeName, 1);
                    AddStatusTree2(nodeInfo.Children, subNode, 2);
                    //subNode.Expand();
                    list.Add(subNode);
                }
            }

            //~个人分组~
            TreeNodeInfo myGroupNode = new TreeNodeInfo("个人分组", 4);
            List<CustomerGroupNodeInfo> groupList = BLLFactory<CustomerGroup>.Instance.GetTree(userId, dataFilter);
            AddCustomerGroupTree2(groupList, myGroupNode, 3);
            //添加一个未分类和全部客户的组别
            myGroupNode.Nodes.Add(new TreeNodeInfo("未分组客户", 3));
            myGroupNode.Nodes.Add(new TreeNodeInfo("全部客户", 3));

            list.Add(myGroupNode);

            return list;
        }


        /// <summary>
        /// 获取客户分组并绑定
        /// </summary>
        private void AddCustomerGroupTree2(List<CustomerGroupNodeInfo> nodeList, TreeNodeInfo treeNode, int i)
        {
            foreach (CustomerGroupNodeInfo nodeInfo in nodeList)
            {
                if (ContainTree(nodeInfo.ID))
                {
                    TreeNodeInfo subNode = new TreeNodeInfo(nodeInfo.Name, i);
                    treeNode.Nodes.Add(subNode);

                    AddCustomerGroupTree2(nodeInfo.Children, subNode, i);
                }
            }
        }

        /// <summary>
        /// 客户状态的处理
        /// </summary>
        private void AddStatusTree2(List<SystemTreeNodeInfo> nodeList, TreeNodeInfo treeNode, int i)
        {
            foreach (SystemTreeNodeInfo nodeInfo in nodeList)
            {
                if (ContainTree(nodeInfo.ID))
                {
                    TreeNodeInfo subNode = new TreeNodeInfo(nodeInfo.TreeName, i);
                    subNode.Tag = nodeInfo.SpecialTag;//用来做一定的标识

                    //绑定数量
                    if (!string.IsNullOrWhiteSpace(nodeInfo.SpecialTag))
                    {
                        var filter = nodeInfo.SpecialTag;
                        //如果过滤条件不为空，那么需要进行过滤
                        if (!string.IsNullOrEmpty(ShareUserCondition))
                        {
                            filter = string.Format(" {0} AND {1}", ShareUserCondition, filter);
                        }
                        var count = BLLFactory<Customer>.Instance.GetRecordCount(filter);
                        subNode.Text += string.Format("({0})", count);
                    }
                    else
                    {
                        DateTime dt = DateTime.Now.ToString("yyyy-MM-dd").ToDateTime(); //当前日期
                        DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d"))); //本周周一
                        DateTime endWeek = startWeek.AddDays(6); //本周周日
                        DateTime startMonth = dt.AddDays(1 - dt.Day); //本月月初
                        DateTime endMonth = startMonth.AddMonths(1).AddDays(-1); //本月月末

                        bool statusFlag = false;
                        SearchCondition condition = new SearchCondition();
                        switch (nodeInfo.TreeName)
                        {
                            //本日联系客户,本周联系客户,本月联系客户,上月联系客户,本日新增客户,本周新增客户,本月新增客户,上月新增客户
                            #region 常规客户状态类型
                            case "本日联系客户":
                                statusFlag = true;
                                condition.AddCondition("LastContactDate", dt, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("LastContactDate", dt.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "本周联系客户":
                                statusFlag = true;
                                condition.AddCondition("LastContactDate", startWeek, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("LastContactDate", endWeek.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "本月联系客户":
                                statusFlag = true;
                                condition.AddCondition("LastContactDate", startMonth, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("LastContactDate", endMonth.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "上月联系客户":
                                statusFlag = true;
                                condition.AddCondition("LastContactDate", startMonth.AddMonths(-1), SqlOperator.MoreThanOrEqual)
                                    .AddCondition("LastContactDate", endMonth.AddMonths(-1).AddDays(1), SqlOperator.LessThan);
                                break;

                            case "本日新增客户":
                                statusFlag = true;
                                condition.AddCondition("CreateTime", dt, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("CreateTime", dt.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "本周新增客户":
                                statusFlag = true;
                                condition.AddCondition("CreateTime", startWeek, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("CreateTime", endWeek.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "本月新增客户":
                                statusFlag = true;
                                condition.AddCondition("CreateTime", startMonth, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("CreateTime", endMonth.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "上月新增客户":
                                statusFlag = true;
                                condition.AddCondition("CreateTime", startMonth.AddMonths(-1), SqlOperator.MoreThanOrEqual)
                                    .AddCondition("CreateTime", endMonth.AddMonths(-1).AddDays(1), SqlOperator.LessThan);
                                break;
                                #endregion
                        }

                        if (statusFlag)
                        {
                            var filter = condition.BuildConditionSql().Replace("Where", "");
                            subNode.Tag = filter;

                            //计算数量
                            //如果过滤条件不为空，那么需要进行过滤
                            if (!string.IsNullOrEmpty(ShareUserCondition))
                            {
                                filter = string.Format(" {0} AND {1}", ShareUserCondition, filter);
                            }
                            var count = BLLFactory<Customer>.Instance.GetRecordCount(filter);
                            subNode.Text += string.Format("({0})", count);
                        }
                    }
                    treeNode.Nodes.Add(subNode);

                    AddStatusTree2(nodeInfo.Children, subNode, i + 1);
                }
            }
        }

        /// <summary>
        /// 从数据库获取对应字典数据，并绑定到相关节点上
        /// </summary>
        private void AddDictData2(TreeNodeInfo treeNode, int i)
        {
            string nodeText = treeNode.Text;
            if (nodeText == "客户省份")
            {
                List<string> provinceList = BLLFactory<Customer>.Instance.GetCustomersProvince(ShareUserCondition);
                foreach (string province in provinceList)
                {
                    TreeNodeInfo subNode = new TreeNodeInfo(province, i);
                    if (treeNode.Tag != null)
                    {
                        string filter = string.Format("{0}='{1}' ", treeNode.Tag, province);
                        subNode.Tag = filter;

                        //增加数值
                        //如果过滤条件不为空，那么需要进行过滤
                        if (!string.IsNullOrEmpty(ShareUserCondition))
                        {
                            filter = string.Format(" {0} AND {1}", ShareUserCondition, filter);
                        }
                        int count = BLLFactory<Customer>.Instance.GetRecordCount(filter);
                        subNode.Text += string.Format("({0})", count);
                    }
                    treeNode.Nodes.Add(subNode);
                }
            }
            else if (nodeText == "客户城市")
            {
                List<string> cityList = BLLFactory<Customer>.Instance.GetCustomersCity(ShareUserCondition);
                foreach (string city in cityList)
                {
                    TreeNodeInfo subNode = new TreeNodeInfo(city, i);
                    if (treeNode.Tag != null)
                    {
                        string filter = string.Format("{0}='{1}' ", treeNode.Tag, city);
                        subNode.Tag = filter;

                        //增加数值
                        //如果过滤条件不为空，那么需要进行过滤
                        if (!string.IsNullOrEmpty(ShareUserCondition))
                        {
                            filter = string.Format(" {0} AND {1}", ShareUserCondition, filter);
                        }
                        int count = BLLFactory<Customer>.Instance.GetRecordCount(filter);
                        subNode.Text += string.Format("({0})", count);
                    }
                    treeNode.Nodes.Add(subNode);
                }
            }
            else
            {
                List<DictDataInfo> dict = BLLFactory<DictData>.Instance.FindByDictType(treeNode.Text);
                foreach (DictDataInfo info in dict)
                {
                    if (ContainTree(info.ID))
                    {
                        TreeNodeInfo subNode = new TreeNodeInfo(info.Name, i);
                        if (treeNode.Tag != null)
                        {
                            string filter = string.Format("{0}='{1}' ", treeNode.Tag, info.Value);
                            subNode.Tag = filter;

                            //增加数值
                            //如果过滤条件不为空，那么需要进行过滤
                            if (!string.IsNullOrEmpty(ShareUserCondition))
                            {
                                filter = string.Format(" {0} AND {1}", ShareUserCondition, filter);
                            }
                            int count = BLLFactory<Customer>.Instance.GetRecordCount(filter);
                            subNode.Text += string.Format("({0})", count);
                        }
                        treeNode.Nodes.Add(subNode);
                    }
                }
            }
            if (treeNode.Tag != null && treeNode.Tag.ToString().IndexOf('=') < 0)
            {
                treeNode.Tag = null;//如非正确Sql条件，设置为空，避免发生错误条件
            }

            for (int k = 0; k < treeNode.Nodes.Count; k++)
            {
                AddDictData2(treeNode.Nodes[k], i);
            }
        }

        private void AddSystemTree2(List<SystemTreeNodeInfo> nodeList, TreeNodeInfo treeNode, int i)
        {
            foreach (SystemTreeNodeInfo nodeInfo in nodeList)
            {
                if (ContainTree(nodeInfo.ID))
                {
                    TreeNodeInfo subNode = new TreeNodeInfo(nodeInfo.TreeName, i);
                    subNode.Tag = nodeInfo.SpecialTag;//用来做一定的标识

                    treeNode.Nodes.Add(subNode);

                    AddSystemTree2(nodeInfo.Children, subNode, i + 1);
                }
            }
        }
    }
   
    internal static class ColorHelper
    {
        private static Dictionary<string, Color> colorDict = new Dictionary<string, Color>();

        static ColorHelper()
        {
            colorDict.Add("红色", Color.Red);
            colorDict.Add("橙色", Color.Orange);
            //colorDict.Add("黄色", Color.Yellow);
            colorDict.Add("绿色", Color.Green);
            colorDict.Add("蓝色", Color.Blue);
            colorDict.Add("紫色", Color.Purple);
            colorDict.Add("黑色", Color.Black);
            colorDict.Add("无", Color.Empty);
        }

        /// <summary>
        /// 颜色字典
        /// </summary>
        public static Dictionary<string, Color> ColorDict
        {
            get { return colorDict; }
        }
    }
}
