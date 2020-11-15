using System.Collections;
using System.Data;
using System.Collections.Generic;

using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using YH.Security.Entity;
using YH.Security.IDAL;

namespace YH.Security.DALSQL
{
    /// <summary>
    /// 字段权限域对象
    /// </summary>
	public class FieldDomain : BaseDALSQL<FieldDomainInfo>, IFieldDomain
	{
		#region 对象实例及构造函数

		public static FieldDomain Instance
		{
			get
			{
				return new FieldDomain();
			}
		}
		public FieldDomain() : base("T_ACL_FieldDomain","ID")
        {
            this.SortField = "CreateTime";
            this.IsDescending = false;
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override FieldDomainInfo DataReaderToEntity(IDataReader dataReader)
		{
			FieldDomainInfo info = new FieldDomainInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.EntityFullName = reader.GetString("EntityFullName");
			info.ClassPath = reader.GetString("ClassPath");
			info.ColumnAlias = reader.GetString("ColumnAlias");
			info.FieldList = reader.GetString("FieldList");
			info.CreateTime = reader.GetDateTime("CreateTime");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(FieldDomainInfo obj)
		{
		    FieldDomainInfo info = obj as FieldDomainInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("EntityFullName", info.EntityFullName);
 			hash.Add("ClassPath", info.ClassPath);
 			hash.Add("ColumnAlias", info.ColumnAlias);
 			hash.Add("FieldList", info.FieldList);
 			hash.Add("CreateTime", info.CreateTime);
 				
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
            dict.Add("ID", "ID");
             dict.Add("EntityFullName", "实体类全名");
             dict.Add("ClassPath", "类路径");
             dict.Add("ColumnAlias", "字段别名");
             dict.Add("FieldList", "字段列表");
             dict.Add("CreateTime", "创建时间");
             #endregion

            return dict;
        }

    }
}