using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 信访投诉工作
    /// </summary>
    [DataContract]
    public class AssignWorkInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public AssignWorkInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.AttachGUID = System.Guid.NewGuid().ToString(); //附件组别ID         
            this.Apply_ID = System.Guid.NewGuid().ToString(); //申请单编号 (业务对象负责生成）
        }

        #region Property Members
        
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 工作类别
        /// </summary>
		[DataMember]
        public virtual string Category { get; set; }

        /// <summary>
        /// 紧急程度
        /// </summary>
		[DataMember]
        public virtual string Urgency { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
		[DataMember]
        public virtual string Title { get; set; }

        /// <summary>
        /// 内容摘要
        /// </summary>
		[DataMember]
        public virtual string Abstract { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
		[DataMember]
        public virtual string MainBody { get; set; }

        /// <summary>
        /// 拟办意见
        /// </summary>
		[DataMember]
        public virtual string InitOpinion { get; set; }

        /// <summary>
        /// 回复意见
        /// </summary>
		[DataMember]
        public virtual string ReplyOpinion { get; set; }

        /// <summary>
        /// 回复正文
        /// </summary>
		[DataMember]
        public virtual string ReplyBody { get; set; }

        /// <summary>
        /// 办理附件GUID
        /// </summary>
		[DataMember]
        public virtual string ReplyAttachGUID { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
		[DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 交办单位
        /// </summary>
		[DataMember]
        public virtual string ToDept_ID { get; set; }

        /// <summary>
        /// 交办单位负责人
        /// </summary>
		[DataMember]
        public virtual string DeptManager_ID { get; set; }

        /// <summary>
        /// 过期日期
        /// </summary>
		[DataMember]
        public virtual DateTime ExpiredDate { get; set; }

        /// <summary>
        /// 附件组别ID
        /// </summary>
		[DataMember]
        public virtual string AttachGUID { get; set; }

        /// <summary>
        /// 申请单编号
        /// </summary>
		[DataMember]
        public virtual string Apply_ID { get; set; }

        /// <summary>
        /// 申请单日期
        /// </summary>
		[DataMember]
        public virtual DateTime ApplyDate { get; set; }

        /// <summary>
        /// 申请部门
        /// </summary>
		[DataMember]
        public virtual string ApplyDept { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
		[DataMember]
        public virtual string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
		[DataMember]
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 编辑人
        /// </summary>
		[DataMember]
        public virtual string Editor { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
		[DataMember]
        public virtual DateTime EditTime { get; set; }

        /// <summary>
        /// 分阅人员
        /// </summary>
		[DataMember]
        public virtual string DispatchUsers { get; set; }


        #endregion

    }
}