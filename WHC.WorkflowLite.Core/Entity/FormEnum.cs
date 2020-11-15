using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 表单当前处理状态。
    /// 当前状态(0:处理中,1:已完成,2:已退回,3:已撤消)(其它值为非法值)
    /// </summary>
    [DataContract]
    public enum ApplyStatus
    {
        /// <summary>
        /// 处理中 = 0
        /// </summary>
        [EnumMember]
        处理中 = 0,

        /// <summary>
        /// 已完成 = 1
        /// </summary>
        [EnumMember]
        已完成 = 1,

        /// <summary>
        /// 已退回 = 2
        /// </summary>
        [EnumMember]
        已退回 = 2,

        /// <summary>
        /// 已撤消 =3
        /// </summary>
        [EnumMember]
        已撤消 = 3
    }


    [DataContract]
    public enum FlowTypeUserEnum
    {
        /// <summary>
        /// 业务受理
        /// </summary>
        [EnumMember]
        业务受理 = 1,
        /// <summary>
        /// 现场审查
        /// </summary>
        [EnumMember]
        现场审查 = 2,
        /// <summary>
        /// 初审
        /// </summary>
        [EnumMember]
        初审 = 3,
        /// <summary>
        /// 审批
        /// </summary>
        [EnumMember]
        审批 = 4,
        /// <summary>
        /// 办结
        /// </summary>
        [EnumMember]
        办结 = 5
    }

    /// <summary>
    /// 流程环节名称:0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,(自定义流程)
    /// </summary>
    [DataContract]
    public enum ProcType
    {
        /// <summary>
        /// 无处理 = 0
        /// </summary>
        无处理 = 0, 
        /// <summary>
        /// 审批 = 1
        /// </summary>
        审批 = 1, 
        /// <summary>
        /// 归挡 = 2
        /// </summary>
        归挡 = 2, 
        /// <summary>
        /// 会签 = 3
        /// </summary>
        会签 = 3, 
        /// <summary>
        /// 阅办 = 4
        /// </summary>
        阅办 = 4, 
        /// <summary>
        /// 通知= 5
        /// </summary>
        通知 = 5
    }

    /// <summary>
    /// 申请单的处理状态
    /// </summary>
    [DataContract]
    public enum ApplyIsProc
    {
        /// <summary>
        /// 未处理 = 0
        /// </summary>
        未处理 = 0,
        /// <summary>
        /// 通过 = 1
        /// </summary>
        通过 = 1,
        /// <summary>
        /// 拒绝 = 2
        /// </summary>
        拒绝 = 2, 
    }
}