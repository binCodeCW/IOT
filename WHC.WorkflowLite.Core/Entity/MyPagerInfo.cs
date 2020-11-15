using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace WHC.WorkflowLite.Entity
{
    /// <summary>
    /// 分页信息对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class MyPagerInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MyPagerInfo()
        {
            this.CurrentPageIndex = 1;//当前页码
            this.PageSize = 20;//每页显示的记录
            this.RecordCount = 0;//记录总数
        }

        #region 属性变量

        /// <summary>
        /// 获取或设置当前页码
        /// </summary>
        [XmlElement(ElementName = "CurrentPageIndex")]
        [DataMember]
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// 获取或设置每页显示的记录
        /// </summary>
        [XmlElement(ElementName = "PageSize")]
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// 获取或设置记录总数
        /// </summary>
        [XmlElement(ElementName = "RecordCount")]
        [DataMember]
        public int RecordCount { get; set; }

        /// <summary>
        /// 共有多少页
        /// </summary>
        [XmlElement(ElementName = "PageCount")]
        [DataMember]
        public int PageCount
        {
            get
            {
                if (RecordCount == 0 || PageSize == 0)
                {
                    return 0;
                }
                else
                {
                    int fullPage = RecordCount / PageSize;
                    int left = RecordCount % PageSize;
                    if (left > 0)
                    {
                        fullPage += 1;
                    }
                    return fullPage;
                }
            }
        }

        #endregion
    }
}
