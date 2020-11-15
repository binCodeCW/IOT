using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 用户对指定内容的操作状态记录
    /// </summary>
    [DataContract]
    public class InformationStatusInfo : BaseEntity
    {
        public InformationStatusInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Status = 0;//阅读状态（0：未读，1：已读，其他待定） 
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 信息类型
        /// </summary>
		[DataMember]
        public virtual string Category { get; set; }

        /// <summary>
        /// 信息ID
        /// </summary>
		[DataMember]
        public virtual string Information_ID { get; set; }

        /// <summary>
        /// 阅读状态（0：未读，1：已读，其他待定）
        /// </summary>
		[DataMember]
        public virtual int Status { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
		[DataMember]
        public virtual string User_ID { get; set; }


        #endregion

    }

    /// <summary>
    /// 信息分类
    /// </summary>
    [DataContract]
    public enum InformationCategory 
    {
        [EnumMember]
        客户联系,

        [EnumMember]
        通知公告,

        [EnumMember]
        其他 
    };

}