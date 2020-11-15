using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 竞争对手信息
    /// </summary>
    [DataContract]
    public class CompetitorInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public CompetitorInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.AttachGUID = System.Guid.NewGuid().ToString();
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
        /// 对手名称
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 对手简称
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
        /// 单位网站
        /// </summary>
        [DataMember]
        public virtual string WebSite { get; set; }

        /// <summary>
        /// 附件组别ID
        /// </summary>
        [DataMember]
        public virtual string AttachGUID { get; set; }

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