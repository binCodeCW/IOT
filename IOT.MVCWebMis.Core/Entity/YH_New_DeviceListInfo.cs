using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// YH_New_DeviceListInfo
    /// </summary>
    [DataContract]
    [Serializable]
    public class YH_New_DeviceListInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public YH_New_DeviceListInfo()
        {
            this.DeviceId = System.Guid.NewGuid().ToString();
        }

        #region Property Members

        /// <summary>
        /// 设备编码
        /// </summary>
        [DataMember]
        public virtual string DeviceId { get; set; }

        /// <summary>
        /// 设备名称，型号+ 设备编号
        /// </summary>
		[DataMember]
        public virtual string DeviceName { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
		[DataMember]
        public virtual string TenantId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
		[DataMember]
        public virtual string ProductId { get; set; }

        /// <summary>
        /// IMEI号
        /// </summary>
		[DataMember]
        public virtual string Imei { get; set; }

        /// <summary>
        /// IMSI号
        /// </summary>
		[DataMember]
        public virtual string Imsi { get; set; }

        [DataMember]
        public virtual string FirmwareVersion { get; set; }

        [DataMember]
        public virtual string DeviceStatus { get; set; }

        [DataMember]
        public virtual string AutoObserver { get; set; }

        [DataMember]
        public virtual string CreateTime { get; set; }

        [DataMember]
        public virtual string CreateTimeTrans { get; set; }

        [DataMember]
        public virtual string CreateBy { get; set; }

        [DataMember]
        public virtual string UpdateTime { get; set; }

        [DataMember]
        public virtual string UpdateTimeTrans { get; set; }

        [DataMember]
        public virtual string UpdateBy { get; set; }

        [DataMember]
        public virtual string NetStatus { get; set; }

        [DataMember]
        public virtual string OnlineAt { get; set; }

        [DataMember]
        public virtual string OnlineAtTrans { get; set; }

        [DataMember]
        public virtual string OfflineAt { get; set; }

        [DataMember]
        public virtual string OfflineAtTrans { get; set; }

        [DataMember]
        public virtual string Model { get; set; }

        [DataMember]
        public virtual string Serial { get; set; }

        /// <summary>
        /// 最新位置
        /// </summary>
		[DataMember]
        public virtual string Locationrecent { get; set; }

        /// <summary>
        /// 最后定位时间
        /// </summary>
		[DataMember]
        public virtual DateTime Locatetime { get; set; }


        #endregion

    }
}