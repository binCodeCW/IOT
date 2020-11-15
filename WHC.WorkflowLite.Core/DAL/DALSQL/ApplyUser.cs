using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;

using YH.Pager.Entity;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.DALSQL
{
	/// <summary>
	/// ApplyUser 的摘要说明。
	/// </summary>
    public class ApplyUser : BaseDALSQL<ApplyUserInfo>, IApplyUser
	{
		#region 对象实例及构造函数

		public static ApplyUser Instance
		{
			get
			{
				return new ApplyUser();
			}
		}
		public ApplyUser() : base("tbapp_apply_user","id")
		{
		}

		#endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override ApplyUserInfo DataReaderToEntity(IDataReader dataReader)
        {
            ApplyUserInfo info = new ApplyUserInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.ApplyId = reader.GetString("APPLY_ID");
            info.UserId = reader.GetInt32("USER_ID");
            info.Alerttime = reader.GetDateTime("ALERTTIME");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(ApplyUserInfo obj)
        {
            ApplyUserInfo info = obj as ApplyUserInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("APPLY_ID", info.ApplyId);
            hash.Add("USER_ID", info.UserId);
            hash.Add("ALERTTIME", info.Alerttime);

            return hash;
        }


        /// <summary>
        /// 获取用户已办业务的ID字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetApplyIdDone(int userId)
        {
            string result = "'0'";
            string sql = string.Format("select distinct apply_id from tbapp_apply_flow where proc_uid={0}", userId);

            var value = SqlValueList(sql);
            if (!string.IsNullOrEmpty(value))
            {
                var idList = value.ToDelimitedList<string>(",");
                idList.ForEach(s => result += string.Format(",'{0}'",s));
            }
            return result;
        }

        /// <summary>
        /// 根据申请表单ID获取对应的处理人员
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <returns></returns>
        public string GetUsersByApplyId(string applyId)
        {
            string sql = string.Format("select user_id from {0} where apply_id='{1}' ", tableName,applyId);
            return SqlValueList(sql);
        }

    }
}