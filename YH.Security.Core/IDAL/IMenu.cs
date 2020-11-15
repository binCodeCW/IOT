using System.Collections.Generic;

using YH.Security.Entity;
using YH.Framework.ControlUtil;

namespace YH.Security.IDAL
{
    /// <summary>
    /// 功能菜单
    /// </summary>
    public interface IMenu : IBaseDAL<MenuInfo>
    {
        /// <summary>
        /// 获取树形结构的菜单列表
        /// </summary>
        /// <param name="systemType">系统类型名称</param>
        List<MenuNodeInfo> GetTree(string systemType = "");

        /// <summary>
        /// 获取所有的菜单列表
        /// </summary>
        /// <param name="systemType">系统类型名称</param>
        List<MenuInfo> GetAllMenu(string systemType = "");

        /// <summary>
        /// 获取第一级的菜单列表
        /// </summary>
        /// <param name="systemType">系统类型名称</param>
        List<MenuInfo> GetTopMenu(string systemType = "");

        /// <summary>
        /// 获取指定菜单下面的树形列表
        /// </summary>
        /// <param name="id">指定菜单ID</param>
        List<MenuNodeInfo> GetTreeByID(string id);

        /// <summary>
        /// 根据指定的父ID获取其下面一级（仅限一级）的菜单列表
        /// </summary>
        /// <param name="pid">菜单父ID</param>
        List<MenuInfo> GetMenuByID(string pid);


        /// <summary>
        /// 根据角色集合和系统标识获取对应的菜单集合
        /// </summary>
        /// <param name="roleIDs">角色ID集合，逗号分开</param>
        /// <param name="typeID">系统类型名称</param>
        List<MenuNodeInfo> GetMenuNodes(string roleIDs, string typeID);

        /// <summary>
        /// 根据角色ID获取功能集合
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="typeID">系统类型名称</param>
        List<MenuInfo> GetMenusByRole(int roleID, string typeID);
    }
}