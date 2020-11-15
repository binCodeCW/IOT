using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 流程表单当前处理人
    /// </summary>
    [DataContract]
    public class ApplyUserInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ApplyUserInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Alerttime = System.DateTime.Now; //修改时间         
            this.UserId = 0; //用户ID   
        }

        /// <summary>
        /// 参数化构造
        /// </summary>
        /// <param name="applyId">流程申请单ID</param>
        /// <param name="userId">用户ID</param>
        public ApplyUserInfo(string applyId, int userId ) : this()
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
        /// 用户ID
        /// </summary>
        [DataMember]
        public virtual int UserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
        public virtual DateTime Alerttime { get; set; }


        #endregion

    }
}