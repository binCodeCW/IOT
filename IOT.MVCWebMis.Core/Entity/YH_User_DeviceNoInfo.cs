using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// 用户和仪器编号配置表
    /// </summary>
    [DataContract]
    [Serializable]
    public class YH_User_DeviceNoInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public YH_User_DeviceNoInfo()
		{
            this.ID= 0;
            //this.ID = this.ID + 1;
    
		}

        #region Property Members
        
		[DataMember]
        public virtual int ID { get; set; }

        /// <summary>
        /// 用户名/登录名
        /// </summary>
		[DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 仪器编号
        /// </summary>
		[DataMember]
        public virtual string DeviceNo { get; set; }


        #endregion

    }
}