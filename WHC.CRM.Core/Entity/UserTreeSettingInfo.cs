using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 用户配置的系统列表集合
    /// </summary>
    [DataContract]
    public class UserTreeSettingInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public UserTreeSettingInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [DataMember]
        public virtual string Category { get; set; }

        /// <summary>
        /// 树节点ID
        /// </summary>
        [DataMember]
        public virtual string SystemTree_ID { get; set; }

        /// <summary>
        /// 所属用户ID
        /// </summary>
        [DataMember]
        public virtual string Owner_ID { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        [DataMember]
        public virtual string Dept_ID { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        [DataMember]
        public virtual string Company_ID { get; set; }


        #endregion

    }
}