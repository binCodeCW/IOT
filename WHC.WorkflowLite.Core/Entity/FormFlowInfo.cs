using System;
using System.Xml.Serialization;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using System.Runtime.Serialization;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 模板流程
    /// </summary>
    [DataContract]
    public class FormFlowInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public FormFlowInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.ProcType = 0;//流程环节名称：(0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,(自定义流程)
            this.MaySelproc = 0;//是否可以选择该流程的具体处理人 
            this.MayAddFlow = 0; //流程处理人是否可以增加新的流程      
            this.MayMsgSend = 0;//该流程的处理人是否可以发送通知   
            this.CanBack = 0;//在该流程环节处理人是否可以回退(0:否，1/非0:是)  
            this.CanBeBack = 0;//是否可以回退到该流程环节(0:否，1/非0:是)      
            this.CanSms = 0;//是否支持短信审批(0:不支持, 1/非0:支持)  
            this.Orderbyid = DateTime.Now.DateTimeToInt();//模板流程顺序（0,1,2,3...）    
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 流程模板ID
        /// </summary>
        [DataMember]
        public virtual string FormId { get; set; }

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
        /// 流程处理人
        /// </summary>
        [DataMember]
        public virtual string CondUser { get; set; }

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
        public virtual int MayAddFlow { get; set; }

        /// <summary>
        /// 该流程的处理人是否可以发送通知
        /// </summary>
        [DataMember]
        public virtual int MayMsgSend { get; set; }

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
        /// 备注信息
        /// </summary>
        [DataMember]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 模板流程顺序（0,1,2,3...）
        /// </summary>
        [DataMember]
        public virtual decimal Orderbyid { get; set; }


        #endregion
    }
}