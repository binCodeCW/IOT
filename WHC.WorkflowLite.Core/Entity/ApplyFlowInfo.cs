using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 表单具体流程
    /// </summary>
    [DataContract]
    public class ApplyFlowInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ApplyFlowInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.OrderNo = 0;//顺序(从1开始:1,2,3...)    
            this.ProcType = 0;//流程环节名称：(0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,(自定义流程)     
            this.MaySelproc = 0;//是否可以选择该流程的具体处理人  
            this.MayAddflow = 0;//流程处理人是否可以增加新的流程    
            this.MayMsgsend = 0;//该流程的处理人是否可以发送通知   
            this.CanBack = 0;//在该流程环节处理人是否可以回退(0:否，1/非0:是)  
            this.CanBeBack = 0;//是否可以回退到该流程环节(0:否，1/非0:是) 
            this.CanSms = 0; //是否支持短信审批(0:不支持, 1/非0:支持)   
            this.IsAdd = 0;//是否新增环节(0:否，1/非0:是) 
            this.IsBack = 0;//是否被回退过(0:否，1/非0:回退次数) 
            this.IsProc = 0;//是否处理 
            this.ProcUid = 0;//实际处理人ID    
            this.Deltatime = 0;//处理间隔时间(即和上一次审批的间隔秒数)(初始值为0)       
            this.SmsProc = 0;//是否短信处理(0:未处理, 1/非0:已处理)(只有处理成功才记录)
        }

        /// <summary>
        /// 以FormFlowInfo的值参考复制部分
        /// </summary>
        /// <param name="flowInfo">FormFlowInfo对象</param>
        public ApplyFlowInfo(FormFlowInfo flowInfo) : this()
        {
            this.ProcType = flowInfo.ProcType;
            this.FlowName = flowInfo.FlowName;
            this.MaySelproc = flowInfo.MaySelproc;
            this.MayAddflow = flowInfo.MayAddFlow;
            this.MayMsgsend = flowInfo.MayMsgSend;
            this.CanBack = flowInfo.CanBack;
            this.CanBeBack = flowInfo.CanBeBack;
            this.CanSms = flowInfo.CanSms;
            this.ProcUser = flowInfo.ProcUser;//可用标记{$申请人},{$所在部门},{$部门领导},{$部门员工}
            this.CondVerify = flowInfo.CondUser;
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
        /// 对应的条件(为空表示不限制)(只有符合该条件才执行当前步骤)
        /// </summary>
        [DataMember]
        public virtual string CondVerify { get; set; }

        /// <summary>
        /// 该流程对应的处理人
        /// </summary>
        [DataMember]
        public virtual string ProcUser { get; set; }

        /// <summary>
        /// 是否可以选择该流程的具体处理人
        /// </summary>
        [DataMember]
        public virtual int MaySelproc { get; set; }

        /// <summary>
        /// 流程处理人是否可以增加新的流程
        /// </summary>
        [DataMember]
        public virtual int MayAddflow { get; set; }

        /// <summary>
        /// 该流程的处理人是否可以发送通知
        /// </summary>
        [DataMember]
        public virtual int MayMsgsend { get; set; }

        /// <summary>
        /// 在该流程环节处理人是否可以回退(0:否，1/非0:是)
        /// </summary>
        [DataMember]
        public virtual int CanBack { get; set; }

        /// <summary>
        /// 是否可以回退到该流程环节(0:否，1/非0:是)
        /// </summary>
        [DataMember]
        public virtual int CanBeBack { get; set; }

        /// <summary>
        /// 是否支持短信审批(0:不支持, 1/非0:支持)
        /// </summary>
        [DataMember]
        public virtual int CanSms { get; set; }

        /// <summary>
        /// 是否新增环节(0:否，1/非0:是)
        /// </summary>
        [DataMember]
        public virtual int IsAdd { get; set; }

        /// <summary>
        /// 是否被回退过(0:否，1/非0:回退次数)
        /// </summary>
        [DataMember]
        public virtual int IsBack { get; set; }

        /// <summary>
        /// 是否处理
        /// </summary>
        [DataMember]
        public virtual int IsProc { get; set; }

        /// <summary>
        /// 实际处理人ID
        /// </summary>
        [DataMember]
        public virtual int ProcUid { get; set; }

        /// <summary>
        /// 实际处理时间
        /// </summary>
        [DataMember]
        public virtual string ProcTime { get; set; }

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
        /// 处理间隔时间(即和上一次审批的间隔秒数)(初始值为0)
        /// </summary>
        [DataMember]
        public virtual int Deltatime { get; set; }

        /// <summary>
        /// 是否短信处理(0:未处理, 1/非0:已处理)(只有处理成功才记录)
        /// </summary>
        [DataMember]
        public virtual int SmsProc { get; set; }


        #endregion

    }
}