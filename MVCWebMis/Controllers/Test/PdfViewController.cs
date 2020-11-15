using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Spire.Pdf;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;

using WHC.Attachment.BLL;
using WHC.Attachment.Entity;
using IOT.MVCWebMis.Common;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// 查看PDF的控制器
    /// </summary>
    public class PdfViewController : Controller
    {
        // GET: PdfView
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index2()
        {
            var fileName = Request.QueryString["filename"];
            //string condition = string.Format("FileName='{0}'", name);
            string condition = string.Format("1=1");//测试获取一个
            var info = BLLFactory<FileUpload>.Instance.FindSingle(condition);

            string path = "";
            if (info != null)
            {
                //实际路径
                //path = Path.Combine(info.BasePath, info.SavePath);

                //测试路径
                path = "/UploadFiles/fapiao2.pdf";//fapiao3.pdf
            }

            return new FilePathResult(path, "application/pdf");
        }

        public ActionResult Second()
        {
            return View("Second");
        }

        public ActionResult FindWithPager()
        {
            List<FileUploadInfo> list = null;
            var category = Request["BillNo"] ?? "";
            var FID = Request["FID"] ?? "";
            category = string.IsNullOrWhiteSpace(category) ? "政策法规" : category;

            if (!string.IsNullOrWhiteSpace(category))
            {
                string where = string.Format("Category='{0}'", category);
                list = BLLFactory<FileUpload>.Instance.Find(where);
            }
            else if(!string.IsNullOrWhiteSpace(FID))
            {
                string where = string.Format("FID='{0}'", FID);
                list = BLLFactory<FileUpload>.Instance.Find(where);
            }

            if (list == null) return null;

            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (var info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.Category = info.Category;
                obj.FileName = info.FileName;
                obj.SavePath = info.SavePath;
                obj.BasePath = info.BasePath;
                obj.AddTime = info.AddTime;
                obj.FileSize = info.FileSize;
                //参考转义代码
                //  obj.ProvinceName = BLLFactory<Province>.Instance.GetNameByID(info.ProvinceID);

                objList.Add(obj);
            }

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = objList.Count, rows = objList }; //如果使用转义，请使用objList对象
            return ToJsonContent(result);
        }

        public ActionResult Download(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                var info = BLLFactory<FileUpload>.Instance.FindByID(id);
                string path = "";
                if (info != null)
                {
                    //实际路径
                    //path = Path.Combine(info.BasePath, info.SavePath);

                    //测试路径
                    path = "/UploadFiles/fapiao2.pdf";//fapiao3.pdf
                }
                return new FilePathResult(path, "application/pdf");
            }
            else
            {
                return null;
            }
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

        /// <summary>
        /// 根据文件名，获取对应的PDF文件路径（物理路径）
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual ActionResult FindPdfByFileName(string name)
        {
            //获取指定的记录
            //string condition = string.Format("FileName='{0}'", name);
            string condition = string.Format("1=1");//测试获取一个
            var info = BLLFactory<FileUpload>.Instance.FindSingle(condition);
            if (info != null)
            {
                //实际路径
                //var path = Path.Combine(info.BasePath, info.SavePath);

                //测试路径
                var path = "/UploadFiles/fapiao2.pdf";//fapiao3.pdf

                return Content(path);
            }
            else
            {
                return Content("");
            }
        }

        public virtual ActionResult FindByFileName(string name)
        {
            //获取指定的记录
            //string condition = string.Format("FileName='{0}'", name);
            string condition = string.Format("1=1");//测试获取一个
            var info = BLLFactory<FileUpload>.Instance.FindSingle(condition);
            if (info != null)
            {
                //判断是否存在PDF生成的图片文件，
                //生成的jpg文件名为附件的ID
                string pdfjpgPath = string.Format("/GenerateFiles/pdf/{0}.jpg", info.ID);
                string pdfjpg = Server.MapPath(pdfjpgPath);

                //PDF文件路径，相对目录即可
                string pdfPath = @"/Content/Template/fapiao.pdf";
                string pdfRealPath = Server.MapPath(pdfPath);

                //如果不存在，则生成，否则返回已生成的文件
                if(!FileUtil.IsExistFile(pdfjpg))
                {                    
                    //破解
                    ModifyInMemory_Spire.ActivateMemoryPatching();
                    PdfDocument doc = new PdfDocument(pdfRealPath);
                    var image = doc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Bitmap, 300, 300);
                    FileUtil.BytesToFile(ImageHelper.ImageToBytes(image), pdfjpg);
                }
                //存储一个路径
                info.SavePath = pdfjpgPath;//修改使用这个属性返回使用

                //序列号返回对象信息
                string result = JsonConvert.SerializeObject(info, Formatting.Indented);
                return Content(result);
            }
            else
            {
                return Content("");
            }
        }

        public virtual ActionResult UpdateInfo()
        {
            string id = Request["id"];
            string email = Request["email"];
            string mobile = Request["mobile"];

            CommonResult info = new CommonResult();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(email))
            {
                Hashtable ht = new Hashtable();
                ht.Add("Owner_ID", email);
                //其他修改
                var flag = BLLFactory<FileUpload>.Instance.Update(id, ht);
                info.Success = flag;
            }

            string result = JsonConvert.SerializeObject(info, Formatting.Indented);
            return Content(result);
        }
    }
}