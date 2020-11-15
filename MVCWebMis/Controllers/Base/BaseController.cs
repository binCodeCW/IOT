using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Caching;
using System.Drawing.Imaging;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;
using YH.Security.Entity;
using YH.Security.BLL;
using IOT.MVCWebMis.Entity;
using WHC.Attachment.BLL;
using WHC.Attachment.Entity;
using ThoughtWorks.QRCode.Codec;

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// 所有需要进行登录控制的控制器基类
    /// </summary>
    public class BaseController : Controller
    {
        #region 属性变量

        /// <summary>
        /// 当前登录的用户属性
        /// </summary>
        public UserInfo CurrentUser
        {
            get
            {
                return Session["UserInfo"] as UserInfo;
            }
        }

        /// <summary>
        /// 定义常用功能的控制ID，方便基类控制器对用户权限的控制
        /// </summary>
        protected AuthorizeKey AuthorizeKey = new AuthorizeKey();

        #endregion

        #region 权限控制内容

        /// <summary>
        /// 获取用户的能使用的功能集合
        /// </summary>
        protected virtual Dictionary<string, string> Functions
        {
            get
            {
                Dictionary<string, string> functionDict = Session["Functions"] as Dictionary<string, string>;
                if (functionDict == null)
                {
                    functionDict = new Dictionary<string, string>();
                }
                return functionDict;
            }
        }

        /// <summary>
        /// 判断当前用户是否拥有某功能点的权限
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        public virtual bool HasFunction(string functionId)
        {
            return Permission.HasFunction(functionId);
        }

        /// <summary>
        /// 判断是否为系统管理员
        /// </summary>
        /// <returns>true:系统管理员,false:不是系统管理员</returns>
        public virtual bool IsAdmin()
        {
            return Permission.IsAdmin();
        }

        /// <summary>
        /// 用于检查方法执行前的权限，如果未授权，返回MyDenyAccessException异常
        /// </summary>
        /// <param name="functionId"></param>
        protected virtual void CheckAuthorized(string functionId)
        {
            if (!HasFunction(functionId))
            {
                string errorMessage = "您未被授权使用该功能，请重新登录测试或联系管理员进行处理。";
                throw new MyDenyAccessException(errorMessage);
            }
        }

        /// <summary>
        /// 对AuthorizeKey对象里面的操作权限进行赋值，用于页面判断
        /// </summary>
        protected virtual void ConvertAuthorizedInfo()
        {
            //判断用户权限
            AuthorizeKey.CanInsert = HasFunction(AuthorizeKey.InsertKey);
            AuthorizeKey.CanUpdate = HasFunction(AuthorizeKey.UpdateKey);
            AuthorizeKey.CanDelete = HasFunction(AuthorizeKey.DeleteKey);
            AuthorizeKey.CanView = HasFunction(AuthorizeKey.ViewKey);
            AuthorizeKey.CanList = HasFunction(AuthorizeKey.ListKey);
            AuthorizeKey.CanExport = HasFunction(AuthorizeKey.ExportKey);
        }

        #endregion

        #region 异常处理及记录

        /// <summary>
        /// 重写基类在Action执行之前的处理
        /// </summary>
        /// <param name="filterContext">重写方法的参数</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //获取用户登录的信息、如果用户为空跳转到登录界面
            if (CurrentUser == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
                //Response.Redirect("/Login/Index");//如果用户为空跳转到登录界面
                //Response.Flush();
            }
            else
            {
                //设置授权属性，然后赋值给ViewBag保存
                ConvertAuthorizedInfo();
                ViewBag.AuthorizeKey = AuthorizeKey;

                base.OnActionExecuting(filterContext);
            }
        }

        /// <summary>
        /// 重载视图展示界面，方便放置一些常规的ViewBag变量。
        /// 如果放在OnActionExecuting，则会导致任何请求都会执行一次，从而导致多次执行，降低响应效率
        /// </summary>
        protected override ViewResult View(string viewName, string masterName, object model)
        {
            //登录信息统一设置
            if (CurrentUser != null)
            {
                ViewBag.FullName = CurrentUser.FullName;
                ViewBag.Name = CurrentUser.Name;

                //ViewBag.MenuString = GetMenuString();
                ViewBag.MenuString = GetMenuStringCache(); //使用缓存，隔一段时间更新
            }

            return base.View(viewName, masterName, model);
        }

        /// <summary>
        /// 覆盖基类控制器的异常处理
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is MyDenyAccessException)
            {
                base.OnException(filterContext);

                //自定义非授权的异常处理，可记录用户操作

                // 当自定义显示错误 mode = On，显示友好错误页面
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    filterContext.ExceptionHandled = true;
                    this.View("Error").ExecuteResult(this.ControllerContext);
                    Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
            else
            {
                base.OnException(filterContext);
                LogTextHelper.Error(filterContext.Exception);//错误记录

                // 当自定义显示错误 mode = On，显示友好错误页面
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    filterContext.ExceptionHandled = true;
                    this.View("Error").ExecuteResult(this.ControllerContext);
                    //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
        }
        #endregion

        #region 菜单管理

        /// <summary>
        /// 使用分布式缓存实现菜单数据的缓存处理
        /// </summary>
        /// <returns></returns>
        public string GetMenuStringCache()
        {
            string itemValue = MemoryCacheHelper.GetCacheItem<string>("GetMenuStringCache_" + CurrentUser.ID, delegate ()
               {
                   return GetMenuString();
               },
                null, DateTime.Now.AddMinutes(5) //5分钟以后过期，重新获取
            );
            return itemValue;
        }

        #region 定义的格式模板
        // javascript:;
        // {0}?tid={1}
        // nav-toggle 菜单可折叠，arrow则是出现三角箭头符号

        //包含有子节点模板
        string parentTemplate = @"
            <li class='nav-item ' id='{3}'>
                <a href='{0}' class='nav-link nav-toggle' style='{4}'>
                    <i class='{1}'></i>
                    <span class='title'>{2}</span>
                    <span class='arrow'></span>
                    <span class='selected'></span>
                </a>";

        //无子节点的模板
        string itemTemplate = @"
            <li class='nav-item ' id='{3}'>
                <a href='{0}' class='nav-link'>
                    <i class='{1}'></i>
                    <span class='title'>{2}</span>
                    <span class='selected'></span>
                </a></li>";

        string parentTemplateEnd = "</li>";
        string subItemsTemplateStart = "<ul class='sub-menu'>";
        string subItemsTemplateEnd = "</ul>";
        #endregion

        /// <summary>
        /// 获取常规菜单，Layout和Layout2两种
        /// </summary>
        /// <returns></returns>
        public string GetMenuString()
        {
            #region 菜单格式代码
            /*
            <li class="nav-item active open" id="2">
                <a href="javascript:;" class="nav-link nav-toggle">
                    <i class="icon-basket"></i>
                    <span class="title">行业动态</span>
                    <span class="arrow open"></span>
                </a>
                <ul class="sub-menu">
                    <li class="nav-item  active open" id="21">
                        <a href="javascript:;" class="nav-link nav-toggle">
                            <i class="icon-home"></i>
                            <span class="title">行业动态</span>
                            <span class="arrow"></span>
                            <span class="selected"></span>
                        </a>
                        <ul class="sub-menu">
                            <li class="nav-item  active open" id="211">
                                <a href="second?tid=2&sid=21&ssid=211">
                                    <i class="icon-home"></i>
                                    <span class="badge badge-danger">4</span>
                                    <span class="title">政策法规</span>
                                </a>
                            </li>
                            <li class="nav-item " id="212">
                                <a href="second?tid=2&sid=21&ssid=212">
                                    <i class="icon-basket"></i>
                                    <span class="badge badge-warning">4</span>
                                    <span class="title">通知公告</span>
                                </a>
                            </li>
                        </ul>
                    </li>
                </ul>
            </li>
             */

            #endregion

            bool isSuperAdmin = BLLFactory<User>.Instance.UserInRole(CurrentUser.Name, RoleInfo.SuperAdminName);
            StringBuilder sb = new StringBuilder();
            List<MenuNodeInfo> list = null;
            if (isSuperAdmin)
            {
                //管理员可以显示指定系统的所有菜单
                list = BLLFactory<Menu>.Instance.GetTree(ConfigData.SystemType);
            }
            else
            {
                list = BLLFactory<Menu>.Instance.GetMenuNodesByUser(CurrentUser.ID, ConfigData.SystemType);
            }

            //水平菜单样式例外
            if (ConfigData.Layout == LayoutType.Layout3)
            {
                var result = GenerateHorizontaMenu(list, "");
                sb.AppendFormat(result);
            }
            else
            {
                //其他Layout/Layout2两种菜单模型一致
                var result = GenerateMenu(list, "");
                sb.AppendFormat(result);
            }

            return sb.ToString();
        }

        private string GenerateMenu(List<MenuNodeInfo> list, string parentId)
        {
            StringBuilder sb = new StringBuilder();
            foreach (MenuNodeInfo nodeInfo in list)
            {
                if (!HasFunction(nodeInfo.FunctionId))
                {
                    continue;
                }
                //如果标签不对，也不显示
                //if (!string.IsNullOrEmpty(nodeInfo.Tag) && nodeInfo.Tag != tag)
                //{
                //    continue;
                //}

                bool hasChildren = nodeInfo.Children != null && nodeInfo.Children.Count > 0; //是否有子菜单
                bool urlEmpty = string.IsNullOrEmpty(nodeInfo.Url) || (nodeInfo.Url == "#");//如果URL为空也不行
                bool isJsFunction = !string.IsNullOrEmpty(nodeInfo.Url) && nodeInfo.Url.EndsWith("()");

                var tmpUrl = string.Format("{0}{1}tid={2}", nodeInfo.Url, GetUrlJoiner(nodeInfo.Url), nodeInfo.ID);
                //如果有子菜单或者连接为空，那么不设置连接；如果为JS函数，使用函数即可
                var url = (hasChildren || urlEmpty) ? "javascript:;" :
                    (isJsFunction ? "javascript:" + nodeInfo.Url : tmpUrl);


                var style = hasChildren && !string.IsNullOrEmpty(parentId) ? "font-size:14px;color:yellow" : "";
                sb = sb.AppendFormat(hasChildren ? parentTemplate : itemTemplate, url, nodeInfo.WebIcon, nodeInfo.Name, nodeInfo.ID, style);

                if (hasChildren)
                {
                    sb = sb.Append(subItemsTemplateStart);
                }

                //继续添加其他部分
                var result = GenerateMenu(nodeInfo.Children, nodeInfo.ID);
                sb.AppendFormat(result);

                if (hasChildren)
                {
                    sb = sb.Append(parentTemplateEnd);
                }
                if (hasChildren) { sb = sb.Append(subItemsTemplateEnd); }//结束
            }
            return sb.ToString();
        }


        #region 定义的格式模板

        //第一级的菜单LI标识
        string firstLiMenuStart = "<li class='menu-dropdown classic-menu-dropdown '>";
        string subLiMenuStart = "<li class='dropdown-submenu '>";
        string liMenuEnd = "</li>";
        string ulMenuStart = "<ul class='dropdown-menu pull-left'>";
        string ulMenuEnd = "</ul>";
        //包含有子节点模板
        string parentMenuTemplate = @"
                <a href='{0}' class='nav-link nav-toggle' id='{3}'>
                    <i class='{1}'></i>
                    <span class='title'>{2}</span>
                    <span class='arrow'></span>
                    <span class='selected'></span>
                </a>";
        //无子节点的模板
        string itemMenuTemplate = @"
            <li class=' ' id='{3}'>
                <a href='{0}' class='nav-link'>
                    <i class='{1}'></i>
                    <span class='title'>{2}</span>
                    <span class='selected'></span>
                </a></li>";
        #endregion
        /// <summary>
        /// 获取水平的菜单，Layout3样式的菜单
        /// </summary>
        /// <returns></returns>
        private string GenerateHorizontaMenu(List<MenuNodeInfo> list, string parentId)
        {
            StringBuilder sb = new StringBuilder();
            foreach (MenuNodeInfo nodeInfo in list)
            {
                #region 权限控制
                if (!HasFunction(nodeInfo.FunctionId))
                {
                    continue;
                }
                //如果标签不对，也不显示
                //if (!string.IsNullOrEmpty(nodeInfo.Tag) && nodeInfo.Tag != tag)
                //{
                //    continue;
                //} 
                #endregion

                bool hasChildren = nodeInfo.Children != null && nodeInfo.Children.Count > 0; //是否有子菜单
                bool urlEmpty = string.IsNullOrEmpty(nodeInfo.Url) || (nodeInfo.Url == "#");//如果URL为空也不行
                bool isJsFunction = !string.IsNullOrEmpty(nodeInfo.Url) && nodeInfo.Url.EndsWith("()");

                var tmpUrl = string.Format("{0}{1}tid={2}", nodeInfo.Url, GetUrlJoiner(nodeInfo.Url), nodeInfo.ID);//如果有子菜单或者连接为空，那么不设置连接；如果为JS函数，使用函数即可
                var url = (hasChildren || urlEmpty) ? "javascript:;" :
                    (isJsFunction ? "javascript:" + nodeInfo.Url : tmpUrl);

                //开始
                if (string.IsNullOrEmpty(parentId))
                {
                    sb.AppendFormat(firstLiMenuStart);
                }
                else if (hasChildren)
                {
                    sb.Append(subLiMenuStart);
                }

                sb.AppendFormat(hasChildren ? parentMenuTemplate : itemMenuTemplate, url, nodeInfo.WebIcon, nodeInfo.Name, nodeInfo.ID);

                //子菜单开始
                if (hasChildren)
                {
                    sb.Append(ulMenuStart);
                }

                //继续添加其他部分
                var result = GenerateHorizontaMenu(nodeInfo.Children, nodeInfo.ID);
                sb.AppendFormat(result);

                //子菜单结束
                if (hasChildren)
                {
                    sb.Append(ulMenuEnd);
                }

                //结束
                if (string.IsNullOrEmpty(parentId))
                {
                    sb.Append(liMenuEnd);
                }
                else if (hasChildren)
                {
                    sb.Append(liMenuEnd);
                }
            }
            return sb.ToString();
        }

        #endregion

        #region 辅助函数

        /// <summary>
        /// 获取当前登录用户的LoginUserInfo对象
        /// </summary>
        /// <returns></returns>
        public LoginUserInfo GetLoginUser()
        {
            var info = ConvertToLoginUser(this.CurrentUser);
            return info;
        }

        /// <summary>
        /// 转换框架通用的用户基础信息，方便框架使用
        /// </summary>
        /// <param name="info">权限系统定义的用户信息</param>
        /// <returns></returns>
        public LoginUserInfo ConvertToLoginUser(UserInfo info)
        {
            LoginUserInfo loginInfo = new LoginUserInfo();
            loginInfo.ID = info.ID.ToString();
            loginInfo.Name = info.Name;
            loginInfo.FullName = info.FullName;
            loginInfo.IdentityCard = info.IdentityCard;
            loginInfo.MobilePhone = info.MobilePhone;
            loginInfo.QQ = info.QQ;
            loginInfo.Email = info.Email;
            loginInfo.DeptId = info.Dept_ID;
            loginInfo.Gender = info.Gender;
            loginInfo.CompanyId = info.Company_ID;
            return loginInfo;
        }

        /// <summary>
        /// 获取URL的连接字符串，如果有?参数那么连接符为&，否则为?
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetUrlJoiner(string url)
        {
            return url.Contains("?") ? "&" : "?";
        }

        /// <summary>
        /// 生成GUID的服务器方法
        /// </summary>
        /// <returns></returns>
        public ActionResult NewGuid()
        {
            string guid = System.Guid.NewGuid().ToString();
            return Content(guid);
        }

        /// <summary>
        /// 把object对象转换为ContentResult
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected ContentResult ToJsonContent(object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            string result = JsonConvert.SerializeObject(obj, Formatting.Indented, timeConverter);
            return Content(result);
        }

        public ContentResult Content(bool result)
        {
            return Content(result.ToString().ToLower());//小写方便脚本处理
        }

        public ContentResult Content(int result)
        {
            return Content(result.ToString());
        }

        /// <summary>
        /// 把对象为json字符串
        /// </summary>
        /// <param name="obj">待序列号对象</param>
        /// <returns></returns>
        protected string ToJson(object obj)
        {
            //使用Json.NET的序列号类，能够更加高效、完美
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        protected void ClearCache()
        {
            List<string> cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                MemoryCache.Default.Remove(cacheKey);
            }
        }

        /// <summary>
        /// 调用AsposeCell控件，生成Excel文件
        /// </summary>
        /// <param name="datatable">生成的表格数据</param>
        /// <param name="relatedPath">服务器相对路径</param>
        /// <returns></returns>
        protected virtual bool GenerateExcel(DataTable datatable, string relatedPath)
        {
            #region 把DataTable转换为Excel并输出
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
            //为单元格添加样式    
            Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
            //设置居中
            style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
            //设置背景颜色
            style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);
            style.Pattern = Aspose.Cells.BackgroundType.Solid;
            style.Font.IsBold = true;

            int rowIndex = 0;
            for (int i = 0; i < datatable.Columns.Count; i++)
            {
                DataColumn col = datatable.Columns[i];
                string columnName = col.Caption ?? col.ColumnName;
                workbook.Worksheets[0].Cells[rowIndex, i].PutValue(columnName);
                workbook.Worksheets[0].Cells[rowIndex, i].SetStyle(style);
            }
            rowIndex++;

            foreach (DataRow row in datatable.Rows)
            {
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    workbook.Worksheets[0].Cells[rowIndex, i].PutValue(row[i].ToString());
                }
                rowIndex++;
            }

            for (int k = 0; k < datatable.Columns.Count; k++)
            {
                workbook.Worksheets[0].AutoFitColumn(k, 0, 150);
            }
            workbook.Worksheets[0].FreezePanes(1, 0, 1, datatable.Columns.Count);

            //根据用户创建目录，确保生成的文件不会产生冲突
            string realPath = Server.MapPath(relatedPath);
            string parentPath = Directory.GetParent(realPath).FullName;
            DirectoryUtil.AssertDirExist(parentPath);

            workbook.Save(realPath, Aspose.Cells.SaveFormat.Excel97To2003);

            #endregion

            return true;
        }


        /// <summary>
        /// 读取文件的字节
        /// </summary>
        /// <param name="fileData">附件信息</param>
        /// <returns></returns>
        protected virtual byte[] ReadFileBytes(HttpPostedFileBase fileData)
        {
            byte[] data;
            using (Stream inputStream = fileData.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            return data;
        }

        /// <summary>
        /// 附件管理，根据附件ID获取对应的文件地址列表
        /// </summary>
        /// <param name="attachGuid">附件ID</param>
        /// <returns></returns>
        public virtual ActionResult GetImageList(string attachGuid)
        {
            var list = GetImageListInternal(attachGuid);
            return Content(list.ToJson());
        }

        /// <summary>
        /// 内部获取图片列表
        /// </summary>
        /// <param name="attachGuid">附件ID</param>
        /// <returns></returns>
        public List<string> GetImageListInternal(string attachGuid)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(attachGuid))
            {
                var fileList = BLLFactory<FileUpload>.Instance.GetByAttachGUID(attachGuid);
                int port = HttpContext.Request.Url.Port;
                string portString = (port == 80) ? "" : ":" + port;
                string basePath = "http://" + HttpContext.Request.Url.Host + portString;//根网站地址 或者 ConfigData.WebsiteDomain
                foreach (FileUploadInfo info in fileList)
                {
                    string absoluteUrl = Path.Combine(basePath, info.BasePath, info.SavePath);
                    absoluteUrl = absoluteUrl.Replace("\\", "/");
                    list.Add(absoluteUrl);
                }
            }
            return list;
        }

        /// <summary>
        /// 转换二维码连接为图片格式
        /// </summary>
        /// <param name="url">二维码连接</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QR(string url)
        {
            //初始化二维码生成工具
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;

            //将字符串生成二维码图片
            var image = qrCodeEncoder.Encode(url, Encoding.Default);
            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            image.Dispose();

            return File(ms.ToArray(), "image/Png");
        }

        #endregion
    }
}
