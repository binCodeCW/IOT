using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 销售阶段跟进提醒
    /// </summary>
    [DataContract]
    [Serializable]
    public class SaleChanceRemindInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public SaleChanceRemindInfo()
		{
            this.ID= System.Guid.NewGuid().ToString();
                     this.AttachGUID= System.Guid.NewGuid().ToString();
         
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
        /// 销售机会ID
        /// </summary>
		[DataMember]
        public virtual string SaleChance_ID { get; set; }

        /// <summary>
        /// 跟进阶段
        /// </summary>
		[DataMember]
        public virtual string Stage { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
		[DataMember]
        public virtual string Receiver { get; set; }

        /// <summary>
        /// 重要程度
        /// </summary>
		[DataMember]
        public virtual string Importance { get; set; }

        /// <summary>
        /// 提醒内容
        /// </summary>
		[DataMember]
        public virtual string Content { get; set; }

        /// <summary>
        /// 提醒人
        /// </summary>
		[DataMember]
        public virtual string Reminder { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
		[DataMember]
        public virtual DateTime SendTime { get; set; }

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


        #endregion

    }
}