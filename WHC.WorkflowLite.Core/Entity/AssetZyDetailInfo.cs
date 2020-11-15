using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 资产转移明细
    /// </summary>
    [DataContract]
    [Serializable]
    public class AssetZyDetailInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public AssetZyDetailInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
        }

        #region Property Members
        
        /// <summary>
        /// 序号
        /// </summary>
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 转移单号
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
        /// 转移部门
        /// </summary>
		[DataMember]
        public virtual string ZyDept { get; set; }

        /// <summary>
        /// 原使用人
        /// </summary>
		[DataMember]
        public virtual string OldUser { get; set; }

        /// <summary>
        /// 原存放地点
        /// </summary>
		[DataMember]
        public virtual string OldAddr { get; set; }

        /// <summary>
        /// 现使用人
        /// </summary>
		[DataMember]
        public virtual string CurrUser { get; set; }

        /// <summary>
        /// 现存放地点
        /// </summary>
		[DataMember]
        public virtual string CurrAddr { get; set; }

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