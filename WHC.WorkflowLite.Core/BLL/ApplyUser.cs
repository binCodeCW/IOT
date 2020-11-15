using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity; 
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 流程处理人管理
    /// </summary>
	public class ApplyUser : BaseBLL<ApplyUserInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ApplyUser() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 获取用户已办业务的ID字符串
        /// </summary>
        /// <param name="userId">用户的ID</param>
        /// <returns></returns>
        public string GetApplyIdDone(int userId)
        {
            IApplyUser dal = baseDal as IApplyUser;
            return dal.GetApplyIdDone(userId);
        }

        /// <summary>
        /// 根据用户的ID获取其对应的申请单ID字符串(无则返回0）
        /// </summary>
        /// <param name="userId">用户的ID</param>
        /// <returns></returns>
        public string GetApplyIdByUser(int userId)
        {
            //待办列表
            string applyIdString = "'0'";

            string conditon = string.Format("user_id={0}", userId);
            List<ApplyUserInfo> applyUserList = baseDal.Find(conditon);
            if (applyUserList != null)
            {
                foreach (ApplyUserInfo userInfo in applyUserList)
                {
                    applyIdString += string.Format(",'{0}'", userInfo.ApplyId);
                }
                applyIdString = applyIdString.Trim(',');

                //用户申请但被拒绝的
                //status 0:处理中,1:已完成,2:已退回,3:已撤消
                conditon = string.Format("editor={0} and status={1}", userId, (int)ApplyStatus.已退回);
                List<ApplyInfo> applyList = BLLFactory<Apply>.Instance.Find(conditon);
                foreach (ApplyInfo info in applyList)
                {
                    applyIdString += string.Format(",'{0}' ", info.ID);
                }
                applyIdString = applyIdString.Trim(',');
            }

            return applyIdString;
        }

        /// <summary>
        /// 获取用户授权的申请单数量
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="userId">用户的ID</param>
        /// <returns></returns>
        public int GetCountByApplyIdAndUserId(string applyId, int userId)
        {
            string conditon = string.Format("apply_id ='{0}' and user_id={1}", applyId, userId);
            return baseDal.GetRecordCount(conditon);
        }

        /// <summary>
        /// 删除申请单对应的所有用户信息 
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public bool DeleteByApplyId(string applyId, DbTransaction trans = null)
        {
            string condition = string.Format("apply_id ='{0}' ", applyId);
            return baseDal.DeleteByCondition(condition, trans);
        }

        /// <summary>
        /// 删除申请单对应用户的相关信息 
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="userid">用户ID</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public bool DeleteByApplyId(string applyId, int userid, DbTransaction trans = null)
        {
            string condition = string.Format("apply_id ='{0}' and user_id={1}", applyId, userid);
            return baseDal.DeleteByCondition(condition, trans);
        }        
        
        /// <summary>
        /// 根据申请表单ID获取对应的处理人员
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public string GetUsersByApplyId(string applyId)
        {
            IApplyUser userDal = baseDal as IApplyUser;
            return userDal.GetUsersByApplyId(applyId);
        }

        /// <summary>
        /// 是否有待审批信息需要处理[是true,否false]
        /// </summary>
        /// <param name="applyId">申请单id</param>
        /// <param name="userId">用户id</param>
        /// <returns>bool</returns>
        public bool IsCheckPermission(string applyId, int userId)
        {
            if (string.IsNullOrEmpty(applyId) || userId <= 0)
                return false;

            string condition = string.Format(" apply_id='{0}' and user_id={1}", applyId, userId);
            return baseDal.GetRecordCount(condition) > 0;
        }
    }
}
