using System.Web.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using YH.Pager.Entity;
using IOT.MVCWebMis.BLL;
using IOT.MVCWebMis.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class YH04_QualityAnalysisController : BaseController
    {

        public ActionResult Index()
        {
            return View();
            //return View("index2");
        }
    }
}
