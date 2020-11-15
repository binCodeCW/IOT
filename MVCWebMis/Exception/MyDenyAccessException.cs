using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// 自定义拒绝访问异常
    /// </summary>
    public class MyDenyAccessException : MyApiException
    {
        public MyDenyAccessException(string message) : base(message) 
        {
        }

        public MyDenyAccessException(string message, int errorcode) : base(message, errorcode)
        {
        }
    }
}
