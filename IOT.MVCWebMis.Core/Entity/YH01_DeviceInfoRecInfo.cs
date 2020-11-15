﻿using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// YH01_DeviceInfoRecInfo
    /// </summary>
    [DataContract]
    [Serializable]
    public class YH01_DeviceInfoRecInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public YH01_DeviceInfoRecInfo()
		{
            this.ID= 0;
                          this.Deviceno= "NULL";
  
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
        /// 检测时长
        /// </summary>
		[DataMember]
        public virtual string Time { get; set; }

        /// <summary>
        /// 探测效率
        /// </summary>
		[DataMember]
        public virtual string Efficiency { get; set; }

        /// <summary>
        /// 本底
        /// </summary>
		[DataMember]
        public virtual string Bendi { get; set; }

        /// <summary>
        /// 自动模式
        /// </summary>
		[DataMember]
        public virtual string Autoresult { get; set; }

        /// <summary>
        /// 检测时是否扫码
        /// </summary>
		[DataMember]
        public virtual string Enscan { get; set; }

        /// <summary>
        /// 零位校准方向
        /// </summary>
		[DataMember]
        public virtual string Zpcorrectiondirection { get; set; }

        /// <summary>
        /// 零位校准脉冲
        /// </summary>
		[DataMember]
        public virtual string Zpcorrectionpulse { get; set; }

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