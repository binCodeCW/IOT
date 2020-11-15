using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace YH.Security.Entity
{
    /// <summary>
    /// 记录操作日志的数据表配置
    /// </summary>
    [DataContract]
    public class OperationLogSettingInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public OperationLogSettingInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Forbid = false; //是否禁用  
            this.InsertLog = false;
            this.DeleteLog = false;
            this.UpdateLog = false;
            this.CreateTime = System.DateTime.Now;
            this.EditTime = System.DateTime.Now;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [DataMember]
        public virtual bool Forbid { get; set; }

        /// <summary>
        /// 数据库表
        /// </summary>
        [DataMember]
        public virtual string TableName { get; set; }

        /// <summary>
        /// 记录插入日志
        /// </summary>
        [DataMember]
        public virtual bool InsertLog { get; set; }

        /// <summary>
        /// 记录删除日志
        /// </summary>
        [DataMember]
        public virtual bool DeleteLog { get; set; }

        /// <summary>
        /// 记录更新日志
        /// </summary>
        [DataMember]
        public virtual bool UpdateLog { get; set; }

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