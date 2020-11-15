﻿using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 物品领用申请单
    /// </summary>
    [DataContract]
    public class GoodsReceiveInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public GoodsReceiveInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Quantity = 0;
            this.AttachGUID = System.Guid.NewGuid().ToString();
            this.Apply_ID = System.Guid.NewGuid().ToString(); //申请单编号(业务对象负责生成）

        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 用途
        /// </summary>
        [DataMember]
        public virtual string Usage { get; set; }

        /// <summary>
        /// 物品名称
        /// </summary>
        [DataMember]
        public virtual string ItemName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public virtual decimal Quantity { get; set; }

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