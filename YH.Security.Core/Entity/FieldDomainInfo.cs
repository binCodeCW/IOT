using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace YH.Security.Entity
{
    /// <summary>
    /// 字段权限域对象
    /// </summary>
    [DataContract]
    public class FieldDomainInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public FieldDomainInfo()
		{
            this.ID= System.Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
		}

        #region Property Members
        
        /// <summary>
        /// ID
        /// </summary>
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 实体类全名
        /// </summary>
		[DataMember]
        public virtual string EntityFullName { get; set; }

        /// <summary>
        /// 类路径
        /// </summary>
		[DataMember]
        public virtual string ClassPath { get; set; }

        /// <summary>
        /// 字段别名
        /// </summary>
		[DataMember]
        public virtual string ColumnAlias { get; set; }

        /// <summary>
        /// 字段列表
        /// </summary>
		[DataMember]
        public virtual string FieldList { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
		[DataMember]
        public virtual DateTime CreateTime { get; set; }


        #endregion

    }
}