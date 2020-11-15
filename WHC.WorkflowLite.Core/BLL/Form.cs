using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Pager.Entity;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.BLL
{
    /// <summary>
    /// 表单模板管理
    /// </summary>
    public class Form : BaseBLL<FormInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Form() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 根据表单ID获取对应的名称
        /// </summary>
        /// <param name="id">表单ID</param>
        /// <returns></returns>
        public string GetFormName(object id)
        {
            return baseDal.GetFieldValue(id, "FORM_NAME");
        }
                 
        /// <summary>
        /// 列出指定表的所有字段名称
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public List<string> ListColumns(string tableName)
        {
            IForm dal = baseDal as IForm;
            return dal.ListColumns(tableName);
        }

        /// <summary>
        /// 根据过滤条件，在业务表中获取对应的申请单中的记录数
        /// </summary>
        /// <param name="datatable">表名称</param>
        /// <param name="apply_id">申请单ID</param>
        /// <param name="cond_verify">定义条件</param>
        /// <returns></returns>
        public int GetApplyCount(string datatable, string apply_id, string cond_verify)
        {
            IForm dal = baseDal as IForm;
            return dal.GetApplyCount(datatable, apply_id, cond_verify);
        }

        /// <summary>
        /// 把用户/群组列表中的标记替换为实际的用户或群组。
        /// </summary>
        /// <param name="userlist">用户/群组列表</param>
        /// <param name="user_id">用户id</param>
        /// <returns>
        /// 返回更新后的用户/群组列表
        /// </returns>
        public string GetUserList(string userlist, int user_id)
        {
            return "";

            //UserInfo userInfo = BLLFactory<User>.Instance.FindByID(user_id);
            //DeptInfo groupInfo = null;
            //if (userInfo != null)
            //{
            //    groupInfo = BLLFactory<Dept>.Instance.FindByID(userInfo.DeptId);
            //}

            //#region 申请人解析

            //if (userlist.IndexOf("{$申请人}") >= 0)
            //{
            //    if (userInfo != null && !string.IsNullOrEmpty(userInfo.RealName))
            //    {
            //        userlist = userlist.Replace("{$申请人}", userInfo.RealName);
            //    }
            //}

            //#endregion

            //#region 部门相关解析

            //if (userlist.IndexOf("{$所在部门}") >= 0)
            //{
            //    int grade = 2;
            //    string groupNameList = BLLFactory<Dept>.Instance. GetGroupNameList(user_id);
            //    if (!string.IsNullOrEmpty(groupNameList))
            //    {
            //        userlist = userlist.Replace("{$所在部门}", groupNameList);
            //    }
            //}
            //if (userlist.IndexOf("{$部门领导}") >= 0)
            //{
            //    int grade = 2;
            //    int ismanager = 2;
            //    string userNameList = BLLFactory<User>.Instance.GetUsernameList(user_id, grade, ismanager);
            //    if (!string.IsNullOrEmpty(userNameList))
            //    {
            //        userlist = userlist.Replace("{$部门领导}", userNameList);
            //    }
            //}
            //if (userlist.IndexOf("{$部门员工}") >= 0)
            //{
            //    int grade = 2;
            //    int ismanager = 0;
            //    string userNameList = BLLFactory<User>.Instance.GetUsernameList(user_id, grade, ismanager);
            //    if (!string.IsNullOrEmpty(userNameList))
            //    {
            //        userlist = userlist.Replace("{$部门员工}", userNameList);
            //    }
            //} 

            //#endregion

            //#region 公司相关解析

            //if (userlist.IndexOf("{$所在公司}") >= 0)
            //{
            //    int grade = 1;
            //    string groupNameList = BLLFactory<Dept>.Instance.GetGroupNameList(user_id, grade);
            //    if (!string.IsNullOrEmpty(groupNameList))
            //    {
            //        userlist = userlist.Replace("{$所在公司}", groupNameList);
            //    }
            //}

            //if (userlist.IndexOf("{$公司领导}") >= 0)
            //{
            //    int grade = 1;
            //    int ismanager = 1;
            //    string userNameList = BLLFactory<User>.Instance.GetUsernameList(user_id, grade, ismanager);
            //    if (!string.IsNullOrEmpty(userNameList))
            //    {
            //        userlist = userlist.Replace("{$公司领导}", userNameList);
            //    }
            //}
            //if (userlist.IndexOf("{$公司员工}") >= 0)
            //{
            //    int grade = 1;
            //    int ismanager = 0;
            //    string userNameList = BLLFactory<User>.Instance.GetUsernameList(user_id, grade, ismanager);
            //    if (!string.IsNullOrEmpty(userNameList))
            //    {
            //        userlist = userlist.Replace("{$公司员工}", userNameList);
            //    }
            //} 

            //#endregion

            //#region 其他情况的解析

            //userlist = userlist == null ? "" : userlist.Trim();
            //while (userlist.IndexOf(" ,") >= 0)
            //{
            //    userlist = userlist.Replace(" ,", ",");
            //}
            //while (userlist.IndexOf(", ") >= 0)
            //{
            //    userlist = userlist.Replace(", ", ",");
            //}

            //int count = StringHelper.fieldCount(userlist, ",");
            //for (int i = 1; i <= count; i++)
            //{
            //    string strflag = StringHelper.fieldGet(userlist, i, ",");
            //    if (!strflag.EndsWith("{领导}") && !strflag.EndsWith("{员工}"))
            //        continue;

            //    string dept_name = strflag.Substring(0, strflag.Length - "{领导}".Length).Trim();
            //    string condition = string.Format("name ='{0}'", dept_name);
            //    DeptInfo tmpGroupInfo = BLLFactory<Dept>.Instance.FindSingle(condition);
            //    if (tmpGroupInfo == null)
            //    {
            //        continue;
            //    }

            //    string newvalue = "";
            //    if (strflag.EndsWith("{领导}"))
            //    {
            //        condition = string.Format(@"is_manager=<>0  and dept_id in (select id from tb_acl_group where id in ({0}) or upper_dept in ({1}) and grade=0)",
            //                tmpGroupInfo.Grade, tmpGroupInfo.Id);
            //        List<UserInfo> list = BLLFactory<User>.Instance.Find(condition);

            //        foreach (UserInfo tmpUserInfo in list)
            //        {
            //            newvalue += string.Format("{0},", tmpUserInfo.RealName);
            //        }
            //        newvalue = newvalue.Trim(',');
            //    }
            //    else if( strflag.EndsWith("{员工}"))
            //    {
            //        condition = string.Format(@"is_manager=0  and id in (select user_id from tb_acl_User_group where group_id={0})", tmpGroupInfo.Id);
            //        List<UserInfo> list = BLLFactory<User>.Instance.Find(condition);
            //        foreach (UserInfo tmpUserInfo in list)
            //        {
            //            newvalue += string.Format("{0},", tmpUserInfo.RealName);
            //        }
            //        newvalue = newvalue.Trim(',');
            //    }

            //    userlist = userlist.Replace(strflag, newvalue);
            //}

            //while (userlist.IndexOf(",,") >= 0)
            //{
            //    userlist = userlist.Replace(",,", ",");
            //}
            //if (userlist == ",")
            //{
            //    userlist = "";
            //} 

            //#endregion

            //return userlist;
        }

        /// <summary>
        /// 返回当前用户所有可创建的表单列表(不包含ＷＨＥＲＥ的条件语句）
        /// </summary>
        public string GetFormCreateCondition(int userId)
        {
            //string group_name = BLLFactory<Dept>.Instance.GetGroupsByUserID(userId).DeptName;

            ////获取可以访问的表单列表
            //string idList = "";
            //List<AppFormInfo> appFormList = baseDal.GetAll();
            //foreach (AppFormInfo formInfo in appFormList)
            //{
            //    if (string.IsNullOrEmpty(formInfo.WhoCreate) || 
            //        StringHelper.exists(formInfo.WhoCreate, group_name))
            //    {
            //        idList += string.Format("{0},", formInfo.Id);
            //    }
            //}
            //idList = idList.Trim(',');

            ////组装Sql条件语句，用于分页
            //string sql = "";
            //if (!string.IsNullOrEmpty(idList))
            //{
            //    sql = string.Format("id in({0}) ", idList);
            //}

            //return sql;

            return "";
        }
    }
}
