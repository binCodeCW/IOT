using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// YH04_TestDataInfo
    /// </summary>
    [DataContract]
    [Serializable]
    public class YH04_TestDataInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public YH04_TestDataInfo()
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
        /// 本底
        /// </summary>
		[DataMember]
        public virtual string Bendi { get; set; }

        /// <summary>
        /// 检测时间
        /// </summary>
		[DataMember]
        public virtual string Testtime { get; set; }

        /// <summary>
        /// 耗材用量
        /// </summary>
		[DataMember]
        public virtual string UsageCount { get; set; }

        /// <summary>
        /// 检测结果
        /// </summary>
		[DataMember]
        public virtual string TestResult { get; set; }

        /// <summary>
        /// DPM值
        /// </summary>
		[DataMember]
        public virtual string Dpm { get; set; }

        /// <summary>
        /// 检测计数1
        /// </summary>
		[DataMember]
        public virtual string C1 { get; set; }

        /// <summary>
        /// 检测计数2
        /// </summary>
		[DataMember]
        public virtual string C2 { get; set; }

        /// <summary>
        /// 常数H
        /// </summary>
		[DataMember]
        public virtual string SnH { get; set; }

        /// <summary>
        /// 常数L
        /// </summary>
		[DataMember]
        public virtual string SnL { get; set; }

        /// <summary>
        /// 阴性上限
        /// </summary>
		[DataMember]
        public virtual string NegativeLimit { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
		[DataMember]
        public virtual DateTime Recordtime { get; set; }

        /// <summary>
        /// 仪器编号
        /// </summary>
		[DataMember]
        public virtual string DeviceNo { get; set; }


        #endregion

    }
}