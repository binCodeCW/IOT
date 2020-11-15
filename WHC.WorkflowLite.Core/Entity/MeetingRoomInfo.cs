using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 会议室预定申请单
    /// </summary>
    [DataContract]
    public class MeetingRoomInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public MeetingRoomInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.MeetingPersonNumber = 0;
            this.AttachGUID = System.Guid.NewGuid().ToString();
            this.Apply_ID = System.Guid.NewGuid().ToString(); //申请单编号(业务对象负责生成）

        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 会议室名称
        /// </summary>
        [DataMember]
        public virtual string RoomName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public virtual DateTime EndTime { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        [DataMember]
        public virtual string Duration { get; set; }

        /// <summary>
        /// 参会人数
        /// </summary>
        [DataMember]
        public virtual int MeetingPersonNumber { get; set; }

        /// <summary>
        /// 参会人
        /// </summary>
        [DataMember]
        public virtual string MeetingPerson { get; set; }

        /// <summary>
        /// 申请单编号
        /// </summary>
        [DataMember]
        public virtual string Apply_ID { get; set; }

        /// <summary>
        /// 申请单日期
        /// </summary>
        [DataMember]
        public virtual DateTime ApplyDate { get; set; }

        /// <summary>
        /// 申请部门
        /// </summary>
        [DataMember]
        public virtual string ApplyDept { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [DataMember]
        public virtual string Note { get; set; }

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


        #endregion

    }
}