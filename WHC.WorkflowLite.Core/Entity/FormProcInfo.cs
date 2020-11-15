using System;
using System.Xml.Serialization;
using YH.Framework.ControlUtil;
using System.Runtime.Serialization;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 流程环节（模板流程的步骤）
    /// </summary>
    [DataContract]
    public class FormProcInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public FormProcInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.ProcType = 0;//流程环节名称：(0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,(自定义流程)  
            this.Forbid = 0;//是否禁止  
            this.Deleted = 0;
            this.Edittime = System.DateTime.Now; //创建日期   
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 流程环节名称
        /// </summary>
        [DataMember]
        public virtual string ProcName { get; set; }

        /// <summary>
        /// 处理类型：(0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,(自定义流程)
        /// </summary>
        [DataMember]
        public virtual int ProcType { get; set; }

        /// <summary>
        /// 对应表单
        /// </summary>
        [DataMember]
        public virtual int FormId { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [DataMember]
        public virtual int Forbid { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [DataMember]
        public virtual string Editor { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember]
        public virtual DateTime Edittime { get; set; }

        /// <summary>
        /// 是否标记删除
        /// </summary>
        [DataMember]
        public virtual int Deleted { get; set; }


        #endregion


    }
}