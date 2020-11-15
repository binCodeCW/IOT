using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 销售机会
    /// </summary>
    [DataContract]
    [Serializable]
    public class SaleChanceInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public SaleChanceInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Budget = 0;
            this.Stage = 0;
            this.CompetitiveIndex = 0;
            this.ConfidenceIndex = 0;
            this.AttachGUID = System.Guid.NewGuid().ToString();
        }

        #region Property Members
        
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
		[DataMember]
        public virtual string HandNo { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
		[DataMember]
        public virtual string Customer_ID { get; set; }

        /// <summary>
        /// 机会名称
        /// </summary>
		[DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 机会类型
        /// </summary>
		[DataMember]
        public virtual string ChanceType { get; set; }

        /// <summary>
        /// 预计接单日期
        /// </summary>
		[DataMember]
        public virtual DateTime PreOrderDate { get; set; }

        /// <summary>
        /// 实际接单日期
        /// </summary>
		[DataMember]
        public virtual DateTime ActalOrderDate { get; set; }

        /// <summary>
        /// 项目预算
        /// </summary>
		[DataMember]
        public virtual double Budget { get; set; }

        /// <summary>
        /// 机会来源
        /// </summary>
		[DataMember]
        public virtual string Source { get; set; }

        /// <summary>
        /// 进展阶段
        /// </summary>
		[DataMember]
        public virtual double Stage { get; set; }

        /// <summary>
        /// 阶段停留时间
        /// </summary>
		[DataMember]
        public virtual string StageStayTime { get; set; }

        /// <summary>
        /// 竞争指数
        /// </summary>
		[DataMember]
        public virtual double CompetitiveIndex { get; set; }

        /// <summary>
        /// 信心指数
        /// </summary>
		[DataMember]
        public virtual double ConfidenceIndex { get; set; }

        /// <summary>
        /// 机会状态
        /// </summary>
		[DataMember]
        public virtual string Status { get; set; }

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

        /// <summary>
        /// 业务分享用户
        /// </summary>
        [DataMember]
        public virtual string ShareUsers { get; set; }

        #endregion

    }
}