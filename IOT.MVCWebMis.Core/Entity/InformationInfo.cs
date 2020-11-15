using System;
using System.Runtime.Serialization;

using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    public enum InformationCategory { 通知公告, 政策法规, 行业动态, 图片新闻, 待办事项, 其他 };

    /// <summary>
    /// 政策法规公告动态
    /// </summary>
    [DataContract]
    public class InformationInfo : BaseEntity
    {   
        public InformationInfo()
        {
            this.ID = System.Guid.NewGuid().ToString(); //   
            this.Attachment_GUID = System.Guid.NewGuid().ToString(); //附件GUID
            this.Category = InformationCategory.其他; //大类名称 
            this.EditTime = DateTime.Now; //编辑时间
        }

        #region Property Members
        
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
		[DataMember]
        public virtual string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
		[DataMember]
        public virtual string Content { get; set; }

        /// <summary>
        /// 附件GUID
        /// </summary>
		[DataMember]
        public virtual string Attachment_GUID { get; set; }
        /// <summary>
        /// 大类名称
        /// </summary>
		[DataMember]
        public virtual InformationCategory Category { get; set; }

        /// <summary>
        /// 子类名称
        /// </summary>
		[DataMember]
        public virtual string SubType { get; set; }

        /// <summary>
        /// 编辑者
        /// </summary>
		[DataMember]
        public virtual string Editor { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
		[DataMember]
        public virtual DateTime EditTime { get; set; }

        /// <summary>
        /// 是否审批通过
        /// </summary>
		[DataMember]
        public virtual int IsChecked { get; set; }

        /// <summary>
        /// 审批者
        /// </summary>
		[DataMember]
        public virtual string CheckUser { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
		[DataMember]
        public virtual DateTime? CheckTime { get; set; }

        /// <summary>
        /// 是否强制过期
        /// </summary>
		[DataMember]
        public virtual int ForceExpire { get; set; }

        /// <summary>
        /// 过期截止时间
        /// </summary>
		[DataMember]
        public virtual DateTime? TimeOut { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
		[DataMember]
        public virtual int Status { get; set; }


        #endregion

    }
}