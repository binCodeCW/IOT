using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 供应商
    /// </summary>
    [DataContract]
    public class SupplierInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public SupplierInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.RegisterCapital = 0;
            this.TurnOver = 0;
            this.CompanyPictureGUID = System.Guid.NewGuid().ToString();
            this.AttachGUID = System.Guid.NewGuid().ToString();
            this.IsPublic = false;
            this.Satisfaction = 0;
            this.TransactionCount = 0;
            this.TransactionTotal = 0;
            this.Deleted = false;
            this.CreateTime = DateTime.Now;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public virtual string HandNo { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 供应商简称
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
        /// 公司地址
        /// </summary>
        [DataMember]
        public virtual string Address { get; set; }

        /// <summary>
        /// 公司邮编
        /// </summary>
        [DataMember]
        public virtual string ZipCode { get; set; }

        /// <summary>
        /// 办公电话
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
        /// QQ号码
        /// </summary>
        [DataMember]
        public virtual string QQ { get; set; }

        /// <summary>
        /// 所属行业
        /// </summary>
        [DataMember]
        public virtual string Industry { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        [DataMember]
        public virtual string BusinessScope { get; set; }

        /// <summary>
        /// 经营品牌
        /// </summary>
        [DataMember]
        public virtual string Brand { get; set; }

        /// <summary>
        /// 主要客户群
        /// </summary>
        [DataMember]
        public virtual string PrimaryClient { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        [DataMember]
        public virtual string PrimaryBusiness { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        [DataMember]
        public virtual decimal RegisterCapital { get; set; }

        /// <summary>
        /// 营业额
        /// </summary>
        [DataMember]
        public virtual decimal TurnOver { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        [DataMember]
        public virtual string LicenseNo { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        [DataMember]
        public virtual string Bank { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        [DataMember]
        public virtual string BankAccount { get; set; }

        /// <summary>
        /// 地税登记号
        /// </summary>
        [DataMember]
        public virtual string LocalTaxNo { get; set; }

        /// <summary>
        /// 国税登记号
        /// </summary>
        [DataMember]
        public virtual string NationalTaxNo { get; set; }

        /// <summary>
        /// 法人名称
        /// </summary>
        [DataMember]
        public virtual string LegalMan { get; set; }

        /// <summary>
        /// 法人电话
        /// </summary>
        [DataMember]
        public virtual string LegalTelephone { get; set; }

        /// <summary>
        /// 法人手机
        /// </summary>
        [DataMember]
        public virtual string LegalMobile { get; set; }

        /// <summary>
        /// 供应商来源
        /// </summary>
        [DataMember]
        public virtual string Source { get; set; }

        /// <summary>
        /// 单位网站
        /// </summary>
        [DataMember]
        public virtual string WebSite { get; set; }

        /// <summary>
        /// 公司图片信息
        /// </summary>
        [DataMember]
        public virtual string CompanyPictureGUID { get; set; }

        /// <summary>
        /// 附件组别ID
        /// </summary>
        [DataMember]
        public virtual string AttachGUID { get; set; }

        /// <summary>
        /// 供应商类别
        /// </summary>
        [DataMember]
        public virtual string CustomerType { get; set; }

        /// <summary>
        /// 供应商级别
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
        /// 公开与否
        /// </summary>
        [DataMember]
        public virtual bool IsPublic { get; set; }

        /// <summary>
        /// 客户满意度
        /// </summary>
        [DataMember]
        public virtual int Satisfaction { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 交易次数
        /// </summary>
        [DataMember]
        public virtual int TransactionCount { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        [DataMember]
        public virtual decimal TransactionTotal { get; set; }

        /// <summary>
        /// 首次交易时间
        /// </summary>
        [DataMember]
        public virtual DateTime TransactionFirstDay { get; set; }

        /// <summary>
        /// 最近交易时间
        /// </summary>
        [DataMember]
        public virtual DateTime TransactionLastDay { get; set; }

        /// <summary>
        /// 最近联系日期
        /// </summary>
        [DataMember]
        public virtual DateTime LastContactDate { get; set; }

        /// <summary>
        /// 客户阶段
        /// </summary>
        [DataMember]
        public virtual string Stage { get; set; }

        /// <summary>
        /// 客户状态
        /// </summary>
        [DataMember]
        public virtual string Status { get; set; }

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
        public virtual bool Deleted { get; set; }

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

        /// <summary>
        /// 标记颜色
        /// </summary>
        [DataMember]
        public virtual string MarkColor { get; set; }

        /// <summary>
        /// 业务分享用户
        /// </summary>
        [DataMember]
        public virtual string ShareUsers { get; set; }

        #endregion

    }
}