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
    /// 系统列表集合
    /// </summary>
	public class SystemTree : BaseDALSQL<SystemTreeInfo>, ISystemTree
	{
		#region 对象实例及构造函数

		public static SystemTree Instance
		{
			get
			{
				return new SystemTree();
			}
		}
		public SystemTree() : base("T_CRM_SystemTree","ID")
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
		protected override SystemTreeInfo DataReaderToEntity(IDataReader dataReader)
		{
			SystemTreeInfo info = new SystemTreeInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Category = reader.GetString("Category");
			info.TreeName = reader.GetString("TreeName");
			info.SpecialTag = reader.GetString("SpecialTag");
            info.Visible = reader.GetInt32("Visible") > 0;
			info.PID = reader.GetString("PID");
			info.Seq = reader.GetString("Seq");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(SystemTreeInfo obj)
		{
		    SystemTreeInfo info = obj as SystemTreeInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Category", info.Category);
 			hash.Add("TreeName", info.TreeName);
 			hash.Add("SpecialTag", info.SpecialTag);
            hash.Add("Visible", info.Visible ? 1 : 0);
 			hash.Add("PID", info.PID);
 			hash.Add("Seq", info.Seq);
 				
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
             dict.Add("Category", "分类名称");
             dict.Add("TreeName", "树节点名称");
             dict.Add("SpecialTag", "特殊标志");
             dict.Add("Visible", "是否可见");
             dict.Add("PID", "父ID");
             dict.Add("Seq", "排序");
             #endregion

            return dict;
        }

        /// <summary>
        /// 获取树形结构的菜单列表
        /// </summary>
        public List<SystemTreeNodeInfo> GetTree(string category)
        {
            string condition = !string.IsNullOrEmpty(category) ? string.Format("AND Category='{0}'", category) : "";
            List<SystemTreeNodeInfo> nodeList = new List<SystemTreeNodeInfo>();
            string sql = string.Format("Select * From {0} Where Visible > 0 {1} Order By PID, Seq ", tableName, condition);

            DataTable dt = base.SqlTable(sql);
            DataRow[] dataRows = dt.Select(string.Format(" PID = '{0}' ", -1));
            for (int i = 0; i < dataRows.Length; i++)
            {
                string id = dataRows[i]["ID"].ToString();
                SystemTreeNodeInfo nodeInfo = GetNode(id, dt);
                nodeList.Add(nodeInfo);
            }

            return nodeList;
        }

        private SystemTreeNodeInfo GetNode(string id, DataTable dt)
        {
            SystemTreeInfo info = this.FindByID(id);
            SystemTreeNodeInfo nodeInfo = new SystemTreeNodeInfo(info);

            DataRow[] dChildRows = dt.Select(string.Format(" PID='{0}'", id));
            for (int i = 0; i < dChildRows.Length; i++)
            {
                string childId = dChildRows[i]["ID"].ToString();
                SystemTreeNodeInfo childNodeInfo = GetNode(childId, dt);
                nodeInfo.Children.Add(childNodeInfo);
            }
            return nodeInfo;
        }

    }
}