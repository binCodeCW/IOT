using System;
using System.Collections.Generic;

using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using YH.Security.BLL;
using YH.Security.Entity;

namespace IOT.MVCWebMis.Controllers
{    /// <summary>
     /// 增加一个辅助类，操作和权限系统相关的资源，以便使得权限和工作流相对独立使用。
     /// </summary>
    internal class SecurityHelper
    {
        private static bool InUserList(List<UserInfo> list, UserInfo userInfo)
        {
            bool result = false;
            foreach (UserInfo info in list)
            {
                if (info.ID == userInfo.ID)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据字典对象获取用户列表
        /// </summary>
        /// <param name="dict">UserID:RoleID:DeptID:等键值</param>
        /// <returns></returns>
        public static List<UserInfo> GetUsersByDict(Dictionary<string, string> dict)
        {
            List<UserInfo> list = new List<UserInfo>();
            foreach (string key in dict.Keys)
            {
                #region 转义并转换
                //RoleID:1  UserID:1  DeptID:1
                if (key.IndexOf(':') > 0)
                {
                    string[] typeValue = key.Split(':');
                    string type = typeValue[0].ToLower();//RoleID UserID DeptID
                    string value = typeValue[1];//1

                    if (type == "roleid")
                    {
                        List<UserInfo> tmpList = BLLFactory<User>.Instance.GetUsersByRole(value.ToInt32());
                        foreach (UserInfo info in tmpList)
                        {
                            if (!InUserList(list, info))
                            {
                                list.Add(info);
                            }
                        }
                    }
                    else if (type == "deptid")
                    {
                        List<UserInfo> tmpList = BLLFactory<User>.Instance.FindByDept(value.ToInt32());
                        foreach (UserInfo info in tmpList)
                        {
                            if (!InUserList(list, info))
                            {
                                list.Add(info);
                            }
                        }
                    }
                    else if (type == "userid")
                    {
                        UserInfo info = BLLFactory<User>.Instance.FindByID(value.ToInt32());
                        if (info != null && !InUserList(list, info))
                        {
                            list.Add(info);
                        }
                    }
                }
                else
                {
                    //正常的userid解析
                    var userInfo = BLLFactory<User>.Instance.FindByID(key);
                    if (userInfo != null)
                    {
                        list.Add(userInfo);
                    }
                }
                #endregion
            }
            return list;
        }


        /// <summary>
        /// 根据Json字符串，转换为用户列表对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns></returns>
        public static List<UserInfo> GetUsersByJson(string json)
        {
            List<UserInfo> list = new List<UserInfo>();
            if (!string.IsNullOrEmpty(json))
            {
                //userid,roleid,deptid等键值字典
                Dictionary<string, string> dict = json.ToDictObject();
                if (dict != null)
                {
                    var users = GetUsersByDict(dict);
                    if (users != null)
                    {
                        list.AddRange(users);
                    }
                }
                else
                {
                    //默认是用户ID，如果能转换为整形，则从数据库获取用户信息
                    int userId = 0;
                    if (int.TryParse(json, out userId))
                    {
                        UserInfo info = BLLFactory<User>.Instance.FindByID(userId);
                        if (info != null && !InUserList(list, info))
                        {
                            list.Add(info);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 根据用户的ID，获取用户的全名，并放到缓存里面
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static string GetFullNameByID(string userId)
        {
            string result = "";
            if (!string.IsNullOrEmpty(userId))
            {
                System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
                string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, userId);

                result = MemoryCacheHelper.GetCacheItem<string>(key,
                    delegate () { return BLLFactory<User>.Instance.GetFullNameByID(userId.ToInt32()); },
                    new TimeSpan(0, 30, 0));//30分钟过期
            }
            return result;
        }


        /// <summary>
        /// 根据用户部门的ID，获取部门名称，并放到缓存里面
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public static string GetDeptNameByID(string deptId)
        {
            string result = "";
            if (!string.IsNullOrEmpty(deptId))
            {
                System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
                string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, deptId);

                result = MemoryCacheHelper.GetCacheItem<string>(key,
                    delegate () { return BLLFactory<OU>.Instance.GetFieldValue(deptId, "Name"); },
                    new TimeSpan(0, 30, 0));//30分钟过期
            }
            return result;
        }

        /// <summary>
        /// 获取用户角色集合
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static List<RoleInfo> GetRoleList(int userId)
        {
            System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
            string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, userId);

            List<RoleInfo> roleList = MemoryCacheHelper.GetCacheItem<List<RoleInfo>>(key,
                delegate () { return BLLFactory<YH.Security.BLL.Role>.Instance.GetRolesByUser(userId); },
                new TimeSpan(0, 30, 0));//30分钟过期
            return roleList;
        }

        /// <summary>
        /// 根据当前用户身份，获取对应的顶级机构管理节点。
        /// 如果是超级管理员，返回集团节点；如果是公司管理员，返回其公司节点
        /// </summary>
        /// <returns></returns>
        public static List<OUInfo> GetMyTopGroup(LoginUserInfo userInfo)
        {
            List<OUInfo> list = new List<OUInfo>();
            string key = "Security_MyTopGroup" + userInfo.ID;
            List<OUInfo> returnList = MemoryCacheHelper.GetCacheItem<List<OUInfo>>(key,
                delegate ()
                {
                    if (UserInRole(RoleInfo.SuperAdminName, userInfo.ID.ToInt32()))
                    {
                        list.AddRange(BLLFactory<OU>.Instance.GetTopGroup());//超级管理员取集团节点
                    }
                    else
                    {
                        OUInfo groupInfo = BLLFactory<OU>.Instance.FindByID(userInfo.CompanyId);//公司管理员取公司节点
                        list.Add(groupInfo);
                    }

                    return list;
                },
                new TimeSpan(0, 30, 0));//30分钟过期
            return returnList;
        }

        /// <summary>
        /// 判断当前用户具有某个角色
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public static bool UserInRole(string roleName, int userId)
        {
            List<RoleInfo> roleList = GetRoleList(userId);
            bool result = false;
            if (roleList != null)
            {
                foreach (RoleInfo info in roleList)
                {
                    if (info.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
    }
}