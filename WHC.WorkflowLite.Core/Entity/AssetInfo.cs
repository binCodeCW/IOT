using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 实物资产表
    /// </summary>
    [DataContract]
    [Serializable]
    public class AssetInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public AssetInfo()
		{
		}

        #region Property Members
        
        /// <summary>
        /// 编号
        /// </summary>
		[DataMember]
        public virtual decimal ID { get; set; }

        /// <summary>
        /// 资产编码
        /// </summary>
		[DataMember]
        public virtual string Code { get; set; }

        /// <summary>
        /// 资产类别
        /// </summary>
		[DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
		[DataMember]
        public virtual string ClassName { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
		[DataMember]
        public virtual string CurrDept { get; set; }

        /// <summary>
        /// 管理部门
        /// </summary>
		[DataMember]
        public virtual string ChargeDept { get; set; }

        /// <summary>
        /// 增加方式
        /// </summary>
		[DataMember]
        public virtual string AddMethod { get; set; }

        /// <summary>
        /// 使用状况
        /// </summary>
		[DataMember]
        public virtual string UseStatus { get; set; }

        /// <summary>
        /// 购置日期
        /// </summary>
		[DataMember]
        public virtual DateTime PurchaseDate { get; set; }

        /// <summary>
        /// 使用人
        /// </summary>
		[DataMember]
        public virtual string UsePerson { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
		[DataMember]
        public virtual string Spec { get; set; }

        /// <summary>
        /// 存放地点
        /// </summary>
		[DataMember]
        public virtual string KeepAddr { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
		[DataMember]
        public virtual int Qty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
		[DataMember]
        public virtual string Remark { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
		[DataMember]
        public virtual string Unit { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
		[DataMember]
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 本币原值
        /// </summary>
		[DataMember]
        public virtual decimal OriginValue { get; set; }

        /// <summary>
        /// 使用年限
        /// </summary>
		[DataMember]
        public virtual int LimitYears { get; set; }

        /// <summary>
        /// 使用到期日
        /// </summary>
		[DataMember]
        public virtual DateTime DueDate { get; set; }

        /// <summary>
        /// 已使用年数
        /// </summary>
		[DataMember]
        public virtual string UseYears { get; set; }

        /// <summary>
        /// 是否超年限
        /// </summary>
		[DataMember]
        public virtual string IsAge { get; set; }

        /// <summary>
        /// 资产动态
        /// </summary>
		[DataMember]
        public virtual string Status { get; set; }

        /// <summary>
        /// 在用类型
        /// </summary>
		[DataMember]
        public virtual string UseType { get; set; }

        /// <summary>
        /// 保管人
        /// </summary>
		[DataMember]
        public virtual string Keeper { get; set; }

        /// <summary>
        /// 盘点状态
        /// </summary>
		[DataMember]
        public virtual string InvStatus { get; set; }

        /// <summary>
        /// 盘点日期
        /// </summary>
		[DataMember]
        public virtual DateTime InvDate { get; set; }

        /// <summary>
        /// 盘点人
        /// </summary>
		[DataMember]
        public virtual string Inventory { get; set; }

        /// <summary>
        /// 财务对账
        /// </summary>
		[DataMember]
        public virtual string Account { get; set; }

        /// <summary>
        /// 对账日期
        /// </summary>
		[DataMember]
        public virtual DateTime AccDate { get; set; }

        /// <summary>
        /// 核对人
        /// </summary>
		[DataMember]
        public virtual string AccHolder { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
		[DataMember]
        public virtual string UserName { get; set; }

        /// <summary>
        /// 自定义1
        /// </summary>
		[DataMember]
        public virtual string UserDef1 { get; set; }

        /// <summary>
        /// 自定义2
        /// </summary>
		[DataMember]
        public virtual string UserDef2 { get; set; }

        /// <summary>
        /// 自定义3
        /// </summary>
		[DataMember]
        public virtual string UserDef3 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
		[DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
		[DataMember]
        public virtual string Operator { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
		[DataMember]
        public virtual DateTime OperateDate { get; set; }


        #endregion

    }
}