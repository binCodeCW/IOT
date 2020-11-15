using System;
using System.Xml.Serialization;
using YH.Framework.ControlUtil;
using System.Runtime.Serialization;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 申请单日志
    /// </summary>
    [DataContract]
    public class ApplyLogInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ApplyLogInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.UserId = 0;//用户ID     
            this.Addtime = System.DateTime.Now; //增加时间 
        }

        /// <summary>
        /// 参数化构造
        /// </summary>
        /// <param name="applyId">流程申请单ID</param>
        /// <param name="userid"> 用户ID</param>
        /// <param name="content">日志内容</param>
        public ApplyLogInfo(string applyId, int userid, string content) : this()
        {
            this.UserId = userid;
            this.ApplyId = applyId;
            this.Content = content;
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
        /// 日志内容
        /// </summary>
        [DataMember]
        public virtual string Content { get; set; }


        #endregion

    }
}