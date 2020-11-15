using IOT.MVCWebMis.Entity;
using IOT.MVCWebMis.IDAL;

using YH.Framework.ControlUtil;
using YH.Security.BLL;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// 为测试用的数据表
    /// </summary>
	public class TestUser : BaseBLL<TestUserInfo>
    {
        public TestUser() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(OperationLog.OnOperationLog);
        }


        /// <summary>
        /// 根据个人图片枚举类型获取图片数据
        /// </summary>
        /// <param name="imagetype">图片枚举类型</param>
        /// <returns></returns>
        public byte[] GetPersonImageBytes(string id, string imagetype = "个人肖像")
        {
            ITestUser dal = baseDal as ITestUser;
            return dal.GetPersonImageBytes(id, imagetype);
        }

        /// <summary>
        /// 更新个人相关图片数据
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="imageBytes">图片字节数组</param>
        /// <param name="imagetype">图片类型</param>
        /// <returns></returns>
        public bool UpdatePersonImageBytes(string id, byte[] imageBytes, string imagetype = "个人肖像")
        {
            ITestUser dal = baseDal as ITestUser;
            return dal.UpdatePersonImageBytes(id, imageBytes, imagetype);
        }
    }
}
