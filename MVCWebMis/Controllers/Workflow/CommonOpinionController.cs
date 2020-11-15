using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace IOT.MVCWebMis.Controllers
{
    public class CommonOpinionController : BusinessController<CommonOpinion, CommonOpinionInfo>
    {
        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(CommonOpinionInfo info)
        {
            //留给子类对参数对象进行修改
        }

        protected override void OnBeforeUpdate(CommonOpinionInfo info)
        {
            //留给子类对参数对象进行修改
        }
        #endregion
        
        [HttpGet]
        public ActionResult GetDictJson(string formId)
        {
            var result = new List<CListItem>();
            var list = BLLFactory<CommonOpinion>.Instance.FindByUser(CurrentUser.ID, formId);
            foreach (var info in list)
            {
                result.Add(new CListItem(info.Opinion, info.Opinion));
            }
            return ToJsonContent(result);
        }

        [HttpGet]        
        public ActionResult FindByUser(int userId)
        {
            //令牌检查,不通过则抛出异常
            
            var result = BLLFactory<CommonOpinion>.Instance.FindByUser(userId);
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult FindByUser2(int userId, string formId)
        {
            //令牌检查,不通过则抛出异常

            var result = BLLFactory<CommonOpinion>.Instance.FindByUser(userId, formId);
            return ToJsonContent(result);

        }   
    
        [HttpPost]
        public ActionResult AddOpinion(JObject param)
        {
            //令牌检查,不通过则抛出异常
            
            dynamic obj = param;
            if (obj != null)
            {
                string opinion = obj.opinion;
                int userId = obj.userId;

                bool success = BLLFactory<CommonOpinion>.Instance.AddOpinion(opinion, userId);
                var result = new CommonResult(success);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

   
        [HttpPost]
        public ActionResult AddOpinion2(JObject param)
        {
            //令牌检查,不通过则抛出异常
            
            dynamic obj = param;
            if (obj != null)
            {
                string opinion = obj.opinion;
                int userId = obj.userId;
                string formId = obj.formId;

                bool success = BLLFactory<CommonOpinion>.Instance.AddOpinion(opinion, userId, formId);
                var result = new CommonResult(success);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

    }
}
        