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
    /// ��ģ�����
    /// </summary>
    public class Form : BaseBLL<FormInfo>
    {
        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public Form() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// ���ݱ�ID��ȡ��Ӧ������
        /// </summary>
        /// <param name="id">��ID</param>
        /// <returns></returns>
        public string GetFormName(object id)
        {
            return baseDal.GetFieldValue(id, "FORM_NAME");
        }
                 
        /// <summary>
        /// �г�ָ����������ֶ�����
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns></returns>
        public List<string> ListColumns(string tableName)
        {
            IForm dal = baseDal as IForm;
            return dal.ListColumns(tableName);
        }

        /// <summary>
        /// ���ݹ�����������ҵ����л�ȡ��Ӧ�����뵥�еļ�¼��
        /// </summary>
        /// <param name="datatable">������</param>
        /// <param name="apply_id">���뵥ID</param>
        /// <param name="cond_verify">��������</param>
        /// <returns></returns>
        public int GetApplyCount(string datatable, string apply_id, string cond_verify)
        {
            IForm dal = baseDal as IForm;
            return dal.GetApplyCount(datatable, apply_id, cond_verify);
        }

        /// <summary>
        /// ���û�/Ⱥ���б��еı���滻Ϊʵ�ʵ��û���Ⱥ�顣
        /// </summary>
        /// <param name="userlist">�û�/Ⱥ���б�</param>
        /// <param name="user_id">�û�id</param>
        /// <returns>
        /// ���ظ��º���û�/Ⱥ���б�
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

            //#region �����˽���

            //if (userlist.IndexOf("{$������}") >= 0)
            //{
            //    if (userInfo != null && !string.IsNullOrEmpty(userInfo.RealName))
            //    {
            //        userlist = userlist.Replace("{$������}", userInfo.RealName);
            //    }
            //}

            //#endregion

            //#region ������ؽ���

            //if (userlist.IndexOf("{$���ڲ���}") >= 0)
            //{
            //    int grade = 2;
            //    string groupNameList = BLLFactory<Dept>.Instance. GetGroupNameList(user_id);
            //    if (!string.IsNullOrEmpty(groupNameList))
            //    {
            //        userlist = userlist.Replace("{$���ڲ���}", groupNameList);
            //    }
            //}
            //if (userlist.IndexOf("{$�����쵼}") >= 0)
            //{
            //    int grade = 2;
            //    int ismanager = 2;
            //    string userNameList = BLLFactory<User>.Instance.GetUsernameList(user_id, grade, ismanager);
            //    if (!string.IsNullOrEmpty(userNameList))
            //    {
            //        userlist = userlist.Replace("{$�����쵼}", userNameList);
            //    }
            //}
            //if (userlist.IndexOf("{$����Ա��}") >= 0)
            //{
            //    int grade = 2;
            //    int ismanager = 0;
            //    string userNameList = BLLFactory<User>.Instance.GetUsernameList(user_id, grade, ismanager);
            //    if (!string.IsNullOrEmpty(userNameList))
            //    {
            //        userlist = userlist.Replace("{$����Ա��}", userNameList);
            //    }
            //} 

            //#endregion

            //#region ��˾��ؽ���

            //if (userlist.IndexOf("{$���ڹ�˾}") >= 0)
            //{
            //    int grade = 1;
            //    string groupNameList = BLLFactory<Dept>.Instance.GetGroupNameList(user_id, grade);
            //    if (!string.IsNullOrEmpty(groupNameList))
            //    {
            //        userlist = userlist.Replace("{$���ڹ�˾}", groupNameList);
            //    }
            //}

            //if (userlist.IndexOf("{$��˾�쵼}") >= 0)
            //{
            //    int grade = 1;
            //    int ismanager = 1;
            //    string userNameList = BLLFactory<User>.Instance.GetUsernameList(user_id, grade, ismanager);
            //    if (!string.IsNullOrEmpty(userNameList))
            //    {
            //        userlist = userlist.Replace("{$��˾�쵼}", userNameList);
            //    }
            //}
            //if (userlist.IndexOf("{$��˾Ա��}") >= 0)
            //{
            //    int grade = 1;
            //    int ismanager = 0;
            //    string userNameList = BLLFactory<User>.Instance.GetUsernameList(user_id, grade, ismanager);
            //    if (!string.IsNullOrEmpty(userNameList))
            //    {
            //        userlist = userlist.Replace("{$��˾Ա��}", userNameList);
            //    }
            //} 

            //#endregion

            //#region ��������Ľ���

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
            //    if (!strflag.EndsWith("{�쵼}") && !strflag.EndsWith("{Ա��}"))
            //        continue;

            //    string dept_name = strflag.Substring(0, strflag.Length - "{�쵼}".Length).Trim();
            //    string condition = string.Format("name ='{0}'", dept_name);
            //    DeptInfo tmpGroupInfo = BLLFactory<Dept>.Instance.FindSingle(condition);
            //    if (tmpGroupInfo == null)
            //    {
            //        continue;
            //    }

            //    string newvalue = "";
            //    if (strflag.EndsWith("{�쵼}"))
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
            //    else if( strflag.EndsWith("{Ա��}"))
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
        /// ���ص�ǰ�û����пɴ����ı��б�(�������ףȣţңŵ�������䣩
        /// </summary>
        public string GetFormCreateCondition(int userId)
        {
            //string group_name = BLLFactory<Dept>.Instance.GetGroupsByUserID(userId).DeptName;

            ////��ȡ���Է��ʵı��б�
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

            ////��װSql������䣬���ڷ�ҳ
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
