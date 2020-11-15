using YH.Security.Entity;
using YH.Framework.ControlUtil;
using System.Collections.Generic;
using System.Data.Common;

namespace YH.Security.IDAL
{
    public interface IRole : IBaseDAL<RoleInfo>
	{
        /// <summary>
        /// 为角色添加功能
        /// </summary>
		void AddFunction(string functionID, int roleID);

        /// <summary>
        /// 为角色添加机构
        /// </summary>
		void AddOU(int ouID, int roleID);

        /// <summary>
        /// 为角色添加用户
        /// </summary>
		void AddUser(int userID, int roleID);
              
        /// <summary>
        /// 为角色指定新的人员列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="newUserList">人员列表</param>
        /// <returns></returns>
        bool EditRoleUsers(int roleID, List<int> newUserList);
                
        /// <summary>
        /// 为角色指定新的操作功能列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="newFunctionList">功能列表</param>
        /// <param name="systemType">系统类型</param>
        /// <returns></returns>
        bool EditRoleFunctions(int roleID, List<string> newFunctionList, string systemType);

        /// <summary>
        /// 为角色指定新的机构列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="newOUList">机构列表</param>
        /// <returns></returns>
        bool EditRoleOUs(int roleID, List<int> newOUList);

        /// <summary>
        /// 根据功能ID，获取拥有的角色列表
        /// </summary>
        /// <param name="functionID">功能ID</param>
        /// <returns></returns>
        List<RoleInfo> GetRolesByFunction(string functionID);

        /// <summary>
        /// 获取包含该机构的角色集合
        /// </summary>
        /// <param name="ouID">机构ID</param>
        /// <returns></returns>
        List<RoleInfo> GetRolesByOU(int ouID);

        /// <summary>
        /// 获取用户所在的角色集合
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        List<RoleInfo> GetRolesByUser(int userID);

        /// <summary>
        /// 移除角色对应的功能
        /// </summary>
		void RemoveFunction(string functionID, int roleID);

        /// <summary>
        /// 移除角色对应的机构
        /// </summary>
		void RemoveOU(int ouID, int roleID);

        /// <summary>
        /// 移除角色包含的用户
        /// </summary>
		void RemoveUser(int userID, int roleID);

        /// <summary>
        /// 设置删除标志
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="deleted">是否删除</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        bool SetDeletedFlag(object id, bool deleted = true, DbTransaction trans = null);

                       
        /// <summary>
        /// 给指定角色添加菜单
        /// </summary>
        /// <param name="menuID">菜单ID</param>
        /// <param name="roleID">角色ID</param>
        void AddMenu(string menuID, int roleID);

        /// <summary>
        /// 为指定角色移除对应菜单
        /// </summary>
        /// <param name="menuID">菜单ID</param>
        /// <param name="roleID">角色ID</param>
        void RemoveMenu(string menuID, int roleID);

        /// <summary>
        /// 为角色指定新的菜单列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="newList">菜单列表</param>
        /// <param name="systemType">系统类型</param>
        /// <returns></returns>
        bool EditRoleMenus(int roleID, List<string> newList, string systemType);
	}
}