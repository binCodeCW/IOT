using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 表单流程会签记录
    /// </summary>
    [DataContract]
    public class ApplySignInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ApplySignInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.UserId = 0;//用户ID        
            this.IsProc = 0; //是否处理(0:未处理/未处理完,1:已处理,2/其它:已退回)  
            this.Deltatime = 0;//处理间隔时间
            this.ProcTime = System.DateTime.Now; //实际处理时间  
        }

        /// <summary>
        /// 参数化构造
        /// </summary>
        public ApplySignInfo(string applyId, int userId, string flowId, string opinion = null, int isProc = 0) : this()
        {
            this.ApplyId = applyId;
            this.UserId = userId;
            this.FlowId = flowId;
            this.Opinion = opinion;
            this.IsProc = isProc;
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
        /// 流程步骤ID
        /// </summary>
        [DataMember]
        public virtual string FlowId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public virtual int UserId { get; set; }

        /// <summary>
        /// 是否处理(0:未处理/未处理完,1:已处理,2/其它:已退回)
        /// </summary>
        [DataMember]
        public virtual int IsProc { get; set; }

        /// <summary>
        /// 实际处理时间
        /// </summary>
        [DataMember]
        public virtual DateTime ProcTime { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        [DataMember]
        public virtual string Opinion { get; set; }

        /// <summary>
        /// 发送了通知给哪些人
        /// </summary>
        [DataMember]
        public virtual string MsgSendTo { get; set; }

        /// <summary>
        /// 处理间隔时间
        /// </summary>
        [DataMember]
        public virtual int Deltatime { get; set; }


        #endregion

    }
}