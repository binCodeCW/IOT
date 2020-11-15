using System.Collections;
using System.Data;
using System.Collections.Generic;

using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.CRM.Entity;
using WHC.CRM.IDAL;

namespace WHC.CRM.DALSQL
{
    /// <summary>
    /// 客户提醒设置
    /// </summary>
	public class CustomerAlarm : BaseDALSQL<CustomerAlarmInfo>, ICustomerAlarm
	{
		#region 对象实例及构造函数

		public static CustomerAlarm Instance
		{
			get
			{
				return new CustomerAlarm();
			}
		}
		public CustomerAlarm() : base("T_CRM_CustomerAlarm","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override CustomerAlarmInfo DataReaderToEntity(IDataReader dataReader)
		{
			CustomerAlarmInfo info = new CustomerAlarmInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.User_ID = reader.GetString("User_ID");
			info.Grade = reader.GetString("Grade");
			info.Days = reader.GetInt32("Days");
			info.Note = reader.GetString("Note");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(CustomerAlarmInfo obj)
		{
		    CustomerAlarmInfo info = obj as CustomerAlarmInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("User_ID", info.User_ID);
 			hash.Add("Grade", info.Grade);
 			hash.Add("Days", info.Days);
 			hash.Add("Note", info.Note);
 				
			return hash;
		}

        /// <summary>
        /// 获取字段中文别名（用于界面显示）的字典集合
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> GetColumnNameAlias()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            #region 添加别名解析
            //dict.Add("ID", "编号");
            dict.Add("ID", "");
             dict.Add("User_ID", "所属用户");
             dict.Add("Grade", "客户级别");
             dict.Add("Days", "提醒天数");
             dict.Add("Note", "备注信息");
             #endregion

            return dict;
        }

    }
}