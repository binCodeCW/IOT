using System.Web.Mvc;

namespace IOT.MVCWebMis.Common
{
    public static class HtmlHelpers
    {
        public static bool HasFunction(this HtmlHelper helper, string functionId)
        {
            return Permission.HasFunction(functionId);
        }

        public static bool IsAdmin()
        {
            return Permission.IsAdmin();
        }


        /// <summary>
        /// 客户端提交内容需要解析，把Name|ID 转换为ID
        /// </summary>
        /// <param name="nameId">Name|ID字符串</param>
        /// <returns></returns>
        public static string GetSplitId(string nameId)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(nameId) && nameId.Contains("|"))
            {
                int index = nameId.LastIndexOf("|");
                result = nameId.Substring(index + 1);//获取ID
            }
            return result;
        }
        /// <summary>
        /// 客户端提交内容需要解析，把Name|ID 转换为ID
        /// </summary>
        /// <param name="nameId">Name|ID字符串</param>
        /// <returns></returns>
        public static string GetSplitName(string nameId)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(nameId) && nameId.Contains("|"))
            {
                int index = nameId.LastIndexOf("|");
                result = nameId.Substring(0, index);//获取Name
            }
            return result;
        }
    }
}