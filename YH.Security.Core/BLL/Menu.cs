using System.Collections.Generic;

using YH.Security.Entity;
using YH.Security.IDAL;
using YH.Framework.ControlUtil;

namespace YH.Security.BLL
{
    /// <summary>
    /// 功能菜单
    /// </summary>
	public class Menu : BaseBLL<MenuInfo>
    {
        private IMenu menuDal;

        public Menu() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(YH.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件

            this.menuDal = baseDal as IMenu;
        }
                
        /// <summary>
        /// 获取树形结构的菜单列表
        /// </summary>
        public List<MenuNodeInfo> GetTree(string systemType)
        {
            return menuDal.GetTree(systemType);
        }

        /// <summary>
        /// 获取所有的菜单列表
        /// </summary>
        public List<MenuInfo> GetAllTree(string systemType)
        {
            return menuDal.GetAllMenu(systemType);
        }

        /// <summary>
        /// 获取第一级的菜单列表
        /// </summary>
        public List<MenuInfo> GetTopMenu(string systemType)
        {
            return menuDal.GetTopMenu(systemType);
        }

        /// <summary>
        /// 获取指定菜单下面的树形列表
        /// </summary>
        /// <param name="id">指定菜单ID</param>
        public List<MenuNodeInfo> GetTreeByID(string mainMenuID)
        {
            return menuDal.GetTreeByID(mainMenuID);
        }

        /// <summary>
        /// 根据指定的父ID获取其下面一级（仅限一级）的菜单列表
        /// </summary>
        /// <param name="pid">菜单父ID</param>
        public List<MenuInfo> GetMenuByID(string pid)
        {
            return menuDal.GetMenuByID(pid);
        }



        /*
         * 在引入和角色多对多的关系后，菜单作为角色的资源之一，和功能模块并立。
         * 因此在处理上和Function表的处理类似，作为角色的资源之一。
         */ 
      
        /// <summary>
        /// 根据角色集合和系统标识获取对应的菜单集合
        /// </summary>
        /// <param name="roleIDs">角色ID字符串</param>
        /// <param name="typeID">系统类型</param>
        /// <returns></returns>
        public List<MenuNodeInfo> GetMenuNodes(string roleIDs, string typeID)
        {
            if (roleIDs == string.Empty)
            {
                roleIDs = "-1";
            }
            return this.menuDal.GetMenuNodes(roleIDs, typeID);
        }

        /// <summary>
        /// 根据角色ID获取功能集合
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="typeID">系统类别ID</param>
        /// <returns></returns>
        public List<MenuInfo> GetMenusByRole(int roleID, string typeID)
        {
            return this.menuDal.GetMenusByRole(roleID, typeID);
        }

        /// <summary>
        /// 根据用户ID，获取对应的菜单列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="typeID">系统类别ID</param>
        /// <returns></returns>
        public List<MenuNodeInfo> GetMenuNodesByUser(int userID, string typeID)
        {
            List<RoleInfo> rolesByUser = BLLFactory<Role>.Instance.GetRolesByUser(userID);
            string roleIDs = ",";
            foreach (RoleInfo info in rolesByUser)
            {
                roleIDs = roleIDs + info.ID + ",";
            }
            roleIDs = roleIDs.Trim(',');//移除前后的逗号

            List<MenuNodeInfo> menuList = new List<MenuNodeInfo>();
            if (!string.IsNullOrEmpty(roleIDs))
            {
                menuList = this.GetMenuNodes(roleIDs, typeID);
            }
            return menuList;
        }
    }
}
