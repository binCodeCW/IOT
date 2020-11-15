using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 客户提醒设置
    /// </summary>
    [DataContract]
    public class CustomerAlarmInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public CustomerAlarmInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Days = 0;

        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        [DataMember]
        public virtual string User_ID { get; set; }

        /// <summary>
        /// 客户级别
        /// </summary>
        [DataMember]
        public virtual string Grade { get; set; }

        /// <summary>
        /// 提醒天数
        /// </summary>
        [DataMember]
        public virtual int Days { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }


        #endregion

    }
}