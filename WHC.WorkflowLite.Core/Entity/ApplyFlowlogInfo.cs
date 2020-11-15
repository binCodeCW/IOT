using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 表单流程步骤日志
    /// </summary>
    [DataContract]
    public class ApplyFlowlogInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ApplyFlowlogInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.OrderNo = 0;//顺序(从1开始:1,2,3...)       
            this.ProcType = 0;//流程环节名称：(0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,(自定义流程)        
            this.ProcTime = System.DateTime.Now; //实际处理时间      
            this.Begtime = System.DateTime.Now; //开始时间   
        }

        /// <summary>
        /// 参数化构造
        /// </summary>
        /// <param name="applyId">流程申请单ID</param>
        /// <param name="flowName">步骤名称</param>
        /// <param name="procuser">该流程对应的处理人</param>
        /// <param name="opinion">处理意见,默认为空字符串</param>
        /// <param name="procType">流程环节名称，默认为0：(0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,(自定义流程)</param>
        public ApplyFlowlogInfo(string applyId, string flowName, int procuser, string opinion = "", int procType = 0) : this()
        {
            this.ApplyId = applyId;
            this.FlowName = flowName;
            this.ProcUser = procuser.ToString();
            this.Opinion = opinion;
            this.ProcType = procType;
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
        /// 流程ID
        /// </summary>
        [DataMember]
        public virtual string FlowId { get; set; }

        /// <summary>
        /// 顺序(从1开始:1,2,3...)
        /// </summary>
        [DataMember]
        public virtual int OrderNo { get; set; }

        /// <summary>
        /// 流程环节名称：(0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,(自定义流程)
        /// </summary>
        [DataMember]
        public virtual int ProcType { get; set; }

        /// <summary>
        /// 步骤名称
        /// </summary>
        [DataMember]
        public virtual string FlowName { get; set; }

        /// <summary>
        /// 该流程对应的处理人
        /// </summary>
        [DataMember]
        public virtual string ProcUser { get; set; }

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
        /// 下一流程名称
        /// </summary>
        [DataMember]
        public virtual string NextFlowName { get; set; }

        /// <summary>
        /// 下一处理人
        /// </summary>
        [DataMember]
        public virtual string NextProcUser { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public virtual DateTime Begtime { get; set; }


        #endregion


    }
}