using System;
using System.Xml.Serialization;
using YH.Framework.ControlUtil;
using System.Runtime.Serialization;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 常用意见
    /// </summary>
    [DataContract]
    public class CommonOpinionInfo : BaseEntity
    {    

        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public CommonOpinionInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Seq = 0;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 应用场景（一般为表单ID），空适应所有表单
        /// </summary>
        [DataMember]
        public virtual string FormScene { get; set; }

        /// <summary>
        /// 所属用户名，空适应所有用户
        /// </summary>
        [DataMember]
        public virtual string BelongUser { get; set; }

        /// <summary>
        /// 意见内容
        /// </summary>
        [DataMember]
        public virtual string Opinion { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        public virtual int Seq { get; set; }


        #endregion

    }
}