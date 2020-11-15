﻿using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 物品维修申请单
    /// </summary>
    [DataContract]
    public class MaintenanceInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public MaintenanceInfo()
		{
            this.ID= System.Guid.NewGuid().ToString();
                this.RepairFee= 0;
                this.AttachGUID = System.Guid.NewGuid().ToString();
                this.Apply_ID = System.Guid.NewGuid().ToString(); //申请单编号(业务对象负责生成）
      
		}

        #region Property Members
        
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 故障设备名称
        /// </summary>
		[DataMember]
        public virtual string DeviceName { get; set; }

        /// <summary>
        /// 故障描述
        /// </summary>
		[DataMember]
        public virtual string FaultDescription { get; set; }

        /// <summary>
        /// 报修日期
        /// </summary>
		[DataMember]
        public virtual DateTime RepairDate { get; set; }

        /// <summary>
        /// 预计维修费用
        /// </summary>
		[DataMember]
        public virtual decimal RepairFee { get; set; }

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
        /// 附件组别ID
        /// </summary>
		[DataMember]
        public virtual string AttachGUID { get; set; }

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