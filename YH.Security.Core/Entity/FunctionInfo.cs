using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using YH.Framework.ControlUtil;

namespace YH.Security.Entity
{
    /// <summary>
    /// 系统功能定义
    /// </summary>
    [Serializable]
    [DataContract]
    public class FunctionInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public FunctionInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.PID = "-1";

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
        /// 功能名称
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 控制标识
        /// </summary>
        [DataMember]
        public virtual string ControlID { get; set; }

        /// <summary>
        /// 系统编号
        /// </summary>
        [DataMember]
        public virtual string SystemType_ID { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [DataMember]
        public virtual string SortCode { get; set; }


        #endregion

    }

    /// <summary>
    /// 系统功能节点对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class FunctionNodeInfo : FunctionInfo
    {
        private List<FunctionNodeInfo> m_Children = new List<FunctionNodeInfo>();

        /// <summary>
        /// 子菜单实体类对象集合
        /// </summary>
        [DataMember]
        public List<FunctionNodeInfo> Children
        {
            get { return m_Children; }
            set { m_Children = value; }
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FunctionNodeInfo()
        {
            this.m_Children = new List<FunctionNodeInfo>();
        }

        /// <summary>
        /// 参数构造函数
        /// </summary>
        /// <param name="functionInfo">FunctionInfo对象</param>
        public FunctionNodeInfo(FunctionInfo functionInfo)
        {
            base.ControlID = functionInfo.ControlID;
            base.ID = functionInfo.ID;
            base.Name = functionInfo.Name;
            base.PID = functionInfo.PID;
            base.SystemType_ID = functionInfo.SystemType_ID;
            base.SortCode = functionInfo.SortCode;
        }
    }
}