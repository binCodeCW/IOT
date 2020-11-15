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
    /// 用户对指定内容的操作状态记录
    /// </summary>
	public class InformationStatus : BaseBLL<InformationStatusInfo>
    {
        public InformationStatus() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="InfoType">信息类型</param>
        /// <param name="InfoID">信息主键ID</param>
        /// <param name="Status">状态:0未读 1已读 </param>
        public void SetStatus(string UserID, InformationCategory InfoType, string InfoID, int Status)
        {
            IInformationStatus dal = baseDal as IInformationStatus;
            dal.SetStatus(UserID, InfoType, InfoID, Status);
        }

        /// <summary>
        /// 匹配状态
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="InfoType">信息类型</param>
        /// <param name="InfoID">信息主键ID</param>
        /// <param name="Status">状态:0未读 1已读 </param>
        /// <returns></returns>
        public bool CheckStatus(string UserID, InformationCategory InfoType, string InfoID, int Status)
        {
            IInformationStatus dal = baseDal as IInformationStatus;
            return dal.CheckStatus(UserID, InfoType, InfoID, Status);
        }

        /// <summary>
        /// 查看指定的记录是否已读
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="InfoType">信息类型</param>
        /// <param name="InfoID">信息主键ID</param>
        /// <returns></returns>
        public bool IsReadedStatus(string UserID, InformationCategory InfoType, string InfoID)
        {
            IInformationStatus dal = baseDal as IInformationStatus;
            int status = 1;//0未读 1已读 
            return dal.CheckStatus(UserID, InfoType, InfoID, status);
        }
    }
}
