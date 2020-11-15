using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using YH.Framework.ControlUtil;

namespace YH.Security.Entity
{
    /// <summary>
    /// 功能菜单
    /// </summary>
    [Serializable]
    [DataContract]
    public class MenuInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public MenuInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.PID = "-1";
            this.Visible = true;
            this.Expand = false;
            this.CreateTime = System.DateTime.Now;
            this.EditTime = System.DateTime.Now;
            this.Deleted = false;
            this.EmbedIcon = new byte[] { };
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [DataMember]
        public virtual string PID { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 图标（遗留不用）
        /// </summary>
        [DataMember]
        public virtual string Icon { get; set; }

        /// <summary>
        /// 图标字节
        /// </summary>
        [DataMember]
        public virtual byte[] EmbedIcon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public virtual string Seq { get; set; }

        /// <summary>
        /// 功能ID
        /// </summary>
        [DataMember]
        public virtual string FunctionId { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        [DataMember]
        public virtual bool Visible { get; set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        [DataMember]
        public virtual bool Expand { get; set; }

        /// <summary>
        /// Winform窗体类型
        /// </summary>
        [DataMember]
        public virtual string WinformType { get; set; }

        /// <summary>
        /// Web界面Url地址
        /// </summary>
        [DataMember]
        public virtual string Url { get; set; }

        /// <summary>
        /// Web界面的菜单图标
        /// </summary>
        [DataMember]
        public virtual string WebIcon { get; set; }

        /// <summary>
        /// 系统编号
        /// </summary>
        [DataMember]
        public virtual string SystemType_ID { get; set; }

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
        /// 特殊标签
        /// </summary>
        [DataMember]
        public virtual string Tag { get; set; }

        #endregion

    }

    /// <summary>
    /// 功能菜单节点对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class MenuNodeInfo : MenuInfo
    {
        private List<MenuNodeInfo> m_Children = new List<MenuNodeInfo>();

        /// <summary>
        /// 子菜单实体类对象集合
        /// </summary>
        [DataMember]
        public List<MenuNodeInfo> Children
        {
            get { return m_Children; }
            set { m_Children = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MenuNodeInfo()
        {
            this.m_Children = new List<MenuNodeInfo>();
        }

        /// <summary>
        /// 参数构造函数
        /// </summary>
        /// <param name="menuInfo">MenuInfo对象</param>
        public MenuNodeInfo(MenuInfo menuInfo)
        {
            base.ID = menuInfo.ID;
            base.Name = menuInfo.Name;
            base.PID = menuInfo.PID;
            base.Seq = menuInfo.Seq;
            base.Visible = menuInfo.Visible;
            base.Expand = menuInfo.Expand;
            base.FunctionId = menuInfo.FunctionId;
            base.Icon = menuInfo.Icon;
            base.EmbedIcon = menuInfo.EmbedIcon;
            base.WebIcon = menuInfo.WebIcon;
            base.WinformType = menuInfo.WinformType;
            base.Url = menuInfo.Url;
            base.SystemType_ID = menuInfo.SystemType_ID;
            base.Creator = menuInfo.Creator;
            base.Creator_ID = menuInfo.Creator_ID;
            base.CreateTime = menuInfo.CreateTime;
            base.Editor = menuInfo.Editor;
            base.Editor_ID = menuInfo.Editor_ID;
            base.EditTime = menuInfo.EditTime;
            base.Deleted = menuInfo.Deleted;
            base.Tag = menuInfo.Tag;
        }
    }
}