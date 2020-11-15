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
    /// 用户配置的系统列表集合
    /// </summary>
	public class UserTreeSetting : BaseDALSQL<UserTreeSettingInfo>, IUserTreeSetting
	{
		#region 对象实例及构造函数

		public static UserTreeSetting Instance
		{
			get
			{
				return new UserTreeSetting();
			}
		}
		public UserTreeSetting() : base("T_CRM_UserTreeSetting","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override UserTreeSettingInfo DataReaderToEntity(IDataReader dataReader)
		{
			UserTreeSettingInfo info = new UserTreeSettingInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Category = reader.GetString("Category");
			info.SystemTree_ID = reader.GetString("SystemTree_ID");
			info.Owner_ID = reader.GetString("Owner_ID");
            info.Dept_ID = reader.GetString("Dept_ID");
            info.Company_ID = reader.GetString("Company_ID");			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(UserTreeSettingInfo obj)
		{
		    UserTreeSettingInfo info = obj as UserTreeSettingInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Category", info.Category);
 			hash.Add("SystemTree_ID", info.SystemTree_ID);
 			hash.Add("Owner_ID", info.Owner_ID);
            hash.Add("Dept_ID", info.Dept_ID);
            hash.Add("Company_ID", info.Company_ID); 				
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
            dict.Add("ID", "编号");
            dict.Add("Category", "分类名称");
            dict.Add("SystemTree_ID", "树节点ID");
            dict.Add("Owner_ID", "所属用户ID");
            dict.Add("Dept_ID", "所属部门");
            dict.Add("Company_ID", "所属公司");
            #endregion

            return dict;
        }

    }
}