using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 用车申请单
    /// </summary>
    [DataContract]
    public class CarUsageInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public CarUsageInfo()
		{
            this.ID= System.Guid.NewGuid().ToString();
            this.AttachGUID = System.Guid.NewGuid().ToString();
            this.Apply_ID = System.Guid.NewGuid().ToString(); //申请单编号(业务对象负责生成）
          
		}

        #region Property Members
        
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 用车事由
        /// </summary>
		[DataMember]
        public virtual string Reason { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
		[DataMember]
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
		[DataMember]
        public virtual DateTime EndTime { get; set; }

        /// <summary>
        /// 出发地点
        /// </summary>
		[DataMember]
        public virtual string StartLocation { get; set; }

        /// <summary>
        /// 用车时长
        /// </summary>
		[DataMember]
        public virtual string Duration { get; set; }

        /// <summary>
        /// 目的地点
        /// </summary>
		[DataMember]
        public virtual string Destination { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
		[DataMember]
        public virtual string CarType { get; set; }

        /// <summary>
        /// 附件组别ID
        /// </summary>
		[DataMember]
        public virtual string AttachGUID { get; set; }

        /// <summary>
        /// 申请单编号
        /// </summary>
		[DataMember]
        public virtual string Apply_ID { get; set; }

        /// <summary>
        /// 申请单日期
        /// </summary>
		[DataMember]
        public virtual DateTime ApplyDate { get; set; }

        /// <summary>
        /// 申请部门
        /// </summary>
		[DataMember]
        public virtual string ApplyDept { get; set; }

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


        #endregion

    }
}