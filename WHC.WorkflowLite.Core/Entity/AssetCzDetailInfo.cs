using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 资产处置明细
    /// </summary>
    [DataContract]
    [Serializable]
    public class AssetCzDetailInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public AssetCzDetailInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Price = 0;
            this.TotalQty = 0;
            this.TotalAmount = 0;

        }

        #region Property Members

        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 处置单号
        /// </summary>
		[DataMember]
        public virtual string BillNo { get; set; }

        /// <summary>
        /// 资产编号
        /// </summary>
		[DataMember]
        public virtual string AssetCode { get; set; }

        /// <summary>
        /// 资产名称
        /// </summary>
		[DataMember]
        public virtual string AssetName { get; set; }

        /// <summary>
        /// 处置部门
        /// </summary>
		[DataMember]
        public virtual string CzDept { get; set; }

        /// <summary>
        /// 使用人
        /// </summary>
		[DataMember]
        public virtual string UsePerson { get; set; }

        /// <summary>
        /// 使用地点
        /// </summary>
		[DataMember]
        public virtual string UseAddr { get; set; }

        /// <summary>
        /// 存放地点
        /// </summary>
		[DataMember]
        public virtual string KeepAddr { get; set; }

        /// <summary>
        /// 处置方式
        /// </summary>
		[DataMember]
        public virtual string DisposeType { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
		[DataMember]
        public virtual string Unit { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
		[DataMember]
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
		[DataMember]
        public virtual int TotalQty { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
		[DataMember]
        public virtual decimal TotalAmount { get; set; }

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


        #endregion

    }
}