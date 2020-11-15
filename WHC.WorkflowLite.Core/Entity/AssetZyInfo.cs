﻿using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 资产转移单
    /// </summary>
    [DataContract]
    [Serializable]
    public class AssetZyInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public AssetZyInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Apply_ID = System.Guid.NewGuid().ToString();//申请单编号(业务对象负责生成）
            this.AttachGUID = System.Guid.NewGuid().ToString();

        }

        #region Property Members

        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 转移单号
        /// </summary>
		[DataMember]
        public virtual string BillNo { get; set; }

        /// <summary>
        /// 转移资产
        /// </summary>
		[DataMember]
        public virtual string AssetDesc { get; set; }

        /// <summary>
        /// 资产转出部门（单位）
        /// </summary>
		[DataMember]
        public virtual string OutDept { get; set; }

        /// <summary>
        /// 资产转入部门（单位）
        /// </summary>
		[DataMember]
        public virtual string InDept { get; set; }

        /// <summary>
        /// 资产管理部门（单位）
        /// </summary>
		[DataMember]
        public virtual string ChargeDept { get; set; }

        /// <summary>
        /// 转出部门资产管理员
        /// </summary>
		[DataMember]
        public virtual string DeptAdmin { get; set; }

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