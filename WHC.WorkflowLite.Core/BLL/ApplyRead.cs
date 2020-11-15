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
    /// 阅办步骤管理
    /// </summary>
	public class ApplyRead : BaseBLL<ApplyReadInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ApplyRead() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

        }

        /// <summary>
        /// 增加阅办处理记录
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="userIdList">流程用户ID列表</param>
        /// <returns></returns>
        public bool AddReadRecord(string applyId, string userIdList)
        {
            if (string.IsNullOrEmpty(applyId) || string.IsNullOrEmpty(userIdList))
                return false;

            string[] userIdArray = userIdList.Split(',');
            if (userIdArray == null || userIdArray.Length == 0)
                return false;

            IApplyRead dal = baseDal as IApplyRead;
            foreach (string userId in userIdArray)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var applyUserInfo = new ApplyUserInfo(applyId, Convert.ToInt32(userId));
                    BLLFactory<ApplyUser>.Instance.Insert(applyUserInfo);

                    ApplyReadInfo readInfo = new ApplyReadInfo(applyId, Convert.ToInt32(userId));
                    baseDal.Insert(readInfo);
                }
            }

            return true;
        }

        /// <summary>
        /// 更新已读内容及时间
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="content">已读处理意见</param>
        /// <returns></returns>
        public bool UpdateReadInfo(string applyId, int userId, string content)
        {
            //移除流程用户
            BLLFactory<ApplyUser>.Instance.DeleteByApplyId(applyId, userId);

            IApplyRead dal = baseDal as IApplyRead;
            return dal.UpdateReadInfo(applyId, userId, content);
        }

        /// <summary>
        /// 判断是否需要显示阅办状态
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public bool IsReadStatus(string applyId, int userId)
        {
            string condition = string.Format("apply_id ='{0}' and user_id ={1} ", applyId, userId);
            bool existUser = BLLFactory<ApplyUser>.Instance.IsExistRecord(condition);
            bool existRead = baseDal.IsExistRecord(condition);

            return existRead && existUser;
        }
                       
        /// <summary>
        /// 获取阅办相关信息列表
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public List<ApplyReadInfo> FindByApplyId(string applyId)
        {
            string condition = string.Format("apply_id ='{0}' AND read_time is not null", applyId);
            return Find(condition);
        }
    }
}
