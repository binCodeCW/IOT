using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace YH.Security.Entity
{
    /// <summary>
    /// 用户关键操作记录
    /// </summary>
    [DataContract]
    public class OperationLogInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public OperationLogInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.CreateTime = System.DateTime.Now;

        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 登录用户ID
        /// </summary>
        [DataMember]
        public virtual string User_ID { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [DataMember]
        public virtual string LoginName { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        [DataMember]
        public virtual string FullName { get; set; }

        /// <summary>
        /// 所属机构ID
        /// </summary>
        [DataMember]
        public virtual string Company_ID { get; set; }

        /// <summary>
        /// 所属机构名称
        /// </summary>
        [DataMember]
        public virtual string CompanyName { get; set; }

        /// <summary>
        /// 操作表名称
        /// </summary>
        [DataMember]
        public virtual string TableName { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [DataMember]
        public virtual string OperationType { get; set; }

        /// <summary>
        /// 日志描述
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [DataMember]
        public virtual string IPAddress { get; set; }

        /// <summary>
        /// Mac地址
        /// </summary>
        [DataMember]
        public virtual string MacAddress { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public virtual DateTime CreateTime { get; set; }


        #endregion

    }
}