using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 付款申请单
    /// </summary>
    [DataContract]
    public class PaymentInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public PaymentInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.PayAmount = 0;
            this.AttachGUID = System.Guid.NewGuid().ToString();
            this.Apply_ID = System.Guid.NewGuid().ToString(); //申请单编号(业务对象负责生成）

        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 付款事由
        /// </summary>
        [DataMember]
        public virtual string Reason { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        [DataMember]
        public virtual decimal PayAmount { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        [DataMember]
        public virtual string PayType { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        [DataMember]
        public virtual DateTime PayDate { get; set; }

        /// <summary>
        /// 收款人全称
        /// </summary>
        [DataMember]
        public virtual string PayeeFullName { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        [DataMember]
        public virtual string BankAccount { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        [DataMember]
        public virtual string Bank { get; set; }

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