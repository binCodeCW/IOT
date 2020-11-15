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
    /// 申请单日志
    /// </summary>
	public class ApplyLog : BaseBLL<ApplyLogInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ApplyLog() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 获取对应表单下的所有申请单日志
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public List<ApplyLogInfo> GetAllByApplyId(string applyId)
        {
            string condition = string.Format("APPLY_ID='{0}' ", applyId);
            return Find(condition, "order by addtime");
        }

        /// <summary>
        /// 添加申请单日志
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="userId">处理用户</param>
        /// <param name="content">日志内容</param>
        /// <returns></returns>
        public bool AddApplyLog(string applyId, int userId, string content)
        {
            ApplyLogInfo logInfo = new ApplyLogInfo(applyId, userId, content);
            return baseDal.Insert(logInfo);
        }
    }
}
