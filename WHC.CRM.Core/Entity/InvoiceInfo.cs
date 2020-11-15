using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 客户开票信息
    /// </summary>
    [DataContract]
    public class InvoiceInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public InvoiceInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.TotalMoney = 0;
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
        /// 编号
        /// </summary>
        [DataMember]
        public virtual string HandNo { get; set; }

        /// <summary>
        /// 发票代码
        /// </summary>
        [DataMember]
        public virtual string InvoiceCode { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        [DataMember]
        public virtual string InvoiceNo { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [DataMember]
        public virtual string InvoiceTitle { get; set; }

        /// <summary>
        /// 发票项目名称
        /// </summary>
        [DataMember]
        public virtual string ItemName { get; set; }

        /// <summary>
        /// 发票总金额
        /// </summary>
        [DataMember]
        public virtual decimal TotalMoney { get; set; }

        /// <summary>
        /// 开票日期
        /// </summary>
        [DataMember]
        public virtual DateTime InvoiceDate { get; set; }

        /// <summary>
        /// 开票人
        /// </summary>
        [DataMember]
        public virtual string InvoiceMan { get; set; }

        /// <summary>
        /// 发票状态
        /// </summary>
        [DataMember]
        public virtual string Status { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [DataMember]
        public virtual string ReceiveAddress { get; set; }

        /// <summary>
        /// 收货邮编
        /// </summary>
        [DataMember]
        public virtual string ReceiveZipCode { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        [DataMember]
        public virtual string ReceiveMan { get; set; }

        /// <summary>
        /// 收货人手机
        /// </summary>
        [DataMember]
        public virtual string ReceiveMobile { get; set; }

        /// <summary>
        /// 收货人固话
        /// </summary>
        [DataMember]
        public virtual string ReceivePhone { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        [DataMember]
        public virtual string Seq { get; set; }

        /// <summary>
        /// 备注
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