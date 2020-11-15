using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using YH.Framework.ControlUtil;
using YH.Framework.ControlUtil.Facade;

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// API自定义错误消息处理委托类。
    /// 用于处理访问不到对应API地址的情况，对错误进行自定义操作。
    /// </summary>
    public class CustomErrorMessageDelegatingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>((responseToCompleteTask) =>
            {
                HttpResponseMessage response = responseToCompleteTask.Result;
                HttpError error = null;
                if (response.TryGetContentValue<HttpError>(out error))
                {
                    //添加自定义错误处理
                    //error.Message = "Your Customized Error Message";
                }

                if (error != null)
                {
                    //获取抛出自定义异常，有拦截器统一解析
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        //封装处理异常信息，返回指定JSON对象
                        Content = new StringContent(new BaseResultJson(error.Message, false, 404).ToJson()),
                        ReasonPhrase = "Exception"
                    });
                }
                else
                {
                    return response;
                }
            });
        }
    }
}