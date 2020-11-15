using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// YH_AlarmMapInfo
    /// </summary>
    [DataContract]
    [Serializable]
    public class YH_AlarmMapInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public YH_AlarmMapInfo()
        {
            this.ID = 0;

        }

        #region Property Members

        /// <summary>
        /// ID号
        /// </summary>
        [DataMember]
        public virtual int ID { get; set; }

        /// <summary>
        /// 仪器类型编号
        /// </summary>
		[DataMember]
        public virtual string DeviceTypeNo { get; set; }

        /// <summary>
        /// 仪器类型名称
        /// </summary>
		[DataMember]
        public virtual string DeviceTypeName { get; set; }

        /// <summary>
        /// 故障号
        /// </summary>
		[DataMember]
        public virtual string ErrorNO { get; set; }

        /// <summary>
        /// 故障说明
        /// </summary>
		[DataMember]
        public virtual string ErrorText { get; set; }


        #endregion

    }
}