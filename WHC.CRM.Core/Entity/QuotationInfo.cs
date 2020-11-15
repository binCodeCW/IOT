using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 产品报价单
    /// </summary>
    [DataContract]
    public class QuotationInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public QuotationInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Quantity = 0;
            this.Amount = 0;
            this.DiscountAmount = 0;
            this.ReceivedMoney = 0;
            this.AttachGUID = System.Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [DataMember]
        public virtual string Customer_ID { get; set; }

        /// <summary>
        /// 客户联系人ID
        /// </summary>
        [DataMember]
        public virtual string Conatct_ID { get; set; }

        /// <summary>
        /// 报价单编号
        /// </summary>
        [DataMember]
        public virtual string HandNo { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember]
        public virtual string Contact { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        [DataMember]
        public virtual string ContactPhone { get; set; }

        /// <summary>
        /// 联系人手机
        /// </summary>
        [DataMember]
        public virtual string ContactMobile { get; set; }

        /// <summary>
        /// 销售数量
        /// </summary>
        [DataMember]
        public virtual int Quantity { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        [DataMember]
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// 折后金额
        /// </summary>
        [DataMember]
        public virtual decimal DiscountAmount { get; set; }

        /// <summary>
        /// 已收金额
        /// </summary>
        [DataMember]
        public virtual decimal ReceivedMoney { get; set; }

        /// <summary>
        /// 优惠折扣
        /// </summary>
        [DataMember]
        public virtual string DiscountNote { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        [DataMember]
        public virtual string Operator { get; set; }

        /// <summary>
        /// 报价单日期
        /// </summary>
        [DataMember]
        public virtual DateTime OrderDate { get; set; }

        /// <summary>
        /// 报价单状态
        /// </summary>
        [DataMember]
        public virtual string OrderStatus { get; set; }

        /// <summary>
        /// 附件组别ID
        /// </summary>
        [DataMember]
        public virtual string AttachGUID { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }

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
        /// 所属部门
        /// </summary>
        [DataMember]
        public virtual string Dept_ID { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        [DataMember]
        public virtual string Company_ID { get; set; }


        #endregion

    }
}