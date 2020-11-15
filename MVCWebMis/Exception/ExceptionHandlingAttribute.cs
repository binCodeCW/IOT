using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using YH.Framework.ControlUtil.Facade;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// API自定义错误过滤器属性
    /// </summary>
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 统一对调用异常信息进行处理，返回自定义的异常信息
        /// </summary>
        /// <param name="context">HTTP上下文对象</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            //自定义异常的处理
            MyApiException ex = context.Exception as MyApiException;
            if (ex != null)
            {                
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    //封装处理异常信息，返回指定JSON对象
                    Content = new StringContent(new BaseResultJson(ex.Message, false, ex.errcode).ToJson()),
                    ReasonPhrase = "Exception"
                });
            }

            //记录关键的异常信息
            Debug.WriteLine(context.Exception);
            
            //常规异常的处理
            string msg = string.IsNullOrEmpty(context.Exception.Message) ? "接口出现了错误，请重试或者联系管理员" : context.Exception.Message;
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(msg),
                ReasonPhrase = "Critical Exception"
            });
        }
    }
}