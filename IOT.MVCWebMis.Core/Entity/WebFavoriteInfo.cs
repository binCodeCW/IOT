using System;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;

namespace IOT.MVCWebMis.Entity
{
    /// <summary>
    /// 网页收藏夹信息
    /// </summary>
    [DataContract]
    public class WebFavoriteInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public WebFavoriteInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.Seq = DateTime.Now.DateTimeToInt();
            this.CreateTime = DateTime.Now;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public virtual string Title { get; set; }

        /// <summary>
        /// URL地址
        /// </summary>
        [DataMember]
        public virtual string Url { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public virtual decimal Seq { get; set; }

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


        #endregion

    }
}