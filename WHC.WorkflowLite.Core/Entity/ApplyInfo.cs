using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 流程申请单
    /// </summary>
    [DataContract]
    [KnownType(typeof(ApplyStatus))]
    public class ApplyInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ApplyInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Status = ApplyStatus.处理中;  //当前状态(0:处理中,1:已完成,2:已退回,3:已撤消)(其它值为非法值)     
            this.ProcType = 0;//当前处理类型(0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,自定义流程)       
            this.MustSelect = false; //是否必须申请人选择处理人(0:不需要,1/非0:需要)         
            this.InformFinish = false; //是否通知处理人（完成）  
            this.InformRefuse = false; //是否通知处理人（退回）        
            this.InformCancel = false; //是否通知处理人（撤销）    
            this.SendMail = false; //通知方式（邮件） 
            this.SendBroad = false; //通知方式（内部广播） 
            this.SendMobile = false; //通知方式（短信）       
            this.CanBack = false; //在该流程环节处理人是否可以回退(0:否，1/非0:是) 
            this.MayCancel = false; //可否撤销(在流程未结束前，用户是否可以取消该申请)     
            this.Deleted = 0;
            this.Edittime = System.DateTime.Now; //创建日期   
        }

        /// <summary>
        /// 以FormInfo 信息复制部分数据
        /// </summary>
        /// <param name="formInfo"></param>
        public ApplyInfo(FormInfo formInfo, LoginUserInfo userInfo) : this()
        {
            this.FormId = formInfo.ID;
            this.Category = formInfo.Category;
            this.WhoInform = formInfo.WhoInform;
            this.MayCancel = formInfo.MayCancel;
            this.InformCancel = formInfo.InformCancel;
            this.InformFinish = formInfo.InformFinish;
            this.InformRefuse = formInfo.InformRefuse;
            this.SendBroad = formInfo.SendBroad;
            this.SendMobile = formInfo.SendMobile;
            this.SendMail = formInfo.SendMail;
            this.DataTable = formInfo.DataTable;

            //设置用户信息
            this.Editor = userInfo.ID;
            this.Dept_ID = userInfo.DeptId;
            this.Company_ID = userInfo.CompanyId;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 表单类型ID
        /// </summary>
        [DataMember]
        public virtual string FormId { get; set; }

        /// <summary>
        /// 表单分类
        /// </summary>
        [DataMember]
        public virtual string Category { get; set; }

        /// <summary>
        /// 申请单标题
        /// </summary>
        [DataMember]
        public virtual string Title { get; set; }

        /// <summary>
        /// 当前状态(0:处理中,1:已完成,2:已退回,3:已撤消)(其它值为非法值)
        /// </summary>
        [DataMember]
        public virtual ApplyStatus Status { get; set; }

        /// <summary>
        /// 当前处理类型(0:无处理,1:审批,2:归挡,3:会签,4:阅办,5:通知,自定义流程)
        /// </summary>
        [DataMember]
        public virtual int ProcType { get; set; }

        /// <summary>
        /// 当前处理人
        /// </summary>
        [DataMember]
        public virtual string ProcUser { get; set; }

        /// <summary>
        /// 最近一次的流程环节处理时间
        /// </summary>
        [DataMember]
        public virtual string ProcTime { get; set; }

        /// <summary>
        /// 是否必须申请人选择处理人(0:不需要,1/非0:需要)
        /// </summary>
        [DataMember]
        public virtual bool MustSelect { get; set; }       
        
        /// <summary>
        /// 对应的数据表
        /// </summary>
        [DataMember]
        public virtual string DataTable { get; set; }


        /// <summary>
        /// 要通知谁(该申请单完成后要自动通知哪些人)
        /// </summary>
        [DataMember]
        public virtual string WhoInform { get; set; }

        /// <summary>
        /// 撤消理由
        /// </summary>
        [DataMember]
        public virtual string WhyCancel { get; set; }

        /// <summary>
        /// 是否通知处理人（完成）
        /// </summary>
        [DataMember]
        public virtual bool InformFinish { get; set; }

        /// <summary>
        /// 是否通知处理人（退回）
        /// </summary>
        [DataMember]
        public virtual bool InformRefuse { get; set; }

        /// <summary>
        /// 是否通知处理人（撤销）
        /// </summary>
        [DataMember]
        public virtual bool InformCancel { get; set; }

        /// <summary>
        /// 通知方式（邮件）
        /// </summary>
        [DataMember]
        public virtual bool SendMail { get; set; }

        /// <summary>
        /// 通知方式（内部广播）
        /// </summary>
        [DataMember]
        public virtual bool SendBroad { get; set; }

        /// <summary>
        /// 通知方式（短信）
        /// </summary>
        [DataMember]
        public virtual bool SendMobile { get; set; }

        /// <summary>
        /// 在该流程环节处理人是否可以回退(0:否，1/非0:是)
        /// </summary>
        [DataMember]
        public virtual bool CanBack { get; set; }

        /// <summary>
        /// 可否撤销(在流程未结束前，用户是否可以取消该申请)
        /// </summary>
        [DataMember]
        public virtual bool MayCancel { get; set; }

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
        /// 部门ID
        /// </summary>
        [DataMember]
        public virtual string Dept_ID { get; set; }

        /// <summary>
        /// 所属公司ID
        /// </summary>
        [DataMember]
        public virtual string Company_ID { get; set; }

        /// <summary>
        /// 是否标记删除
        /// </summary>
        [DataMember]
        public virtual int Deleted { get; set; }


        #endregion

    }
}