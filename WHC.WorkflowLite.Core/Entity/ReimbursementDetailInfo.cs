using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 报销明细
    /// </summary>
    [DataContract]
    public class ReimbursementDetailInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ReimbursementDetailInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.AttachGUID = System.Guid.NewGuid().ToString();

        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 申请单编号
        /// </summary>
        [DataMember]
        public virtual string Apply_ID { get; set; }

        /// <summary>
        /// 主表单头ID
        /// </summary>
        [DataMember]
        public virtual string Header_ID { get; set; }

        /// <summary>
        /// 费用类型
        /// </summary>
        [DataMember]
        public virtual string FeeType { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        [DataMember]
        public virtual DateTime OccurTime { get; set; }

        /// <summary>
        /// 费用金额
        /// </summary>
        [DataMember]
        public virtual decimal FeeAmount { get; set; }

        /// <summary>
        /// 费用说明
        /// </summary>
        [DataMember]
        public virtual string FeeDescription { get; set; }

        /// <summary>
        /// 附件组别ID
        /// </summary>
        [DataMember]
        public virtual string AttachGUID { get; set; }


        #endregion

    }
}