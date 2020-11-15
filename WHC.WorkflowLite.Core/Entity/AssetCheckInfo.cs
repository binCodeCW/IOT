using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 资产盘点主表
    /// </summary>
    [DataContract]
    [Serializable]
    public class AssetCheckInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public AssetCheckInfo()
		{
            this.ID= System.Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
            this.ApplyDate = DateTime.Now;
		}

        #region Property Members
        
        /// <summary>
        /// 序号
        /// </summary>
		[DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 盘点单号
        /// </summary>
		[DataMember]
        public virtual string BillNo { get; set; }

        /// <summary>
        /// 盘点公司
        /// </summary>
		[DataMember]
        public virtual string Company_ID { get; set; }

        /// <summary>
        /// 盘点部门
        /// </summary>
		[DataMember]
        public virtual string Dept_ID { get; set; }

        /// <summary>
        /// 盘点数量
        /// </summary>
		[DataMember]
        public virtual int CheckQty { get; set; }

        /// <summary>
        /// 已盘数量
        /// </summary>
		[DataMember]
        public virtual int DoneQty { get; set; }

        /// <summary>
        /// 未盘数量
        /// </summary>
		[DataMember]
        public virtual int TodoQty { get; set; }

        /// <summary>
        /// 单据状态
        /// </summary>
		[DataMember]
        public virtual int CheckStatus { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
		[DataMember]
        public virtual int TaskStatus { get; set; }

        /// <summary>
        /// 申请日期
        /// </summary>
		[DataMember]
        public virtual DateTime ApplyDate { get; set; }

        /// <summary>
        /// 备注
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
        /// 企业号UserID
        /// </summary>
		[DataMember]
        public virtual string CorpUserId { get; set; }

        /// <summary>
        /// 指定盘点人
        /// </summary>
		[DataMember]
        public virtual string CheckPerson { get; set; }


        #endregion

    }
}