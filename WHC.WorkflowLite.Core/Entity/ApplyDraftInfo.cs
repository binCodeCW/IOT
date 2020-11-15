using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 申请单草稿（通用存储）
    /// </summary>
    [DataContract]
    public class ApplyDraftInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ApplyDraftInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 表单ID
        /// </summary>
        [DataMember]
        public virtual string Form_ID { get; set; }

        /// <summary>
        /// 表单分类
        /// </summary>
        [DataMember]
        public virtual string Category { get; set; }

        /// <summary>
        /// 表单名称
        /// </summary>
        [DataMember]
        public virtual string FormName { get; set; }

        /// <summary>
        /// 草稿标题
        /// </summary>
        [DataMember]
        public virtual string Title { get; set; }
        /// <summary>
        /// 申请单草稿(JSON格式)
        /// </summary>
        [DataMember]
        public virtual string ApplyDraftJson { get; set; }

        /// <summary>
        /// 业务表单草稿(JSON格式)
        /// </summary>
        [DataMember]
        public virtual string BizDraftJson { get; set; }

        /// <summary>
        /// 业务明细表单草稿(JSON格式)
        /// </summary>
        [DataMember]
        public virtual string BizDraftJson2 { get; set; }

        /// <summary>
        /// 业务明细表单草稿(JSON格式)
        /// </summary>
        [DataMember]
        public virtual string BizDraftJson3 { get; set; }

        /// <summary>
        /// 业务明细表单草稿(JSON格式)
        /// </summary>
        [DataMember]
        public virtual string BizDraftJson4 { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public virtual string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public virtual DateTime CreateTime { get; set; }


        #endregion

    }
}