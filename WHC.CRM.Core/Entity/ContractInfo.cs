using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 客户合同信息
    /// </summary>
    [DataContract]
    public class ContractInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ContractInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.ContractMoney = 0;
            this.AttachGUID = System.Guid.NewGuid().ToString();
            this.CreateTime = DateTime.Now;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [DataMember]
        public virtual string Customer_ID { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        [DataMember]
        public virtual string HandNo { get; set; }

        /// <summary>
        /// 收支类型
        /// </summary>
        [DataMember]
        public virtual string ExpenditureType { get; set; }

        /// <summary>
        /// 合同类型
        /// </summary>
        [DataMember]
        public virtual string ContractType { get; set; }

        /// <summary>
        /// 合同名称
        /// </summary>
        [DataMember]
        public virtual string ContractName { get; set; }

        /// <summary>
        /// 合同金额
        /// </summary>
        [DataMember]
        public virtual decimal ContractMoney { get; set; }

        /// <summary>
        /// 公司签约人
        /// </summary>
        [DataMember]
        public virtual string CompanySigner { get; set; }

        /// <summary>
        /// 客户签约人
        /// </summary>
        [DataMember]
        public virtual string CustomerSigner { get; set; }

        /// <summary>
        /// 签约日期
        /// </summary>
        [DataMember]
        public virtual DateTime SignDate { get; set; }

        /// <summary>
        /// 签约地点
        /// </summary>
        [DataMember]
        public virtual string SignLocation { get; set; }

        /// <summary>
        /// 乙方名称
        /// </summary>
        [DataMember]
        public virtual string PartyBName { get; set; }

        /// <summary>
        /// 合同开始日期
        /// </summary>
        [DataMember]
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// 合同结束日期
        /// </summary>
        [DataMember]
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// 结算情况
        /// </summary>
        [DataMember]
        public virtual string Settlement { get; set; }

        /// <summary>
        /// 合同状态
        /// </summary>
        [DataMember]
        public virtual string Status { get; set; }

        /// <summary>
        /// 关联项目
        /// </summary>
        [DataMember]
        public virtual string RelatedItems { get; set; }

        /// <summary>
        /// 联系人
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
        /// 合同内容
        /// </summary>
        [DataMember]
        public virtual string Content { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 附件组别ID
        /// </summary>
        [DataMember]
        public virtual string AttachGUID { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        [DataMember]
        public virtual string Operator { get; set; }

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
        /// 所属部门
        /// </summary>
        [DataMember]
        public virtual string Dept_ID { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        [DataMember]
        public virtual string Company_ID { get; set; }


        #endregion

    }
}