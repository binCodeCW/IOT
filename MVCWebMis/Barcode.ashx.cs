using System;
using System.Web;
using grsvr6Lib;

namespace IOT.MVCWebMis
{
    public class Barcode : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string text = ServerTools.ReportUtility.GenerateBarcodeGraph(context.Request.QueryString["params"]);

            context.Response.Write(text);
        }

        public bool IsReusable
        {
            get
            {
                return true; //false;
            }
        }
    }

    public class ServerTools
    {
        //定义一个全局可用的报表工具类接口变量
        //接口可用的方法可以在帮助文档中查询 Utility 接口
        public static GridppReportUtility ReportUtility = new GridppReportUtility();
    }
}
