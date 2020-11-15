using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// YH08_DeviceInfoRecInfo
    /// </summary>
    [DataContract]
    [Serializable]
    public class YH08_DeviceInfoRecInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public YH08_DeviceInfoRecInfo()
		{
            this.ID= 0;
		}

        #region Property Members
        
        /// <summary>
        /// 数据行，递增1
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
        /// 质控第一次DOB
        /// </summary>
		[DataMember]
        public virtual string Dob0 { get; set; }

        /// <summary>
        /// 质控第二次DOB
        /// </summary>
		[DataMember]
        public virtual string Dob1 { get; set; }

        /// <summary>
        /// 质控第三次DOB
        /// </summary>
		[DataMember]
        public virtual string Dob2 { get; set; }

        /// <summary>
        /// 质控第四次DOB
        /// </summary>
		[DataMember]
        public virtual string Dob3 { get; set; }

        /// <summary>
        /// 质控第五次DOB
        /// </summary>
		[DataMember]
        public virtual string Dob4 { get; set; }

        /// <summary>
        /// 质控第六次DOB
        /// </summary>
		[DataMember]
        public virtual string Dob5 { get; set; }

        /// <summary>
        /// 质控第七次DOB
        /// </summary>
		[DataMember]
        public virtual string Dob6 { get; set; }

        /// <summary>
        /// 质控第八次DOB
        /// </summary>
		[DataMember]
        public virtual string Dob7 { get; set; }

        /// <summary>
        /// 质控第九次DOB
        /// </summary>
		[DataMember]
        public virtual string Dob8 { get; set; }

        /// <summary>
        /// 质控第十次DOB
        /// </summary>
		[DataMember]
        public virtual string Dob9 { get; set; }

        /// <summary>
        /// 仪器编号
        /// </summary>
		[DataMember]
        public virtual string DeviceNo { get; set; }

        /// <summary>
        /// 参数P
        /// </summary>
		[DataMember]
        public virtual string P { get; set; }

        /// <summary>
        /// 参数T
        /// </summary>
		[DataMember]
        public virtual string T { get; set; }

        /// <summary>
        /// 压力
        /// </summary>
		[DataMember]
        public virtual string PressType { get; set; }

        /// <summary>
        /// 碳12曲线系数1
        /// </summary>
		[DataMember]
        public virtual string M_lCfC12_0 { get; set; }

        /// <summary>
        /// 碳12曲线系数2
        /// </summary>
		[DataMember]
        public virtual string M_lCfC12_1 { get; set; }

        /// <summary>
        /// 碳12曲线系数3
        /// </summary>
		[DataMember]
        public virtual string M_lCfC12_2 { get; set; }

        /// <summary>
        /// 碳12曲线系数4
        /// </summary>
		[DataMember]
        public virtual string M_lCfC12_3 { get; set; }

        /// <summary>
        /// 碳12曲线系数5
        /// </summary>
		[DataMember]
        public virtual string M_lCfC12_4 { get; set; }

        /// <summary>
        /// 碳12曲线系数6
        /// </summary>
		[DataMember]
        public virtual string M_lCfC12_5 { get; set; }

        /// <summary>
        /// 碳13曲线系数1
        /// </summary>
		[DataMember]
        public virtual string M_lCfC13_0 { get; set; }

        /// <summary>
        /// 碳13曲线系数2
        /// </summary>
		[DataMember]
        public virtual string M_lCfC13_1 { get; set; }

        /// <summary>
        /// 碳13曲线系数3
        /// </summary>
		[DataMember]
        public virtual string M_lCfC13_2 { get; set; }

        /// <summary>
        /// 碳13曲线系数4
        /// </summary>
		[DataMember]
        public virtual string M_lCfC13_3 { get; set; }

        /// <summary>
        /// 碳13曲线系数5
        /// </summary>
		[DataMember]
        public virtual string M_lCfC13_4 { get; set; }

        /// <summary>
        /// 碳13曲线系数6
        /// </summary>
		[DataMember]
        public virtual string M_lCfC13_5 { get; set; }

        /// <summary>
        /// 平均值
        /// </summary>
		[DataMember]
        public virtual string Aver { get; set; }

        /// <summary>
        /// 标准方差
        /// </summary>
		[DataMember]
        public virtual string Se { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
		[DataMember]
        public virtual DateTime Recordtime { get; set; }


        #endregion

    }
}