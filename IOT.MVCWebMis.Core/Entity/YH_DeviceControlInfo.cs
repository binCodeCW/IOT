using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// YH_DeviceControlInfo
    /// </summary>
    [DataContract]
    [Serializable]
    public class YH_DeviceControlInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public YH_DeviceControlInfo()
		{
            this.ID= System.Guid.NewGuid().ToString();
            this.Port= 0;
            this.Ifint= false;
   
		}

        #region Property Members
        
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// HTTPS接入方式IP
        /// </summary>
		[DataMember]
        public virtual string Ip { get; set; }

        /// <summary>
        /// HTTPS接入方式端口
        /// </summary>
		[DataMember]
        public virtual int Port { get; set; }

        /// <summary>
        /// 项目应用ID
        /// </summary>
		[DataMember]
        public virtual string Appid { get; set; }

        /// <summary>
        /// 项目应用密钥
        /// </summary>
		[DataMember]
        public virtual string Appsecret { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
		[DataMember]
        public virtual string Serviceid { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
		[DataMember]
        public virtual string Deviceid { get; set; }

        /// <summary>
        /// 命令名称
        /// </summary>
		[DataMember]
        public virtual string Cmd { get; set; }

        /// <summary>
        /// 命令字段属性名称
        /// </summary>
		[DataMember]
        public virtual string Attribute { get; set; }

        /// <summary>
        /// 上报数据是否是数字
        /// </summary>
		[DataMember]
        public virtual bool Ifint { get; set; }

        /// <summary>
        /// 上报数据内容
        /// </summary>
		[DataMember]
        public virtual string Data { get; set; }


        #endregion

    }
}