using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 报销申请单
    /// </summary>
    [DataContract]
    public class ReimbursementInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public ReimbursementInfo()
		{
            this.ID = System.Guid.NewGuid().ToString();
            this.AttachGUID = System.Guid.NewGuid().ToString();
            this.Apply_ID = System.Guid.NewGuid().ToString();//必须初始化          
		}

        #region Property Members
        
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 报销类型
        /// </summary>
		[DataMember]
        public virtual string Category { get; set; }

        /// <summary>
        /// 报销事由
        /// </summary>
		[DataMember]
        public virtual string Reason { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
		[DataMember]
        public virtual decimal TotalAmount { get; set; }

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
        /// 备注信息
        /// </summary>
		[DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 附件组别ID
        /// </summary>
        [DataMember]
        public virtual string AttachGUID { get; set; }

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


        #endregion

    }
}