using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 流程模板
    /// </summary>
    [DataContract]
    public class FormInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public FormInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.MayCancel = true; //可否撤销(在流程未结束前，用户是否可以取消该申请) 
            this.InformFinish = false; //是否通知处理人（完成）  
            this.InformRefuse = false; //是否通知处理人（退回）   
            this.InformCancel = false; //是否通知处理人（撤销）  
            this.SendMail = false; //通知方式（邮件）     
            this.SendBroad = false; //通知方式（内部广播）  
            this.SendMobile = false; //通知方式（短信）   
            this.Forbid = false; //是否禁用      
            this.Deleted = 0;
            this.Edittime = System.DateTime.Now; //创建日期      
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 表单分类
        /// </summary>
        [DataMember]
        public virtual string Category { get; set; }

        /// <summary>
        /// 流程模板名称
        /// </summary>
        [DataMember]
        public virtual string FormName { get; set; }

        /// <summary>
        /// 创建流程的url
        /// </summary>
        [DataMember]
        public virtual string ApplyUrl { get; set; }

        /// <summary>
        /// 显示流程处理页面url
        /// </summary>
        [DataMember]
        public virtual string ApplyUrl2 { get; set; }

        /// <summary>
        /// 创建流程的窗体类
        /// </summary>
        [DataMember]
        public virtual string ApplyWin { get; set; }

        /// <summary>
        /// 显示流程处理窗体类
        /// </summary>
        [DataMember]
        public virtual string ApplyWin2 { get; set; }

        /// <summary>
        /// 显示流程处理列表界面
        /// </summary>
        [DataMember]
        public virtual string ApplyWinList { get; set; }
        
        /// <summary>
        /// 对应的数据表
        /// </summary>
        [DataMember]
        public virtual string DataTable { get; set; }

        /// <summary>
        /// 可创建者
        /// </summary>
        [DataMember]
        public virtual string WhoCreate { get; set; }

        /// <summary>
        /// 可浏览者
        /// </summary>
        [DataMember]
        public virtual string WhoBrowse { get; set; }

        /// <summary>
        /// 要通知谁(该申请单完成后要自动通知哪些人)
        /// </summary>
        [DataMember]
        public virtual string WhoInform { get; set; }

        /// <summary>
        /// 可否撤销(在流程未结束前，用户是否可以取消该申请)
        /// </summary>
        [DataMember]
        public virtual bool MayCancel { get; set; }

        /// <summary>
        /// 是否通知处理人（完成）
        /// </summary>
        [DataMember]
        public virtual bool InformFinish { get; set; }

        /// <summary>
        /// 是否通知处理人（退回）
        /// </summary>
        [DataMember]
        public virtual bool InformRefuse { get; set; }

        /// <summary>
        /// 是否通知处理人（撤销）
        /// </summary>
        [DataMember]
        public virtual bool InformCancel { get; set; }

        /// <summary>
        /// 通知方式（邮件）
        /// </summary>
        [DataMember]
        public virtual bool SendMail { get; set; }

        /// <summary>
        /// 通知方式（内部广播）
        /// </summary>
        [DataMember]
        public virtual bool SendBroad { get; set; }

        /// <summary>
        /// 通知方式（短信）
        /// </summary>
        [DataMember]
        public virtual bool SendMobile { get; set; }

        /// <summary>
        /// 表单标识
        /// </summary>
        [DataMember]
        public virtual string FormFlag { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [DataMember]
        public virtual bool Forbid { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [DataMember]
        public virtual string Editor { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember]
        public virtual DateTime Edittime { get; set; }

        /// <summary>
        /// 是否标记删除
        /// </summary>
        [DataMember]
        public virtual int Deleted { get; set; }


        #endregion

    }
}