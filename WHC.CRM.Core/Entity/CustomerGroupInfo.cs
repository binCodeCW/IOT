using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;
using System.Collections.Generic;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 客户组别
    /// </summary>
    [Serializable]
    [DataContract]
    public class CustomerGroupInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public CustomerGroupInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
            this.PID = "-1";
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [DataMember]
        public virtual string PID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public virtual string HandNo { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }

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
        /// 编辑时间
        /// </summary>
        [DataMember]
        public virtual DateTime EditTime { get; set; }

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

    [Serializable]
    [DataContract]
    public class CustomerGroupNodeInfo : CustomerGroupInfo
    {
        private List<CustomerGroupNodeInfo> m_Children = new List<CustomerGroupNodeInfo>();

        /// <summary>
        /// 子分组实体类对象集合
        /// </summary>
        [DataMember]
        public List<CustomerGroupNodeInfo> Children
        {
            get { return m_Children; }
            set { m_Children = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CustomerGroupNodeInfo()
        {
            this.m_Children = new List<CustomerGroupNodeInfo>();
        }

        /// <summary>
        /// 参数构造函数
        /// </summary>
        /// <param name="info">CustomerGroupInfo对象</param>
        public CustomerGroupNodeInfo(CustomerGroupInfo info)
        {
            base.ID = info.ID;
            base.HandNo = info.HandNo;
            base.Name = info.Name;
            base.PID = info.PID;
            base.Note = info.Note;
            base.Editor = info.Editor;
            base.EditTime = info.EditTime;
            base.Creator = info.Creator;
            base.CreateTime = info.CreateTime;
            base.Dept_ID = info.Dept_ID;
            base.Company_ID = info.Company_ID;
        }
    }
}