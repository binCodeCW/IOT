using System.Web.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using YH.Pager.Entity;
using IOT.MVCWebMis.BLL;
using IOT.MVCWebMis.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class YH08_QualityAnalysisController : BaseController
    {
        public ActionResult Index()
        {
            return View();
            //return View("index2");
        }
    }
}
