using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 产品销售记录
    /// </summary>
    [DataContract]
    public class SellInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public SellInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Quantity = 0;
            this.Amount = 0;
            this.DiscountAmount = 0;
            this.ReceivedMoney = 0;
            this.IsShipped = false;
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
        /// 订单编号
        /// </summary>
        [DataMember]
        public virtual string OrderNo { get; set; }

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
        /// 支付方式
        /// </summary>
        [DataMember]
        public virtual string PaymentType { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        [DataMember]
        public virtual string Operator { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        [DataMember]
        public virtual DateTime OrderDate { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [DataMember]
        public virtual string OrderStatus { get; set; }

        /// <summary>
        /// 是否已发货
        /// </summary>
        [DataMember]
        public virtual bool IsShipped { get; set; }

        /// <summary>
        /// 要求到货日期
        /// </summary>
        [DataMember]
        public virtual DateTime RequiredDate { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [DataMember]
        public virtual string ShipAddress { get; set; }

        /// <summary>
        /// 收货电话
        /// </summary>
        [DataMember]
        public virtual string ShipTelephone { get; set; }

        /// <summary>
        /// 收货人员
        /// </summary>
        [DataMember]
        public virtual string ReceiveMan { get; set; }

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