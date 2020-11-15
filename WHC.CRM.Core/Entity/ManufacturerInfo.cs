using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 供应商信息
    /// </summary>
    [DataContract]
    public class ManufacturerInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ManufacturerInfo()
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
        /// 供应商名称
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 供应商电话
        /// </summary>
        [DataMember]
        public virtual string Telephone { get; set; }

        /// <summary>
        /// 供应商手机
        /// </summary>
        [DataMember]
        public virtual string Mobile { get; set; }

        /// <summary>
        /// 供应商地址
        /// </summary>
        [DataMember]
        public virtual string Address { get; set; }

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
        /// 邮政编码
        /// </summary>
        [DataMember]
        public virtual string ZipCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        [DataMember]
        public virtual string Seq { get; set; }

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