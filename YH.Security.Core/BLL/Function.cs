﻿using YH.Security.Entity;
using YH.Security.IDAL;
using YH.Framework.ControlUtil;
using System.Collections.Generic;

namespace YH.Security.BLL
{
    /// <summary>
    /// 系统功能定义
    /// </summary>
    public class Function : BaseBLL<FunctionInfo>
    {
        private IFunction functionDal;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Function()  : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

            functionDal = baseDal as IFunction;
        }

        /// <summary>
        /// 根据角色ID字符串（逗号分开)和系统类型ID，获取对应的操作功能列表
        /// </summary>
        /// <param name="roleIDs">角色ID</param>
        /// <param name="typeID">系统类型ID</param>
        /// <returns></returns>
        public List<FunctionInfo> GetFunctions(string roleIDs, string typeID)
        {
            if (roleIDs == string.Empty)
            {
                roleIDs = "-1";
            }
            return this.functionDal.GetFunctions(roleIDs, typeID);
        }

        /// <summary>
        /// 根据角色ID字符串（逗号分开)和系统类型ID，获取对应的操作功能列表
        /// </summary>
        /// <param name="roleIDs">角色ID</param>
        /// <param name="typeID">系统类型ID</param>
        /// <returns></returns>
        public List<FunctionNodeInfo> GetFunctionNodes(string roleIDs, string typeID)
        {
            if (roleIDs == string.Empty)
            {
                roleIDs = "-1";
            }
            return this.functionDal.GetFunctionNodes(roleIDs, typeID);
        }

        /// <summary>
        /// 根据角色ID获取对应的操作功能列表
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public List<FunctionInfo> GetFunctionsByRole(int roleID)
        {
            return this.functionDal.GetFunctionsByRole(roleID);
        }

        /// <summary>
        /// 根据用户ID，获取对应的功能列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="typeID">系统类别ID</param>
        /// <returns></returns>
        public List<FunctionInfo> GetFunctionsByUser(int userID, string typeID)
        {
            List<RoleInfo> rolesByUser = BLLFactory<Role>.Instance.GetRolesByUser(userID);
            string roleIDs = ",";
            foreach (RoleInfo info in rolesByUser)
            {
                roleIDs = roleIDs + info.ID + ",";
            }
            roleIDs = roleIDs.Trim(',');//移除前后的逗号

            List<FunctionInfo> functions = new List<FunctionInfo>();
            if (!string.IsNullOrEmpty(roleIDs))
            {
                functions = this.GetFunctions(roleIDs, typeID);
            }
            return functions;
        }

        /// <summary>
        /// 根据用户ID，获取对应的功能列表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="typeID">系统类别ID</param>
        /// <returns></returns>
        public List<FunctionNodeInfo> GetFunctionNodesByUser(int userID, string typeID)
        {
            List<RoleInfo> rolesByUser = BLLFactory<Role>.Instance.GetRolesByUser(userID);
            string roleIDs = ",";
            foreach (RoleInfo info in rolesByUser)
            {
                roleIDs = roleIDs + info.ID + ",";
            }
            roleIDs = roleIDs.Trim(',');//移除前后的逗号

            List<FunctionNodeInfo> functions = new List<FunctionNodeInfo>();
            if (!string.IsNullOrEmpty(roleIDs))
            {
                functions = this.GetFunctionNodes(roleIDs, typeID);
            }
            return functions;
        }

        /// <summary>
        /// 获取树形结构的功能列表
        /// </summary>
        /// <param name="systemType">系统类型的OID</param>
        public List<FunctionNodeInfo> GetTree(string systemType)
        {
            return functionDal.GetTree(systemType);
        }

        /// <summary>
        /// 获取指定功能下面的树形列表
        /// </summary>
        /// <param name="mainID">指定功能ID</param>
        public List<FunctionNodeInfo> GetTreeByID(string mainID)
        {
            return functionDal.GetTreeByID(mainID);
        }
                       
        /// <summary>
        /// 根据角色获取树形结构的功能列表
        /// </summary>
        public List<FunctionNodeInfo> GetTreeWithRole(string systemType, List<int> roleList)
        {
            return functionDal.GetTreeWithRole(systemType, roleList);
        }
                      
        /// <summary>
        /// 根据角色获取树形结构的功能列表
        /// </summary>
        public List<FunctionNodeInfo> GetTreeWithUser(string systemType, int userID)
        {
            List<RoleInfo> rolesByUser = BLLFactory<Role>.Instance.GetRolesByUser(userID);
            List<int> roleList = new List<int>();
            foreach (RoleInfo info in rolesByUser)
            {
                roleList.Add(info.ID);
            }

            return GetTreeWithRole(systemType, roleList);
        }

        /// <summary>
        /// 删除指定节点及其子节点。如果该节点含有子节点，子节点也会一并删除
        /// </summary>
        /// <param name="mainID">节点ID</param>
        /// <returns></returns>
        public bool DeleteWithSubNode(string mainID)
        {
            return functionDal.DeleteWithSubNode(mainID);
        }
    }
}