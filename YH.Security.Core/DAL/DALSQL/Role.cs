using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;

using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using YH.Security.Entity;
using YH.Security.IDAL;

namespace YH.Security.DALSQL
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public class Role : BaseDALSQL<RoleInfo>, IRole
    {
        #region 对象实例及构造函数

        public static Role Instance
        {
            get
            {
                return new Role();
            }
        }
        public Role()
            : base("T_ACL_Role", "ID")
        {
            this.SortField = "SortCode";
            this.IsDescending = false;
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override RoleInfo DataReaderToEntity(IDataReader dataReader)
        {
            RoleInfo info = new RoleInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetInt32("ID");
            info.PID = reader.GetInt32("PID");
            info.HandNo = reader.GetString("HandNo");
            info.Name = reader.GetString("Name");
            info.Note = reader.GetString("Note");
            info.SortCode = reader.GetString("SortCode");
            info.Category = reader.GetString("Category");
            info.Company_ID = reader.GetString("Company_ID");
            info.CompanyName = reader.GetString("CompanyName");
            info.Creator = reader.GetString("Creator");
            info.Creator_ID = reader.GetString("Creator_ID");
            info.CreateTime = reader.GetDateTime("CreateTime");
            info.Editor = reader.GetString("Editor");
            info.Editor_ID = reader.GetString("Editor_ID");
            info.EditTime = reader.GetDateTime("EditTime");
            info.Deleted = reader.GetInt32("Deleted") > 0;
            info.Enabled = reader.GetInt32("Enabled") > 0;

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(RoleInfo obj)
        {
            RoleInfo info = obj as RoleInfo;
            Hashtable hash = new Hashtable();

            hash.Add("PID", info.PID);
            hash.Add("HandNo", info.HandNo);
            hash.Add("Name", info.Name);
            hash.Add("Note", info.Note);
            hash.Add("SortCode", info.SortCode);
            hash.Add("Category", info.Category);
            hash.Add("Company_ID", info.Company_ID);
            hash.Add("CompanyName", info.CompanyName);
            hash.Add("Creator", info.Creator);
            hash.Add("Creator_ID", info.Creator_ID);
            hash.Add("CreateTime", info.CreateTime);
            hash.Add("Editor", info.Editor);
            hash.Add("Editor_ID", info.Editor_ID);
            hash.Add("EditTime", info.EditTime);
            hash.Add("Deleted", info.Deleted ? 1 : 0);
            hash.Add("Enabled", info.Enabled ? 1 : 0);

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
            dict.Add("PID", "父ID");
            dict.Add("HandNo", "角色编码");
            dict.Add("Name", "角色名称");
            dict.Add("Note", "备注");
            dict.Add("SortCode", "排序码");
            dict.Add("Category", "角色分类");
            dict.Add("Company_ID", "所属机构ID");
            dict.Add("CompanyName", "所属机构名称");
            dict.Add("Creator", "创建人");
            dict.Add("Creator_ID", "创建人ID");
            dict.Add("CreateTime", "创建时间");
            dict.Add("Editor", "编辑人");
            dict.Add("Editor_ID", "编辑人ID");
            dict.Add("EditTime", "编辑时间");
            dict.Add("Deleted", "是否已删除");
            dict.Add("Enabled", "有效标志");
            #endregion

            return dict;
        }

        /// <summary>
        /// 给指定角色添加功能点
        /// </summary>
        /// <param name="functionID">功能ID</param>
        /// <param name="roleID">角色ID</param>
        public void AddFunction(string functionID, int roleID)
        {
            var targetTable = "T_ACL_Role_Function";
            string checkSql = string.Format("Select count(*) from {0} Where Function_ID='{1}' AND Role_ID={2}", targetTable, functionID, roleID);
            string insertSql = string.Format("INSERT INTO {0}(Function_ID, Role_ID) VALUES('{1}',{2})", targetTable, functionID, roleID);

            InsertCheckDuplicated(checkSql, insertSql);
        }

        /// <summary>
        /// 给指定角色添加机构
        /// </summary>
        /// <param name="ouID">机构ID</param>
        /// <param name="roleID">角色ID</param>
        public void AddOU(int ouID, int roleID)
        {
            var targetTable = "T_ACL_OU_Role";
            string checkSql = string.Format("Select count(*) from {0} Where OU_ID={1} AND Role_ID={2}", targetTable, ouID, roleID);
            string insertSql = string.Format("INSERT INTO {0}(OU_ID, Role_ID) VALUES({1},{2})", targetTable, ouID, roleID);

            InsertCheckDuplicated(checkSql, insertSql);
        }

        /// <summary>
        /// 给指定角色添加用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="roleID">角色ID</param>
        public void AddUser(int userID, int roleID)
        {
            var targetTable = "T_ACL_User_Role";
            string checkSql = string.Format("Select count(*) from {0} Where User_ID={1} AND Role_ID={2}", targetTable, userID, roleID);
            string insertSql = string.Format("INSERT INTO {0}(User_ID, Role_ID) VALUES({1},{2})", targetTable, userID, roleID);

            InsertCheckDuplicated(checkSql, insertSql);
        }

        /// <summary>
        /// 通用的插入检查处理
        /// </summary>
        /// <param name="checkSql">检查是否存在语句</param>
        /// <param name="insertSql">插入数据的语句</param>
        private void InsertCheckDuplicated(string checkSql, string insertSql)
        {
            Database db = CreateDatabase();
            //判断是否存在
            DbCommand command = db.GetSqlStringCommand(checkSql);
            int count = base.GetExecuteScalarValue(db, command);
            if (count == 0)
            {
                //执行语句
                command = db.GetSqlStringCommand(insertSql);
                db.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// 为角色指定新的人员列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="newUserList">人员列表</param>
        /// <returns></returns>
        public bool EditRoleUsers(int roleID, List<int> newUserList)
        {
            string sql = string.Format("Delete from T_ACL_User_Role where Role_ID = {0} ", roleID);
            base.SqlExecute(sql);

            foreach (int userId in newUserList)
            {
                AddUser(userId, roleID);
            }
            return true;
        }

        /// <summary>
        /// 为角色指定新的操作功能列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="newFunctionList">功能列表</param>
        /// <returns></returns>
        public bool EditRoleFunctions(int roleID, List<string> newFunctionList, string systemType)
        {
            string sql = string.Format(@"delete from T_ACL_Role_Function Where Function_ID in ");
            
            string condition = string.Format(@"Select t.Function_ID from T_ACL_Role_Function t inner join T_ACL_Function f 
            on t.Function_ID = f.ID where Role_ID ={0}", roleID);

            if(!string.IsNullOrEmpty(systemType))
            {
                condition += string.Format(" AND SystemType_ID='{0}'", systemType);
            }
            sql += string.Format("({0})", condition);

            base.SqlExecute(sql);

            foreach (string functionId in newFunctionList)
            {
                AddFunction(functionId, roleID);
            }
            return true;
        }

        /// <summary>
        /// 为角色指定新的机构列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="newOUList">机构列表</param>
        /// <returns></returns>
        public bool EditRoleOUs(int roleID, List<int> newOUList)
        {
            string sql = string.Format("Delete from T_ACL_OU_Role where Role_ID = {0} ", roleID);
            base.SqlExecute(sql);

            foreach (int ouId in newOUList)
            {
                AddOU(ouId, roleID);
            }
            return true;
        }

        /// <summary>
        /// 根据功能ID获取对应角色列表
        /// </summary>
        /// <param name="functionID">功能ID</param>
        /// <returns></returns>
        public List<RoleInfo> GetRolesByFunction(string functionID)
        {
            string sql = string.Format(@"SELECT * FROM T_ACL_Role 
            INNER JOIN T_ACL_Role_Function On T_ACL_Role.ID=T_ACL_Role_Function.Role_ID WHERE Function_ID ='{0}' ", functionID);
            return this.GetList(sql, null);
        }

        /// <summary>
        /// 根据机构获取对应的角色类别
        /// </summary>
        /// <param name="ouID"></param>
        /// <returns></returns>
        public List<RoleInfo> GetRolesByOU(int ouID)
        {
            string sql = "SELECT * FROM T_ACL_Role INNER JOIN T_ACL_OU_Role ON T_ACL_Role.ID=Role_ID WHERE OU_ID = " + ouID;
            return this.GetList(sql, null);
        }

        /// <summary>
        /// 根据用户ID获取对应的角色列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public List<RoleInfo> GetRolesByUser(int userID)
        {
            string sql = "SELECT * FROM T_ACL_Role INNER JOIN T_ACL_User_Role On T_ACL_Role.ID=T_ACL_User_Role.Role_ID WHERE User_ID = " + userID;
            return this.GetList(sql, null);
        }

        /// <summary>
        /// 为指定角色移除对应功能
        /// </summary>
        /// <param name="functionID">功能ID</param>
        /// <param name="roleID">角色ID</param>
        public void RemoveFunction(string functionID, int roleID)
        {
            string sql = string.Format("DELETE FROM T_ACL_Role_Function WHERE Function_ID='{0}' AND Role_ID={1}", functionID, roleID);
            SqlExecute(sql);
        }

        /// <summary>
        /// 为指定角色移除机构
        /// </summary>
        /// <param name="ouID">机构ID</param>
        /// <param name="roleID">角色ID</param>
        public void RemoveOU(int ouID, int roleID)
        {
            string sql = string.Format("DELETE FROM T_ACL_OU_Role WHERE OU_ID={0} AND Role_ID={1}", ouID, roleID);
            SqlExecute(sql);
        }

        /// <summary>
        /// 为指定角色移除用户
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="roleID">角色ID</param>
        public void RemoveUser(int userID, int roleID)
        {
            string sql = string.Format("DELETE FROM T_ACL_User_Role WHERE User_ID={0} AND Role_ID={1}", userID, roleID);
            SqlExecute(sql);
        }

        /// <summary>
        /// 设置删除标志
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="deleted">是否删除</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public bool SetDeletedFlag(object id, bool deleted = true, DbTransaction trans = null)
        {
            int intDeleted = deleted ? 1 : 0;
            string sql = string.Format("Update {0} Set Deleted={1} Where ID = {2} ", tableName, intDeleted, id);
            return SqlExecute(sql, trans) > 0;
        }


        /// <summary>
        /// 给指定角色添加菜单
        /// </summary>
        /// <param name="menuID">菜单ID</param>
        /// <param name="roleID">角色ID</param>
        public void AddMenu(string menuID, int roleID)
        {
            string sql = string.Format("INSERT INTO T_ACL_Role_Menu(Menu_ID, Role_ID) VALUES('{0}',{1})", menuID, roleID);
            SqlExecute(sql);
        }

        /// <summary>
        /// 为指定角色移除对应菜单
        /// </summary>
        /// <param name="menuID">菜单ID</param>
        /// <param name="roleID">角色ID</param>
        public void RemoveMenu(string menuID, int roleID)
        {
            string sql = string.Format("DELETE FROM T_ACL_Role_Menu WHERE Menu_ID='{0}' AND Role_ID={1}", menuID, roleID);
            SqlExecute(sql);
        }

        /// <summary>
        /// 为角色指定新的菜单列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="newList">菜单列表</param>
        /// <param name="systemType">系统类型</param>
        /// <returns></returns>
        public bool EditRoleMenus(int roleID, List<string> newList, string systemType)
        {
            string sql = string.Format(@"delete from T_ACL_Role_Menu Where Menu_ID in ");

            string condition = string.Format(@"Select t.Menu_ID from T_ACL_Role_Menu t inner join T_ACL_Menu f 
            on t.Menu_ID = f.ID where Role_ID ={0}", roleID);

            if (!string.IsNullOrEmpty(systemType))
            {
                condition += string.Format(" AND SystemType_ID='{0}'", systemType);
            }
            sql += string.Format("({0})", condition);

            base.SqlExecute(sql);

            foreach (string menuId in newList)
            {
                AddMenu(menuId, roleID);
            }
            return true;
        }
    }
}