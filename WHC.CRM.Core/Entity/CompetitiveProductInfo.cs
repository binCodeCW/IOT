using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 竞争对手产品信息
    /// </summary>
    [DataContract]
    public class CompetitiveProductInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public CompetitiveProductInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.PurchasePrice = 0;
            this.SalePrice = 0;
            this.AttachGUID = System.Guid.NewGuid().ToString();
            this.Status = 0;
            this.CreateTime = DateTime.Now;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 竞争对手ID
        /// </summary>
        [DataMember]
        public virtual string Competitor_ID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public virtual string HandNo { get; set; }

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
        /// 成本价
        /// </summary>
        [DataMember]
        public virtual decimal PurchasePrice { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        [DataMember]
        public virtual decimal SalePrice { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [DataMember]
        public virtual string Manufacture { get; set; }

        /// <summary>
        /// 附件组别ID
        /// </summary>
        [DataMember]
        public virtual string AttachGUID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 停用状态[0正常1停用]
        /// </summary>
        [DataMember]
        public virtual int Status { get; set; }

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