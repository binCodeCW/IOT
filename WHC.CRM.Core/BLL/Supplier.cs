using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Drawing;

using WHC.CRM.Entity;
using WHC.CRM.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;
using WHC.Dictionary.Entity;
using WHC.Dictionary.BLL;

namespace WHC.CRM.BLL
{
    /// <summary>
    /// 供应商
    /// </summary>
	public class Supplier : BaseBLL<SupplierInfo>
    {
        public Supplier() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
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
        /// 根据供应商分组的名称，搜索属于该分组的供应商列表
        /// </summary>
        /// <param name="ownerUser">供应商所属用户</param>
        /// <param name="groupName">供应商分组的名称,如果供应商分组为空，那么返回未分组供应商列表</param>
        /// <param name="pagerInfo">分页条件</param>
        /// <returns></returns>
        public List<SupplierInfo> FindByGroupName(string ownerUser, string groupName, string condition, PagerInfo pagerInfo = null)
        {
            ISupplier dal = baseDal as ISupplier;
            return dal.FindByGroupName(ownerUser, groupName, condition, pagerInfo);
        }

        /// <summary>
        /// 设置删除标志
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <param name="deleted">是否删除</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public bool SetDeletedFlag(string id, bool deleted = true, DbTransaction trans = null)
        {
            ISupplier dal = baseDal as ISupplier;
            return dal.SetDeletedFlag(id, deleted, trans);
        }

        /// <summary>
        /// 获取供应商的名称
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <returns></returns>
        public string GetSupplierName(string id, DbTransaction trans = null)
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
        /// 调整供应商的组别
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <param name="groupIdList">供应商分组Id集合</param>
        /// <returns></returns>
        public bool ModifyGroup(string id, List<string> groupIdList)
        {
            ISupplier dal = baseDal as ISupplier;
            return dal.ModifyGroup(id, groupIdList);
        }

        /// <summary>
        /// 获取供应商的省份列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetSuppliersProvince(string condition)
        {
            List<string> list = GetFieldListByCondition("Province", condition);
            return list;
        }

        /// <summary>
        /// 获取供应商的城市列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetSuppliersCity(string condition)
        {
            List<string> list = GetFieldListByCondition("City", condition);
            return list;
        }

        /// <summary>
        /// 根据供应商名称获取供应商信息
        /// </summary>
        /// <param name="name">供应商名称</param>
        /// <returns></returns>
        public SupplierInfo FindByName(string name)
        {
            string condition = string.Format("Name='{0}' ", name);
            return FindSingle(condition);
        }

        /// <summary>
        /// 给SearchLookup控件提供数据源，返回部分供应商字段信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllForLookup()
        {
            string sql = string.Format("Select ID,Name,SimpleName,HandNo from T_CRM_Supplier order by HandNo");
            return baseDal.SqlTable(sql);
        }

        /// <summary>
        /// 更新供应商的状态信息
        /// </summary>
        /// <param name="id">供应商Id</param>
        /// <param name="orderDate">订单日期</param>
        /// <param name="orderCount">交易次数</param>
        /// <param name="orderMoney">交易金额</param>
        /// <returns></returns>
        public bool UpdateTransactionStatus(string id, DateTime orderDate, int orderCount, decimal orderMoney, DbTransaction trans = null)
        {
            ISupplier dal = baseDal as ISupplier;
            return dal.UpdateTransactionStatus(id, orderDate, orderCount, orderMoney, trans);
        }

        /// <summary>
        /// 更新供应商的最后联系日期
        /// </summary>
        /// <param name="id">供应商ID</param>
        /// <param name="lastContactDate">最后联系日期</param>
        /// <returns></returns>
        public bool UpdateContactDate(string id, DateTime lastContactDate, DbTransaction trans = null)
        {
            ISupplier dal = baseDal as ISupplier;
            return dal.UpdateContactDate(id, lastContactDate, trans);
        }
                      
        /// <summary>
        /// 根据客户ID获取供应商关联ID
        /// </summary>
        /// <param name="customerID">客户ID</param>
        /// <returns></returns>
        public List<string> GetSupplierByCustomer(string customerID)
        {
            ISupplier dal = baseDal as ISupplier;
            return dal.GetSupplierByCustomer(customerID);
        }
               
        /// <summary>
        /// 根据供应商所属客户ID，分页获取供应商列表
        /// </summary>
        /// <param name="customerID">供应商所属客户ID</param>
        /// <param name="pagerInfo">分页条件</param>
        /// <returns></returns>
        public List<SupplierInfo> FindByCustomer(string customerID, string condition, PagerInfo pagerInfo = null)
        {
            ISupplier dal = baseDal as ISupplier;
            return dal.FindByCustomer(customerID, condition, pagerInfo);
        }

        /// <summary>
        /// 根据记录手工编号进行查询，获取对应记录
        /// </summary>
        /// <param name="handNo">手工编号</param>
        /// <returns></returns>
        public SupplierInfo FindByHandNo(string handNo)
        {
            string condition = string.Format("HandNo ='{0}' ", handNo);
            return FindSingle(condition);
        }

        /// <summary>
        /// 把供应商分类列表，以树结构的方式提供
        /// </summary>
        /// <param name="userId">当前用户ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <param name="dataFilter">数据过滤条件</param>
        /// <param name="shareUserCondition">分配用户过滤</param>
        /// <returns></returns>
        public List<TreeNodeInfo> GetSupplierTree(string userId, string companyId, string dataFilter, string shareUserCondition)
        {
            SupplierHelper helper = new SupplierHelper();
            var result = helper.GetSupplierTree(userId, companyId, dataFilter, shareUserCondition);
            return result;
        }
    }

    internal class SupplierHelper
    {
        private const string treeCategory = "供应商分类";
        private const string pageCategory = "供应商选项卡";
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
        public List<TreeNodeInfo> GetSupplierTree(string userId, string companyId, string dataFilter, string shareUserCondition)
        {
            this.DataFilter = dataFilter;
            this.ShareUserCondition = shareUserCondition;

            //用户配置的列表
            userTreeList = BLLFactory<UserTreeSetting>.Instance.GetTreeSetting(treeCategory, userId, companyId);

            List<TreeNodeInfo> list = new List<TreeNodeInfo>();

            TreeNodeInfo pNode = new TreeNodeInfo("全部供应商", 0);
            list.Add(pNode);

            string category = "供应商属性分类";
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
                int count = BLLFactory<Supplier>.Instance.GetRecordCount(filter);
                subNode.Text += string.Format("({0})", count);
                //避免透明不显示字体
                subNode.ForeColor = color;
                colorNode.Nodes.Add(subNode);
            }

            category = "供应商状态分类";
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
            List<SupplierGroupNodeInfo> groupList = BLLFactory<SupplierGroup>.Instance.GetTree(userId, dataFilter);
            AddCustomerGroupTree2(groupList, myGroupNode, 3);
            //添加一个未分类和全部客户的组别
            myGroupNode.Nodes.Add(new TreeNodeInfo("未分组供应商", 3));
            myGroupNode.Nodes.Add(new TreeNodeInfo("全部供应商", 3));

            list.Add(myGroupNode);

            return list;
        }


        /// <summary>
        /// 获取客户分组并绑定
        /// </summary>
        private void AddCustomerGroupTree2(List<SupplierGroupNodeInfo> nodeList, TreeNodeInfo treeNode, int i)
        {
            foreach (SupplierGroupNodeInfo nodeInfo in nodeList)
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
                        var count = BLLFactory<Supplier>.Instance.GetRecordCount(filter);
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
                            case "本日联系":
                                statusFlag = true;
                                condition.AddCondition("LastContactDate", dt, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("LastContactDate", dt.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "本周联系":
                                statusFlag = true;
                                condition.AddCondition("LastContactDate", startWeek, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("LastContactDate", endWeek.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "本月联系":
                                statusFlag = true;
                                condition.AddCondition("LastContactDate", startMonth, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("LastContactDate", endMonth.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "上月联系":
                                statusFlag = true;
                                condition.AddCondition("LastContactDate", startMonth.AddMonths(-1), SqlOperator.MoreThanOrEqual)
                                    .AddCondition("LastContactDate", endMonth.AddMonths(-1).AddDays(1), SqlOperator.LessThan);
                                break;

                            case "本日新增":
                                statusFlag = true;
                                condition.AddCondition("CreateTime", dt, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("CreateTime", dt.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "本周新增":
                                statusFlag = true;
                                condition.AddCondition("CreateTime", startWeek, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("CreateTime", endWeek.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "本月新增客户":
                                statusFlag = true;
                                condition.AddCondition("CreateTime", startMonth, SqlOperator.MoreThanOrEqual)
                                    .AddCondition("CreateTime", endMonth.AddDays(1), SqlOperator.LessThan);
                                break;
                            case "上月新增":
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
                            var count = BLLFactory<Supplier>.Instance.GetRecordCount(filter);
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
            if (nodeText == "供应商省份")
            {
                List<string> provinceList = BLLFactory<Supplier>.Instance.GetSuppliersProvince(ShareUserCondition);
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
                        int count = BLLFactory<Supplier>.Instance.GetRecordCount(filter);
                        subNode.Text += string.Format("({0})", count);
                    }
                    treeNode.Nodes.Add(subNode);
                }
            }
            else if (nodeText == "客户城市")
            {
                List<string> cityList = BLLFactory<Supplier>.Instance.GetSuppliersCity(ShareUserCondition);
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
                        int count = BLLFactory<Supplier>.Instance.GetRecordCount(filter);
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
                            int count = BLLFactory<Supplier>.Instance.GetRecordCount(filter);
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
}
