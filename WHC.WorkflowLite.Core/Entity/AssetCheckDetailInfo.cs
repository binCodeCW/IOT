using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 资产盘点明细
    /// </summary>
    [DataContract]
    [Serializable]
    public class AssetCheckDetailInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public AssetCheckDetailInfo()
		{
            this.ID= System.Guid.NewGuid().ToString();
            this.AttachGUID= System.Guid.NewGuid().ToString();

            this.CreateTime = DateTime.Now;
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
        /// 资产代码
        /// </summary>
		[DataMember]
        public virtual string AssetCode { get; set; }

        /// <summary>
        /// 资产名称
        /// </summary>
		[DataMember]
        public virtual string AssetName { get; set; }

        /// <summary>
        /// 资产图片
        /// </summary>
		[DataMember]
        public virtual string AttachGUID { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
		[DataMember]
        public virtual int Qty { get; set; }

        /// <summary>
        /// 使用人
        /// </summary>
		[DataMember]
        public virtual string UsePerson { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
		[DataMember]
        public virtual string CurrDept { get; set; }

        /// <summary>
        /// 存放地点
        /// </summary>
		[DataMember]
        public virtual string KeepAddr { get; set; }

        /// <summary>
        /// 盘点结果
        /// </summary>
		[DataMember]
        public virtual int CheckResult { get; set; }

        /// <summary>
        /// 盘点结果表达
        /// </summary>
		[DataMember]
        public virtual string CheckDisplay { get; set; }

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
        /// 企业号UserID
        /// </summary>
		[DataMember]
        public virtual string CorpUserId { get; set; }

        /// <summary>
        /// 盘点人
        /// </summary>
		[DataMember]
        public virtual string CheckPerson { get; set; }

        /// <summary>
        /// 盘点日期
        /// </summary>
		[DataMember]
        public virtual DateTime? CheckDate { get; set; }


        #endregion

    }
}