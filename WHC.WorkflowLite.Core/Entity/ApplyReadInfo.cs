using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 申请单已读记录
    /// </summary>
    [DataContract]
    public class ApplyReadInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ApplyReadInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Addtime = DateTime.Now;
            this.UserId = 0;
        }

        /// <summary>
        /// 参数化构造
        /// </summary>
        /// <param name="applyId">流程申请单ID</param>
        /// <param name="userId">用户ID</param>
        public ApplyReadInfo(string applyId, int userId) : this()
        {
            this.ApplyId = applyId;
            this.UserId = userId;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 流程申请单ID
        /// </summary>
        [DataMember]
        public virtual string ApplyId { get; set; }

        /// <summary>
        /// 增加时间
        /// </summary>
        [DataMember]
        public virtual DateTime Addtime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public virtual int UserId { get; set; }

        /// <summary>
        /// 阅办意见
        /// </summary>
        [DataMember]
        public virtual string Content { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        [DataMember]
        public virtual DateTime ReadTime { get; set; }


        #endregion

    }
}