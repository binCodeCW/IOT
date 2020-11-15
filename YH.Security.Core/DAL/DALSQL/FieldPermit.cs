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
    /// 字段的列表权限
    /// </summary>
	public class FieldPermit : BaseDALSQL<FieldPermitInfo>, IFieldPermit
	{
		#region 对象实例及构造函数

		public static FieldPermit Instance
		{
			get
			{
				return new FieldPermit();
			}
		}
		public FieldPermit() : base("T_ACL_FieldPermit","ID")
		{
            this.SortField = "Seq";
            this.IsDescending = false;
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override FieldPermitInfo DataReaderToEntity(IDataReader dataReader)
		{
			FieldPermitInfo info = new FieldPermitInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Role_ID = reader.GetInt32("Role_ID");
			info.FieldDomain_ID = reader.GetString("FieldDomain_ID");
			info.EntityFullName = reader.GetString("EntityFullName");
			info.FiledName = reader.GetString("FiledName");
			info.FiledCode = reader.GetString("FiledCode");
			info.Seq = reader.GetDecimal("Seq");
			info.Permit = reader.GetInt32("Permit");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(FieldPermitInfo obj)
		{
		    FieldPermitInfo info = obj as FieldPermitInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Role_ID", info.Role_ID);
 			hash.Add("FieldDomain_ID", info.FieldDomain_ID);
 			hash.Add("EntityFullName", info.EntityFullName);
 			hash.Add("FiledName", info.FiledName);
 			hash.Add("FiledCode", info.FiledCode);
 			hash.Add("Seq", info.Seq);
 			hash.Add("Permit", info.Permit);
 				
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
             dict.Add("Role_ID", "角色ID");
             dict.Add("FieldDomain_ID", "字段域对象ID");
             dict.Add("EntityFullName", "实体类全名");
             dict.Add("FiledName", "字段名称");
             dict.Add("FiledCode", "字段代码");
             dict.Add("Seq", "排序");
             dict.Add("Permit", "字段权限");
             #endregion

            return dict;
        }

    }
}