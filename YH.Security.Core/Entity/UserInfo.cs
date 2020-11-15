using System;
using System.Runtime.Serialization;

namespace YH.Security.Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class UserInfo : SimpleUserInfo
    {
        public const int IdentityLen = 50;
        /// <summary>
        /// 默认密码
        /// </summary>
        public const string DefaultPassword = "12345678";

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public UserInfo()
        {
            this.PID = -1; //父ID 
            this.IsExpire = false; //是否过期
            this.CreateTime = System.DateTime.Now; //创建时间
            this.EditTime = System.DateTime.Now; //编辑时间
            this.Deleted = false; //是否已删除
            this.Status = "未关联";
            this.SubscribeWechat = "未关注";
        }   

        #region Property Members

        /// <summary>
        /// 父ID
        /// </summary>
        [DataMember]
        public virtual int PID { get; set; }

        /// <summary>
        /// 用户呢称
        /// </summary>
        [DataMember]
        public virtual string Nickname { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        [DataMember]
        public virtual bool IsExpire { get; set; }

        /// <summary>
        /// 过期日期
        /// </summary>
        [DataMember]
        public virtual DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 职务头衔
        /// </summary>
        [DataMember]
        public virtual string Title { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [DataMember]
        public virtual string IdentityCard { get; set; }

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
        /// 住址
        /// </summary>
        [DataMember]
        public virtual string Address { get; set; }

        /// <summary>
        /// 办公地址
        /// </summary>
        [DataMember]
        public virtual string WorkAddr { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public virtual string Gender { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [DataMember]
        public virtual DateTime Birthday { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        [DataMember]
        public virtual string QQ { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [DataMember]
        public virtual string Signature { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [DataMember]
        public virtual string AuditStatus { get; set; }

        /// <summary>
        /// 个人图片
        /// </summary>
        [DataMember]
        public virtual byte[] Portrait { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }

        /// <summary>
        /// 自定义字段
        /// </summary>
        [DataMember]
        public virtual string CustomField { get; set; }

        /// <summary>
        /// 默认部门名称
        /// </summary>
        [DataMember]
        public virtual string DeptName { get; set; }

        /// <summary>
        /// 所属机构名称
        /// </summary>
        [DataMember]
        public virtual string CompanyName { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [DataMember]
        public virtual string SortCode { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember]
        public virtual string Creator { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [DataMember]
        public virtual string Creator_ID { get; set; }

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
        /// 编辑人ID
        /// </summary>
        [DataMember]
        public virtual string Editor_ID { get; set; }

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
        /// 当前登录IP
        /// </summary>
        [DataMember]
        public virtual string CurrentLoginIP { get; set; }

        /// <summary>
        /// 当前登录时间
        /// </summary>
        [DataMember]
        public virtual DateTime CurrentLoginTime { get; set; }

        /// <summary>
        /// 当前Mac地址
        /// </summary>
        [DataMember]
        public virtual string CurrentMacAddress { get; set; }

        /// <summary>
        /// 微信绑定的OpenId
        /// </summary>
        [DataMember]
        public virtual string OpenId { get; set; }

        /// <summary>
        /// 微信多平台应用下的统一ID
        /// </summary>
        public virtual string UnionId { get; set; }

        /// <summary>
        /// 公众号状态
        /// </summary>
        [DataMember]
        public virtual string Status { get; set; }

        /// <summary>
        /// 公众号
        /// </summary>
        [DataMember]
        public virtual string SubscribeWechat { get; set; }

        /// <summary>
        /// 科室权限
        /// </summary>
        [DataMember]
        public virtual string DeptPermission { get; set; }

        /// <summary>
        /// 企业微信UserID
        /// </summary>
        [DataMember]
        public virtual string CorpUserId { get; set; }

        /// <summary>
        /// 企业微信状态
        /// </summary>
        [DataMember]
        public virtual string CorpStatus { get; set; }

        #endregion

    }

    /// <summary>
    /// 个人图片分类
    /// </summary>
    [Serializable]
    public enum UserImageType
    {
        个人肖像, 身份证照片1, 身份证照片2, 名片1, 名片2
    }
}