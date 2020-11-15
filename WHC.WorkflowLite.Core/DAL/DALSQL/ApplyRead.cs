using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.DALSQL
{
	/// <summary>
	/// ApplyRead 的摘要说明。
	/// </summary>
    public class ApplyRead : BaseDALSQL<ApplyReadInfo>, IApplyRead
	{
		#region 对象实例及构造函数

		public static ApplyRead Instance
		{
			get
			{
				return new ApplyRead();
			}
		}
		public ApplyRead() : base("tbapp_apply_read","id")
		{
            this.SortField = "ADDTIME";
            this.IsDescending = true;
		}

		#endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override ApplyReadInfo DataReaderToEntity(IDataReader dataReader)
        {
            ApplyReadInfo info = new ApplyReadInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.ApplyId = reader.GetString("APPLY_ID");
            info.Addtime = reader.GetDateTime("ADDTIME");
            info.UserId = reader.GetInt32("USER_ID");
            info.Content = reader.GetString("CONTENT");
            info.ReadTime = reader.GetDateTime("READ_TIME");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(ApplyReadInfo obj)
        {
            ApplyReadInfo info = obj as ApplyReadInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("APPLY_ID", info.ApplyId);
            hash.Add("ADDTIME", info.Addtime);
            hash.Add("USER_ID", info.UserId);
            hash.Add("CONTENT", info.Content);
            hash.Add("READ_TIME", info.ReadTime);

            return hash;
        }
        
        /// <summary>
        /// 更新已读内容及时间
        /// </summary>
        /// <param name="applyId">申请单ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="content">已读内容</param>
        /// <returns></returns>
        public bool UpdateReadInfo(string applyId, int userId, string content)
        {
            string sql = string.Format("Update {0} Set content='{1}', read_time='{2}' Where apply_id='{3}' and user_id={4}",
                tableName, content, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), applyId, userId);
            Database db = CreateDatabase();
            return db.ExecuteNonQuery(CommandType.Text, sql) > 0;
        }
    }
}