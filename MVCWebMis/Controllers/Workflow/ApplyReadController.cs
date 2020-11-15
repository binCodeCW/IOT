using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.WorkflowLite.BLL;
using WHC.WorkflowLite.Entity;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;

namespace IOT.MVCWebMis.Controllers
{
    public class ApplyReadController : BusinessController<ApplyRead, ApplyReadInfo>
    {
        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(ApplyReadInfo info)
        {
            //留给子类对参数对象进行修改
        }

        protected override void OnBeforeUpdate(ApplyReadInfo info)
        {
            //留给子类对参数对象进行修改
        }
        #endregion
        
        #region 示例代码

		/*
        /// <summary>
        /// 获取对应记录的名称
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="token">访问令牌</param>
        /// <returns></returns>
        [HttpGet]
        public string GetName(string id)
        {
            //令牌检查,不通过则抛出异常
            
            return BLLFactory<ApplyRead>.Instance.GetName(id);
        }    
		  
        /// <summary>
        /// 给指定角色添加用户
        /// </summary>
        /// <param name="param">包含多个属性的对象</param>
        /// <param name="token">访问令牌</param>
        [HttpPost]
        public CommonResult AddUser(JObject param)
        {
            //令牌检查,不通过则抛出异常
            
            dynamic obj = param;
            if (obj != null)
            {
                string userID = obj.userID;
                string roleID = obj.roleID;

                bool result = BLLFactory<Role>.Instance.AddUser(userID, roleID);
                return new CommonResult(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 验证用户身份有效性
        /// </summary>
        /// <param name="param">包含多个属性的对象</param>
        /// <returns></returns>
        [HttpPost]
        public CommonResult VerifyUser(JObject param, string signature, string timestamp, string nonce, string appid)
        {
            //如果用户签名检查不通过，则抛出MyApiException异常。
            base.CheckSignature(signature, timestamp, nonce, appid);

            dynamic obj = param;
            if (obj != null)
            {
                string account = obj.account;
                string password = obj.password;
                string corpAccount = obj.corpAccount;
                string ip = obj.ip; 
                string macAddr = obj.macAddr;

                return BLLFactory<User>.Instance.VerifyUser(account, password, corpAccount, ip, macAddr);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        /// <summary>
        /// 根据条件查询数据库,并返回对象集合(用于分页数据显示)
        /// </summary>
        /// <returns>指定对象的集合</returns>
        [HttpPost]
        public virtual PagedList<ApplyReadInfo> FindWithPager(string condition, PagerInfo pagerInfo)
        {
            //令牌检查,不通过则抛出异常
            
            List<ApplyReadInfo> list = BLLFactory<ApplyRead>.Instance.FindWithPager(condition, pagerInfo);

            //构造成Json的格式传递
            var result = new PagedList<ApplyReadInfo>() { total_count = pagerInfo.RecordCount, list = list };
            return result;
        }
		*/
        #endregion

        [HttpPost]
        public ActionResult AddReadRecord(JObject param)
        {
            //令牌检查,不通过则抛出异常
            
            dynamic obj = param;
            if (obj != null)
            {
                string applyId = obj.applyId;
                string userIdList = obj.userIdList;

                bool info = BLLFactory<ApplyRead>.Instance.AddReadRecord(applyId, userIdList);
                var result = new CommonResult(info);
                return ToJsonContent(result);

            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        [HttpPost]
        public ActionResult UpdateReadInfo(JObject param)
        {
            //令牌检查,不通过则抛出异常
            
            dynamic obj = param;
            if (obj != null)
            {
                string applyId = obj.applyId;
                int userId = obj.userId;
                string content = obj.content;

                bool info = BLLFactory<ApplyRead>.Instance.UpdateReadInfo(applyId, userId, content);
                var result = new CommonResult(info);
                return ToJsonContent(result);
            }
            else
            {
                throw new MyApiException("传递参数错误");
            }
        }

        [HttpGet]
        public ActionResult IsReadStatus(string applyId, int userId)
        {
            //令牌检查,不通过则抛出异常
            
            var success = BLLFactory<ApplyRead>.Instance.IsReadStatus(applyId, userId);
            var result = new CommonResult(success);
            return ToJsonContent(result);
        }

        [HttpGet]
        public ActionResult FindByApplyId(string applyId)
        {
            //令牌检查,不通过则抛出异常
            
            var result = BLLFactory<ApplyRead>.Instance.FindByApplyId(applyId);
            return ToJsonContent(result);
        }
    }
}
        