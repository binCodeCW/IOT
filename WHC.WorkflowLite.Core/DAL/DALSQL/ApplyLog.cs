using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.DALSQL
{
	/// <summary>
	/// ApplyLog 的摘要说明。
	/// </summary>
    public class ApplyLog : BaseDALSQL<ApplyLogInfo>, IApplyLog
	{
		#region 对象实例及构造函数

		public static ApplyLog Instance
		{
			get
			{
				return new ApplyLog();
			}
		}

        /// <summary>
        /// 默认构造函数
        /// </summary>
		public ApplyLog() : base("tbapp_apply_log","id")
		{
            this.SortField = "ADDTIME";
            this.IsDescending = false;
		}

		#endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override ApplyLogInfo DataReaderToEntity(IDataReader dataReader)
        {
            ApplyLogInfo info = new ApplyLogInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.ApplyId = reader.GetString("APPLY_ID");
            info.Addtime = reader.GetDateTime("ADDTIME");
            info.UserId = reader.GetInt32("USER_ID");
            info.Content = reader.GetString("CONTENT");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(ApplyLogInfo obj)
        {
            ApplyLogInfo info = obj as ApplyLogInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);           
            hash.Add("APPLY_ID", info.ApplyId);
            hash.Add("ADDTIME", info.Addtime);
            hash.Add("USER_ID", info.UserId);
            hash.Add("CONTENT", info.Content);

            return hash;
        }

    }
}