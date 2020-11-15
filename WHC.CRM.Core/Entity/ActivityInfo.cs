using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 客户活动管理
    /// </summary>
    [DataContract]
    public class ActivityInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ActivityInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
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
        /// 编号
        /// </summary>
        [DataMember]
        public virtual string HandNo { get; set; }

        /// <summary>
        /// 活动类别
        /// </summary>
        [DataMember]
        public virtual string Category { get; set; }

        /// <summary>
        /// 活动标题
        /// </summary>
        [DataMember]
        public virtual string Title { get; set; }

        /// <summary>
        /// 活动内容
        /// </summary>
        [DataMember]
        public virtual string Content { get; set; }

        /// <summary>
        /// 附件组别ID
        /// </summary>
        [DataMember]
        public virtual string AttachGUID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public virtual DateTime EndTime { get; set; }

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
        /// 活动场所
        /// </summary>
        [DataMember]
        public virtual string Place { get; set; }

        /// <summary>
        /// 活动场所地址
        /// </summary>
        [DataMember]
        public virtual string PlaceAddress { get; set; }

        /// <summary>
        /// 活动场所电话
        /// </summary>
        [DataMember]
        public virtual string PlacePhone { get; set; }

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