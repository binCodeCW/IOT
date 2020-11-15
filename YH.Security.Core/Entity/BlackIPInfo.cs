using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace YH.Security.Entity
{
    /// <summary>
    /// 登录系统的黑白名单列表
    /// </summary>
    [DataContract]
    public class BlackIPInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public BlackIPInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.AuthorizeType = 0;
            this.Forbid = false;//是否禁用   
            this.CreateTime = System.DateTime.Now;
            this.EditTime = System.DateTime.Now;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        [DataMember]
        public virtual int AuthorizeType { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [DataMember]
        public virtual bool Forbid { get; set; }

        /// <summary>
        /// IP起始地址
        /// </summary>
        [DataMember]
        public virtual string IPStart { get; set; }

        /// <summary>
        /// IP结束地址
        /// </summary>
        [DataMember]
        public virtual string IPEnd { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public virtual string Creator { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [DataMember]
        public virtual string Creator_ID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 编辑人
        /// </summary>
        [DataMember]
        public virtual string Editor { get; set; }

        /// <summary>
        /// 编辑人ID
        /// </summary>
        [DataMember]
        public virtual string Editor_ID { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        [DataMember]
        public virtual DateTime EditTime { get; set; }


        #endregion

    }
}