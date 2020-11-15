using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 存放地点
    /// </summary>
    [DataContract]
    [Serializable]
    public class StoreAddressInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public StoreAddressInfo()
		{
            this.ID= System.Guid.NewGuid().ToString();
       
		}

        #region Property Members
        
        /// <summary>
        /// 编号
        /// </summary>
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
		[DataMember]
        public virtual string Dept_ID { get; set; }

        /// <summary>
        /// 存放地点
        /// </summary>
		[DataMember]
        public virtual string KeepAddr { get; set; }

        /// <summary>
        /// 备注信息
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


        #endregion

    }
}