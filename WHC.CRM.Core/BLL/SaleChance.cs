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

namespace WHC.CRM.BLL
{
    /// <summary>
    /// 销售机会
    /// </summary>
	public class SaleChance : BaseBLL<SaleChanceInfo>
    {
        public SaleChance() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 获取记录日期年度列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetYearList()
        {
            ISaleChance dal = baseDal as ISaleChance;
            return dal.GetYearList();
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
        /// 更新阶段信息
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="stage">当前阶段</param>
        /// <returns></returns>
        public CommonResult UpdateStage(string id, double stage)
        {
            Hashtable ht = new Hashtable(); ;
            ht.Add("Stage", stage);

            var result = baseDal.UpdateFields(ht, id);
            return new CommonResult(result, result ? "成功" : "失败");
        }
    }
}
