using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace YH.Security.Entity
{
    /// <summary>
    /// 系统标识信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class SystemTypeInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public SystemTypeInfo()
        {
            this.OID = System.Guid.NewGuid().ToString(); //系统标识
        }

        #region Property Members

        /// <summary>
        /// 系统标识
        /// </summary>
        [DataMember]
        public virtual string OID { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        [DataMember]
        public virtual string CustomID { get; set; }

        /// <summary>
        /// 授权编码
        /// </summary>
        [DataMember]
        public virtual string Authorize { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }


        #endregion

    }
}