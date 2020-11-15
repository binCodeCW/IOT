using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// 自定义API异常
    /// </summary>
    public class MyApiException : System.Exception
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 默认构造函数 
        /// </summary>
        /// <param name="message">错误消息</param>
        public MyApiException(string message) : base(message) 
        {
        }

        /// <summary>
        /// 参数化构造函数，提供错误码
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="errcode">错误码</param>
        public MyApiException(string message, int errcode) : base(message)
        {
            this.errcode = errcode;
        }

    }
}
