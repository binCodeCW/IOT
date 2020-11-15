using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace YH.Security.Entity
{
    /// <summary>
    /// 角色信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class RoleInfo : BaseEntity
    {
        /// <summary>
        /// 超级管理员名称
        /// </summary>
        public const string SuperAdminName = "超级管理员";

        /// <summary>
        /// 公司级别的系统管理员
        /// </summary>
        public const string CompanyAdminName = "系统管理员";

      
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public RoleInfo()
        {
            this.ID = 0;
            this.PID = -1;
            this.CreateTime = System.DateTime.Now;
            this.EditTime = System.DateTime.Now;
            this.Deleted = false; //是否已删除
            this.Enabled = true; //有效标志

        }

        #region Property Members

        [DataMember]
        public virtual int ID { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [DataMember]
        public virtual int PID { get; set; }

        /// <summary>
        /// 角色编码
        /// </summary>
        [DataMember]
        public virtual string HandNo { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [DataMember]
        public virtual string SortCode { get; set; }

        /// <summary>
        /// 角色分类
        /// </summary>
        [DataMember]
        public virtual string Category { get; set; }

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

        /// <summary>
        /// 是否已删除
        /// </summary>
        [DataMember]
        public virtual bool Deleted { get; set; }

        /// <summary>
        /// 有效标志
        /// </summary>
        [DataMember]
        public virtual bool Enabled { get; set; }


        #endregion
    }
}