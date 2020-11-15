using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using YH.Framework.ControlUtil;

namespace YH.Security.Entity
{
    /// <summary>
    /// 部门机构信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class OUInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public OUInfo()
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
        /// 机构编码
        /// </summary>
		[DataMember]
        public virtual string HandNo { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
		[DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
		[DataMember]
        public virtual string SortCode { get; set; }

        /// <summary>
        /// 机构分类
        /// </summary>
		[DataMember]
        public virtual string Category { get; set; }

        /// <summary>
        /// 机构地址
        /// </summary>
		[DataMember]
        public virtual string Address { get; set; }

        /// <summary>
        /// 外线电话
        /// </summary>
		[DataMember]
        public virtual string OuterPhone { get; set; }

        /// <summary>
        /// 内线电话
        /// </summary>
		[DataMember]
        public virtual string InnerPhone { get; set; }

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


        #endregion

    }

    /// <summary>
    /// 部门机构节点对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class OUNodeInfo : OUInfo
    {
        private List<OUNodeInfo> m_Children = new List<OUNodeInfo>();

        /// <summary>
        /// 子机构实体类对象集合
        /// </summary>
        [DataMember]
        public List<OUNodeInfo> Children
        {
            get { return m_Children; }
            set { m_Children = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public OUNodeInfo()
        {
            this.m_Children = new List<OUNodeInfo>();
        }

        /// <summary>
        /// 参数构造函数
        /// </summary>
        /// <param name="info">OUInfo对象</param>
        public OUNodeInfo(OUInfo info)
        {
            base.ID = info.ID;
            base.PID = info.PID;
            base.HandNo = info.HandNo;
            base.Name = info.Name;
            base.SortCode = info.SortCode;
            base.Category = info.Category;
            base.Address = info.Address;
            base.OuterPhone = info.OuterPhone;
            base.InnerPhone = info.InnerPhone;
            base.Note = info.Note;
            base.Creator = info.Creator;
            base.Creator_ID = info.Creator_ID;
            base.CreateTime = info.CreateTime;
            base.Editor = info.Editor;
            base.Editor_ID = info.Editor_ID;
            base.EditTime = info.EditTime;
            base.Deleted = info.Deleted;
            base.Enabled = info.Enabled;
            base.Company_ID = info.Company_ID;
            base.CompanyName = info.CompanyName;              
        }
    }
}