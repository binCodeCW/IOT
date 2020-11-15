using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace YH.Security.Entity
{
    /// <summary>
    /// 角色的数据权限
    /// </summary>
    [DataContract]
    public class RoleDataInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public RoleDataInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Role_ID = 0;

        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [DataMember]
        public virtual int Role_ID { get; set; }

        /// <summary>
        /// 所属机构
        /// </summary>
        [DataMember]
        public virtual string BelongCompanys { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        [DataMember]
        public virtual string BelongDepts { get; set; }

        /// <summary>
        /// 排除部门
        /// </summary>
        [DataMember]
        public virtual string ExcludeDepts { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }


        #endregion

    }
}