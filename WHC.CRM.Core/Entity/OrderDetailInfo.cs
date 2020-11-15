using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 订单明细
    /// </summary>
    [DataContract]
    public class OrderDetailInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public OrderDetailInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Quantity = 0;
            this.SalePrice = 0;
            this.SubAmout = 0;
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
        /// 产品ID
        /// </summary>
        [DataMember]
        public virtual string Product_ID { get; set; }

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
        /// 产品编码
        /// </summary>
        [DataMember]
        public virtual string ProductNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [DataMember]
        public virtual string MaterialCode { get; set; }

        /// <summary>
        /// 条形码
        /// </summary>
        [DataMember]
        public virtual string BarCode { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        [DataMember]
        public virtual string ProductType { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember]
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        [DataMember]
        public virtual string PinyinCode { get; set; }

        /// <summary>
        /// 产品规格
        /// </summary>
        [DataMember]
        public virtual string Specification { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        [DataMember]
        public virtual string Model { get; set; }

        /// <summary>
        /// 标准单位
        /// </summary>
        [DataMember]
        public virtual string Unit { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [DataMember]
        public virtual string Color { get; set; }

        /// <summary>
        /// 尺寸
        /// </summary>
        [DataMember]
        public virtual string ProductSize { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public virtual int Quantity { get; set; }

        /// <summary>
        /// 销售单价
        /// </summary>
        [DataMember]
        public virtual decimal SalePrice { get; set; }

        /// <summary>
        /// 金额小结
        /// </summary>
        [DataMember]
        public virtual decimal SubAmout { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        [DataMember]
        public virtual DateTime ExpireDate { get; set; }

        /// <summary>
        /// 备注说明
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