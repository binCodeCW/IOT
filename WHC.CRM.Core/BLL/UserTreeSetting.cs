using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.CRM.Entity;
using WHC.CRM.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;

namespace WHC.CRM.BLL
{
    /// <summary>
    /// 用户配置的系统列表集合
    /// </summary>
	public class UserTreeSetting : BaseBLL<UserTreeSettingInfo>
    {
        public UserTreeSetting() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 删除指定用户的分类节点
        /// </summary>
        /// <param name="category">树类别名称</param>
        /// <param name="userId">所属用户ID</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public bool DeleteByCategory(string category, string userId, string companyId, DbTransaction trans = null)
        {
            string condition = string.Format("Category='{0}' and Owner_ID='{1}' AND Company_ID='{2}'", category, userId, companyId);
            return base.DeleteByCondition(condition, trans);
        }

        /// <summary>
        /// 保存用户的树节点设置信息
        /// </summary>
        /// <param name="category">树分类</param>
        /// <param name="userId">所属用户</param>
        /// <param name="treeIdList">树节点ID集合</param>
        /// <returns></returns>
        public bool SaveTreeSetting(string category, string userId, string companyId, List<string> treeIdList)
        {
            bool result = false;
            DbTransaction trans = CreateTransaction();
            if (trans != null)
            {
                //先移除旧记录
                DeleteByCategory(category, userId, companyId, trans);

                foreach (string id in treeIdList)
                {
                    UserTreeSettingInfo info = new UserTreeSettingInfo();
                    info.Owner_ID = userId;
                    info.Company_ID = companyId;
                    info.Category = category;
                    info.SystemTree_ID = id;

                    base.Insert(info, trans);
                }

                trans.Commit();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 获取用户的树形节点集合
        /// </summary>
        /// <param name="category">树分类</param>
        /// <param name="userId">所属用户</param>
        /// <returns></returns>
        public List<string> GetTreeSetting(string category, string userId, string companyId)
        {
            List<string> result = new List<string>();
            string condition = string.Format("Category='{0}' and Owner_ID='{1}' AND Company_ID='{2}'", category, userId, companyId);
            List<UserTreeSettingInfo> list = base.Find(condition);
            foreach (UserTreeSettingInfo info in list)
            {
                result.Add(info.SystemTree_ID);
            }
            return result;
        }
    }
}
