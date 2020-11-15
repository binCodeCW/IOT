using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// YH_DeviceAlarmInfo
    /// </summary>
    [DataContract]
    [Serializable]
    public class YH_DeviceAlarmInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public YH_DeviceAlarmInfo()
        {
            this.ID = 0;
            this.Deviceno = "NULL";
        }

        #region Property Members

        /// <summary>
        /// ID号
        /// </summary>
        [DataMember]
        public virtual int ID { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
		[DataMember]
        public virtual string Timestamp { get; set; }

        /// <summary>
        /// 时刻
        /// </summary>
		[DataMember]
        public virtual DateTime Stamptime { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
		[DataMember]
        public virtual string Appid { get; set; }

        /// <summary>
        /// 服务ID
        /// </summary>
		[DataMember]
        public virtual string Serviceid { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
		[DataMember]
        public virtual string Deviceid { get; set; }

        /// <summary>
        /// 报警信息
        /// </summary>
		[DataMember]
        public virtual string ErrorNum { get; set; }

        /// <summary>
        /// 仪器型号
        /// </summary>
		[DataMember]
        public virtual string DeviceStyle { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
		[DataMember]
        public virtual DateTime Recordtime { get; set; }

        /// <summary>
        /// 仪器编号
        /// </summary>
		[DataMember]
        public virtual string Deviceno { get; set; }


        #endregion

    }
}