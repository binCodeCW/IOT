using System;
using System.Runtime.Serialization;

using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// 收到协议
    /// </summary>
    [DataContract]
    [Serializable]
    public class ReceiveProtocalInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public ReceiveProtocalInfo()
		{
            this.ID= System.Guid.NewGuid().ToString();
            this.CreateTime= System.DateTime.Now;
		}

        #region Property Members
        
        /// <summary>
        /// ID
        /// </summary>
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 发送方用户ID
        /// </summary>
		[DataMember]
        public virtual string FromUserId { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
		[DataMember]
        public virtual string MsgType { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
		[DataMember]
        public virtual string Seq { get; set; }

        /// <summary>
        /// 承载的内容
        /// </summary>
		[DataMember]
        public virtual string Content { get; set; }

        /// <summary>
        /// 协议数据
        /// </summary>
		[DataMember]
        public virtual string Protocal { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
		[DataMember]
        public virtual DateTime CreateTime { get; set; }


        #endregion

    }
}