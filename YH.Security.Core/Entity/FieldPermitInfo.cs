using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace YH.Security.Entity
{
    /// <summary>
    /// 字段的列表权限
    /// </summary>
    [DataContract]
    public class FieldPermitInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public FieldPermitInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Role_ID = 0;
            this.Seq = 0;
            this.Permit = 0;

        }

        #region Property Members

        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [DataMember]
        public virtual int Role_ID { get; set; }

        /// <summary>
        /// 字段域对象ID
        /// </summary>
        [DataMember]
        public virtual string FieldDomain_ID { get; set; }

        /// <summary>
        /// 实体类全名
        /// </summary>
        [DataMember]
        public virtual string EntityFullName { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [DataMember]
        public virtual string FiledName { get; set; }

        /// <summary>
        /// 字段代码
        /// </summary>
        [DataMember]
        public virtual string FiledCode { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public virtual decimal Seq { get; set; }

        /// <summary>
        /// 字段权限
        /// </summary>
        [DataMember]
        public virtual int Permit { get; set; }


        #endregion

    }
}