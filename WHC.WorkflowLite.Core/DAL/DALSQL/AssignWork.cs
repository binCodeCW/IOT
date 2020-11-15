using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;

namespace WHC.WorkflowLite.DALSQL
{
    /// <summary>
    /// 信访投诉工作
    /// </summary>
    public class AssignWork : BaseDALSQL<AssignWorkInfo>, IAssignWork
    {
        #region 对象实例及构造函数

        public static AssignWork Instance
        {
            get
            {
                return new AssignWork();
            }
        }
        public AssignWork() : base("TW_AssignWork", "ID")
        {
            this.SortField = "CreateTime";
            this.IsDescending = true;
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override AssignWorkInfo DataReaderToEntity(IDataReader dataReader)
        {
            AssignWorkInfo info = new AssignWorkInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.Category = reader.GetString("Category");
            info.Urgency = reader.GetString("Urgency");
            info.Title = reader.GetString("Title");
            info.Abstract = reader.GetString("Abstract");
            info.MainBody = reader.GetString("MainBody");
            info.InitOpinion = reader.GetString("InitOpinion");
            info.ReplyOpinion = reader.GetString("ReplyOpinion");
            info.ReplyBody = reader.GetString("ReplyBody");
            info.ReplyAttachGUID = reader.GetString("ReplyAttachGUID");
            info.Note = reader.GetString("Note");
            info.ToDept_ID = reader.GetString("ToDept_ID");
            info.DeptManager_ID = reader.GetString("DeptManager_ID");
            info.ExpiredDate = reader.GetDateTime("ExpiredDate");
            info.AttachGUID = reader.GetString("AttachGUID");
            info.Apply_ID = reader.GetString("Apply_ID");
            info.ApplyDate = reader.GetDateTime("ApplyDate");
            info.ApplyDept = reader.GetString("ApplyDept");
            info.Creator = reader.GetString("Creator");
            info.CreateTime = reader.GetDateTime("CreateTime");
            info.Editor = reader.GetString("Editor");
            info.EditTime = reader.GetDateTime("EditTime");
            info.DispatchUsers = reader.GetString("DispatchUsers");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(AssignWorkInfo obj)
        {
            AssignWorkInfo info = obj as AssignWorkInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("Category", info.Category);
            hash.Add("Urgency", info.Urgency);
            hash.Add("Title", info.Title);
            hash.Add("Abstract", info.Abstract);
            hash.Add("MainBody", info.MainBody);
            hash.Add("InitOpinion", info.InitOpinion);
            hash.Add("ReplyOpinion", info.ReplyOpinion);
            hash.Add("ReplyBody", info.ReplyBody);
            hash.Add("ReplyAttachGUID", info.ReplyAttachGUID);
            hash.Add("Note", info.Note);
            hash.Add("ToDept_ID", info.ToDept_ID);
            hash.Add("DeptManager_ID", info.DeptManager_ID);
            hash.Add("ExpiredDate", info.ExpiredDate);
            hash.Add("AttachGUID", info.AttachGUID);
            hash.Add("Apply_ID", info.Apply_ID);
            hash.Add("ApplyDate", info.ApplyDate);
            hash.Add("ApplyDept", info.ApplyDept);
            hash.Add("Creator", info.Creator);
            hash.Add("CreateTime", info.CreateTime);
            hash.Add("Editor", info.Editor);
            hash.Add("EditTime", info.EditTime);
            hash.Add("DispatchUsers", info.DispatchUsers);

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
            dict.Add("Category", "工作类别");
            dict.Add("Urgency", "紧急程度");
            dict.Add("Title", "标题");
            dict.Add("Abstract", "内容摘要");
            dict.Add("MainBody", "正文");
            dict.Add("InitOpinion", "拟办意见");
            dict.Add("ReplyOpinion", "回复意见");
            dict.Add("ReplyBody", "回复正文");
            dict.Add("ReplyAttachGUID", "办理附件GUID");
            dict.Add("Note", "备注信息");
            dict.Add("ToDept_ID", "交办单位");
            dict.Add("DeptManager_ID", "交办单位负责人");
            dict.Add("ExpiredDate", "过期日期");
            dict.Add("AttachGUID", "附件组别ID");
            dict.Add("Apply_ID", "申请单编号");
            dict.Add("ApplyDate", "申请单日期");
            dict.Add("ApplyDept", "申请部门");
            dict.Add("Creator", "创建人");
            dict.Add("CreateTime", "创建时间");
            dict.Add("Editor", "编辑人");
            dict.Add("EditTime", "编辑时间");
            dict.Add("DispatchUsers", "分阅人员");
            #endregion

            return dict;
        }

    }
}