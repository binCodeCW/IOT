using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// YH08_DeviceDataRecInfo
    /// </summary>
    [DataContract]
    [Serializable]
    public class YH08_DeviceDataRecInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public YH08_DeviceDataRecInfo()
		{
            this.ID= 0;
                  
		}

        #region Property Members
        
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
        /// 耗材用量
        /// </summary>
		[DataMember]
        public virtual string Usagecount { get; set; }

        /// <summary>
        /// DOB值
        /// </summary>
		[DataMember]
        public virtual string Dob { get; set; }

        /// <summary>
        /// 初步碳12底气浓度
        /// </summary>
		[DataMember]
        public virtual string M_lC12B { get; set; }

        /// <summary>
        /// 初步碳12样气浓度
        /// </summary>
		[DataMember]
        public virtual string M_lC12S { get; set; }

        /// <summary>
        /// 检测通道
        /// </summary>
		[DataMember]
        public virtual string CheckChannel { get; set; }

        /// <summary>
        /// 底气碳12浓度
        /// </summary>
		[DataMember]
        public virtual string Cbc12 { get; set; }

        /// <summary>
        /// 样气碳12浓度
        /// </summary>
		[DataMember]
        public virtual string Csc12 { get; set; }

        /// <summary>
        /// 底气碳13浓度
        /// </summary>
		[DataMember]
        public virtual string Cbc13 { get; set; }

        /// <summary>
        /// 样气碳13浓度
        /// </summary>
		[DataMember]
        public virtual string Csc13 { get; set; }

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