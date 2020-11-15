using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.CRM.Entity
{
    /// <summary>
    /// 客户联系人
    /// </summary>
    [DataContract]
    public class ContactInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public ContactInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.AttachGUID = System.Guid.NewGuid().ToString();
            this.Deleted = false;
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
        /// 供应商ID
        /// </summary>
        [DataMember]
        public virtual string Supplier_ID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public virtual string HandNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public virtual string Name { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [DataMember]
        public virtual string IDCarNo { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [DataMember]
        public virtual DateTime Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public virtual string Sex { get; set; }

        /// <summary>
        /// 办公电话
        /// </summary>
        [DataMember]
        public virtual string OfficePhone { get; set; }

        /// <summary>
        /// 家庭电话
        /// </summary>
        [DataMember]
        public virtual string HomePhone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [DataMember]
        public virtual string Fax { get; set; }

        /// <summary>
        /// 联系人手机
        /// </summary>
        [DataMember]
        public virtual string Mobile { get; set; }

        /// <summary>
        /// 联系人地址
        /// </summary>
        [DataMember]
        public virtual string Address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [DataMember]
        public virtual string ZipCode { get; set; }

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
        /// 籍贯
        /// </summary>
        [DataMember]
        public virtual string Hometown { get; set; }

        /// <summary>
        /// 家庭住址
        /// </summary>
        [DataMember]
        public virtual string HomeAddress { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        [DataMember]
        public virtual string Nationality { get; set; }

        /// <summary>
        /// 教育程度
        /// </summary>
        [DataMember]
        public virtual string Eduction { get; set; }

        /// <summary>
        /// 毕业学校
        /// </summary>
        [DataMember]
        public virtual string GraduateSchool { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>
        [DataMember]
        public virtual string Political { get; set; }

        /// <summary>
        /// 职业类型
        /// </summary>
        [DataMember]
        public virtual string JobType { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        [DataMember]
        public virtual string Titles { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        [DataMember]
        public virtual string Rank { get; set; }

        /// <summary>
        /// 所在部门
        /// </summary>
        [DataMember]
        public virtual string Department { get; set; }

        /// <summary>
        /// 爱好
        /// </summary>
        [DataMember]
        public virtual string Hobby { get; set; }

        /// <summary>
        /// 属相
        /// </summary>
        [DataMember]
        public virtual string Animal { get; set; }

        /// <summary>
        /// 星座
        /// </summary>
        [DataMember]
        public virtual string Constellation { get; set; }

        /// <summary>
        /// 婚姻状态
        /// </summary>
        [DataMember]
        public virtual string MarriageStatus { get; set; }

        /// <summary>
        /// 健康状况
        /// </summary>
        [DataMember]
        public virtual string HealthCondition { get; set; }

        /// <summary>
        /// 重要级别
        /// </summary>
        [DataMember]
        public virtual string Importance { get; set; }

        /// <summary>
        /// 认可程度
        /// </summary>
        [DataMember]
        public virtual string Recognition { get; set; }

        /// <summary>
        /// 关系
        /// </summary>
        [DataMember]
        public virtual string RelationShip { get; set; }

        /// <summary>
        /// 负责需求
        /// </summary>
        [DataMember]
        public virtual string ResponseDemand { get; set; }

        /// <summary>
        /// 关心重点
        /// </summary>
        [DataMember]
        public virtual string CareFocus { get; set; }

        /// <summary>
        /// 利益诉求
        /// </summary>
        [DataMember]
        public virtual string InterestDemand { get; set; }

        /// <summary>
        /// 体型
        /// </summary>
        [DataMember]
        public virtual string BodyType { get; set; }

        /// <summary>
        /// 吸烟
        /// </summary>
        [DataMember]
        public virtual string Smoking { get; set; }

        /// <summary>
        /// 喝酒
        /// </summary>
        [DataMember]
        public virtual string Drink { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        [DataMember]
        public virtual string Height { get; set; }

        /// <summary>
        /// 体重
        /// </summary>
        [DataMember]
        public virtual string Weight { get; set; }

        /// <summary>
        /// 视力
        /// </summary>
        [DataMember]
        public virtual string Vision { get; set; }

        /// <summary>
        /// 个人简述
        /// </summary>
        [DataMember]
        public virtual string Introduce { get; set; }

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


        #endregion

    }
}