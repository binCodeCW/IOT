using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// 排序信息
    /// </summary>
    public class SortInfo
    {
        public SortInfo() { }

        /// <summary>
        /// 参数化构造函数
        /// </summary>
        /// <param name="name">排序字段</param>
        /// <param name="sort">排序方向： asc或者desc</param>
        public SortInfo(string name, string sort)
        {
            this.SortName = name;
            this.IsDesc = ("desc".Equals(sort, StringComparison.OrdinalIgnoreCase)) ;
        }

        /// <summary>
        /// 排序字段名称
        /// </summary>
        public string SortName { get; set; }

        /// <summary>
        /// 排序方向,是否降序
        /// </summary>
        public bool IsDesc { get; set; }
    }
}