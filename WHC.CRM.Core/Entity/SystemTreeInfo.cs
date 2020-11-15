using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;
using System.Collections.Generic;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 系统列表集合
    /// </summary>
    [DataContract]
    public class SystemTreeInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public SystemTreeInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Visible = true;
            this.PID = "-1";

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
        /// 树节点名称
        /// </summary>
		[DataMember]
        public virtual string TreeName { get; set; }

        /// <summary>
        /// 特殊标志
        /// </summary>
		[DataMember]
        public virtual string SpecialTag { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
		[DataMember]
        public virtual bool Visible { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
		[DataMember]
        public virtual string PID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
		[DataMember]
        public virtual string Seq { get; set; }


        #endregion

    }


    [Serializable]
    [DataContract]
    public class SystemTreeNodeInfo : SystemTreeInfo
    {
        private List<SystemTreeNodeInfo> m_Children = new List<SystemTreeNodeInfo>();

        /// <summary>
        /// 子菜单实体类对象集合
        /// </summary>
        [DataMember]
        public List<SystemTreeNodeInfo> Children
        {
            get { return m_Children; }
            set { m_Children = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SystemTreeNodeInfo()
        {
            this.m_Children = new List<SystemTreeNodeInfo>();
        }

        /// <summary>
        /// 参数构造函数
        /// </summary>
        /// <param name="info">SystemTreeInfo对象</param>
        public SystemTreeNodeInfo(SystemTreeInfo info)
        {
            base.ID = info.ID;
            base.Category = info.Category;
            base.TreeName = info.TreeName;
            base.PID = info.PID;
            base.Seq = info.Seq;
            base.Visible = info.Visible;
            base.SpecialTag = info.SpecialTag;
        }
    }


    /// <summary>
    /// 用来承载TreeNode的信息
    /// </summary>

    [Serializable]
    [DataContract]
    public class TreeNodeInfo
    {
        /// <summary>
        /// 子对象集合
        /// </summary>
        [DataMember]
        public List<TreeNodeInfo> Nodes { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        [DataMember]
        public string Text { get; set; }

        /// <summary>
        /// 节点标签
        /// </summary>
        [DataMember]
        public string Tag { get; set; }

        /// <summary>
        /// 图标序号
        /// </summary>
        [DataMember]
        public int IconIndex { get; set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        [DataMember]
        public bool IsExpanded { get; set; }

        /// <summary>
        /// 前景色
        /// </summary>
        [DataMember]
        public string ForeColor { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TreeNodeInfo() {
            this.Nodes = new List<TreeNodeInfo>();
        }

        /// <summary>
        /// 参数构造函数
        /// </summary>
        /// <param name="text">节点名称</param>
        /// <param name="iconIndex">图标序号</param>
        /// <param name="tag">节点标签</param>
        public TreeNodeInfo(string text, int iconIndex, string tag = "") : this()
        {
            this.Text = text;
            this.IconIndex = iconIndex;
            this.Tag = tag;
        }
    }

}