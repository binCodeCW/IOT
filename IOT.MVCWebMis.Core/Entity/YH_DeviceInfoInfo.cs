using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// YH_DeviceInfoInfo
    /// </summary>
    [DataContract]
    [Serializable]
    public class YH_DeviceInfoInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public YH_DeviceInfoInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Longtitude = 0;
            this.Latitude = 0;
            this.OlineStatus = 0;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 仪器编号
        /// </summary>
		[DataMember]
        public virtual string DeviceId { get; set; }

        /// <summary>
        /// 仪器类型
        /// </summary>
		[DataMember]
        public virtual string DeviceType { get; set; }

        /// <summary>
        /// 医院简称
        /// </summary>
		[DataMember]
        public virtual string SimpleName { get; set; }

        /// <summary>
        /// 所在省份
        /// </summary>
		[DataMember]
        public virtual string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
		[DataMember]
        public virtual string City { get; set; }

        /// <summary>
        /// 所在行政区
        /// </summary>
		[DataMember]
        public virtual string District { get; set; }

        /// <summary>
        /// 市场分区
        /// </summary>
		[DataMember]
        public virtual string Area { get; set; }

        /// <summary>
        /// 医院地址
        /// </summary>
		[DataMember]
        public virtual string Address { get; set; }

        /// <summary>
        /// 医院邮编
        /// </summary>
		[DataMember]
        public virtual string ZipCode { get; set; }

        /// <summary>
        /// 医院电话
        /// </summary>
		[DataMember]
        public virtual string Telephone { get; set; }

        /// <summary>
        /// 传真号码
        /// </summary>
		[DataMember]
        public virtual string Fax { get; set; }

        /// <summary>
        /// 主联系人
        /// </summary>
		[DataMember]
        public virtual string Contact { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
		[DataMember]
        public virtual string ContactPhone { get; set; }

        /// <summary>
        /// 联系人手机
        /// </summary>
		[DataMember]
        public virtual string ContactMobile { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
		[DataMember]
        public virtual string Email { get; set; }

        /// <summary>
        /// 客户类别
        /// </summary>
		[DataMember]
        public virtual string CustomerType { get; set; }

        /// <summary>
        /// 客户级别
        /// </summary>
		[DataMember]
        public virtual string Grade { get; set; }

        /// <summary>
        /// 信用等级
        /// </summary>
		[DataMember]
        public virtual string CreditStatus { get; set; }

        /// <summary>
        /// 重要级别
        /// </summary>
		[DataMember]
        public virtual string Importance { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
		[DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
		[DataMember]
        public virtual string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
		[DataMember]
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 编辑人
        /// </summary>
		[DataMember]
        public virtual string Editor { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
		[DataMember]
        public virtual DateTime EditTime { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
		[DataMember]
        public virtual string IotID { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
		[DataMember]
        public virtual string Company_ID { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
		[DataMember]
        public virtual double Longtitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
		[DataMember]
        public virtual double Latitude { get; set; }

        /// <summary>
        /// 仪器在线状态
        /// </summary>
		[DataMember]
        public virtual int OlineStatus { get; set; }

        #endregion

    }
}