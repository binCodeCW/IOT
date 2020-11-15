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
    /// 常用审批意见管理
    /// </summary>
	public class CommonOpinion : BaseBLL<CommonOpinionInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CommonOpinion() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据用户和表单ID获取对应的常用审批意见列表
        /// </summary>
        /// <param name="userId">所属用户</param>
        /// <param name="formId">流程表单ID，可为空</param>
        /// <returns></returns>
        public List<CommonOpinionInfo> FindByUser(int userId, string formId = null)
        {
            string condition = string.Format("(FORM_SCENE='{0}'  OR FORM_SCENE='' OR FORM_SCENE IS NULL) AND (BELONG_USER='{1}' OR BELONG_USER IS NULL) AND SEQ >=0",
                formId ?? "",  userId);

            return Find(condition);
        }

        /// <summary>
        /// 添加常用意见
        /// </summary>
        /// <param name="opinion">常用意见</param>
        /// <param name="userId">所属用户</param>
        /// <param name="formId">流程表单ID，可为空</param>
        /// <returns></returns>
        public bool AddOpinion(string opinion, int userId, string formId = null)
        {
            string condition = string.Format(@" (FORM_SCENE='{0}' OR FORM_SCENE='' OR FORM_SCENE IS NULL) 
            AND (BELONG_USER='{1}' OR BELONG_USER IS NULL)
            AND OPINION = '{2}' ", formId, userId, opinion);
            if (IsExistRecord(condition))
            {
                throw new Exception("已存在相同的常用意见，不可重复添加！");
            }
            else
            {
                CommonOpinionInfo info = new CommonOpinionInfo();
                info.Opinion = opinion;
                info.BelongUser = userId.ToString();
                info.FormScene = formId;
                return baseDal.Insert(info);
            }
        }
    }
}
