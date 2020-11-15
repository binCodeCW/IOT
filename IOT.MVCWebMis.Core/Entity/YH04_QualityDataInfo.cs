using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// YH04_QualityDataInfo
    /// </summary>
    [DataContract]
    [Serializable]
    public class YH04_QualityDataInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public YH04_QualityDataInfo()
		{
            this.ID= 0;
		}

        #region Property Members
        
		[DataMember]
        public virtual long ID { get; set; }

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
        /// 左通道计数1
        /// </summary>
		[DataMember]
        public virtual string Gm1 { get; set; }

        /// <summary>
        /// 左通道计数2
        /// </summary>
		[DataMember]
        public virtual string Gm2 { get; set; }

        /// <summary>
        /// 右通道计数1
        /// </summary>
		[DataMember]
        public virtual string Gm3 { get; set; }

        /// <summary>
        /// 右通道计数2
        /// </summary>
		[DataMember]
        public virtual string Gm4 { get; set; }

        /// <summary>
        /// 左通道本底
        /// </summary>
		[DataMember]
        public virtual string BendiL { get; set; }

        /// <summary>
        /// 右通道本底
        /// </summary>
		[DataMember]
        public virtual string BendiR { get; set; }

        /// <summary>
        /// 检测时间
        /// </summary>
		[DataMember]
        public virtual string Testtime { get; set; }

        /// <summary>
        /// 仪器编号
        /// </summary>
		[DataMember]
        public virtual string DeviceNo { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
		[DataMember]
        public virtual DateTime Recordtime { get; set; }


        #endregion

    }
}