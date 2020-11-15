using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.ContactBook.Entity
{
    /// <summary>
    /// 通讯录联系人
    /// </summary>
    [DataContract]
    public class AddressInfo : BaseEntity
    {    
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public AddressInfo()
        {
            this.ID = Guid.NewGuid().ToString();
            this.AddressType = AddressType.个人; //通讯录类型[个人,公司]  
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 通讯录类型[个人,公司]
        /// </summary>
        [DataMember]
        public virtual AddressType AddressType { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public virtual string Sex { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [DataMember]
        public virtual DateTime Birthdate { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [DataMember]
        public virtual string Mobile { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [DataMember]
        public virtual string Email { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        [DataMember]
        public virtual string QQ { get; set; }

        /// <summary>
        /// 家庭电话
        /// </summary>
        [DataMember]
        public virtual string HomeTelephone { get; set; }

        /// <summary>
        /// 办公电话
        /// </summary>
        [DataMember]
        public virtual string OfficeTelephone { get; set; }

        /// <summary>
        /// 家庭住址
        /// </summary>
        [DataMember]
        public virtual string HomeAddress { get; set; }

        /// <summary>
        /// 办公地址
        /// </summary>
        [DataMember]
        public virtual string OfficeAddress { get; set; }

        /// <summary>
        /// 传真号码
        /// </summary>
        [DataMember]
        public virtual string Fax { get; set; }

        /// <summary>
        /// 公司单位
        /// </summary>
        [DataMember]
        public virtual string Company { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [DataMember]
        public virtual string Dept { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [DataMember]
        public virtual string Position { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        [DataMember]
        public virtual string Other { get; set; }

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

    /// <summary>
    /// 通讯录类型
    /// </summary>
    public enum AddressType { 个人, 公共}
}