using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 产品售后记录
    /// </summary>
    [DataContract]
    public class AfterSellInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public AfterSellInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.AttachGUID = System.Guid.NewGuid().ToString();
            this.Satisfaction = 0;
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
        /// 服务类型
        /// </summary>
        [DataMember]
        public virtual string Category { get; set; }

        /// <summary>
        /// 服务概要
        /// </summary>
        [DataMember]
        public virtual string Title { get; set; }

        /// <summary>
        /// 服务内容
        /// </summary>
        [DataMember]
        public virtual string Content { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 附件组别ID
        /// </summary>
        [DataMember]
        public virtual string AttachGUID { get; set; }

        /// <summary>
        /// 经办人员
        /// </summary>
        [DataMember]
        public virtual string Operator { get; set; }

        /// <summary>
        /// 服务时间
        /// </summary>
        [DataMember]
        public virtual DateTime OperateDate { get; set; }

        /// <summary>
        /// 服务状态
        /// </summary>
        [DataMember]
        public virtual string Status { get; set; }

        /// <summary>
        /// 客户满意度
        /// </summary>
        [DataMember]
        public virtual decimal Satisfaction { get; set; }

        /// <summary>
        /// 客户反馈意见
        /// </summary>
        [DataMember]
        public virtual string Suggestion { get; set; }

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