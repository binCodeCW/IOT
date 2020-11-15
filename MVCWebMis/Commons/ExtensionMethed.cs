using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using YH.Framework.Commons;

namespace IOT.MVCWebMis.Controllers
{
    public static class ExtensionMethed
    {
        public static SearchCondition AddNumberCondition(this SearchCondition searchCondition, string fieldName, string fieldValue)
        {
            if (!string.IsNullOrEmpty(fieldValue))
            {
                bool isRangeValue = fieldValue.Contains('~');//判断是否为区间的值，否则使用Equal操作符
                string[] itemArray = fieldValue.Split('~');
                if (itemArray != null)
                {
                    decimal value = 0M;
                    bool result = false;

                    if (itemArray.Length > 0)
                    {
                        result = decimal.TryParse(itemArray[0].Trim(), out value);
                        if (result)
                        {
                            if (isRangeValue)
                            {
                                searchCondition.AddCondition(fieldName, value, SqlOperator.MoreThanOrEqual);
                            }
                            else
                            {
                                searchCondition.AddCondition(fieldName, value, SqlOperator.Equal);
                            }
                        }
                    }
                    if (itemArray.Length > 1)
                    {
                        result = decimal.TryParse(itemArray[1].Trim(), out value);
                        if (result)
                        {
                            searchCondition.AddCondition(fieldName, value, SqlOperator.LessThanOrEqual);
                        }
                    }
                }
            }
            return searchCondition;
        }

        public static SearchCondition AddDateCondition(this SearchCondition searchCondition, string fieldName, string fieldValue)
        {
            if (!string.IsNullOrEmpty(fieldValue))
            {
                string[] itemArray = fieldValue.Split('~');
                if (itemArray != null)
                {
                    DateTime value;
                    bool result = false;
                    if (itemArray.Length > 0)
                    {
                        result = DateTime.TryParse(itemArray[0].Trim(), out value);
                        if (result)
                        {
                            searchCondition.AddCondition(fieldName, value, SqlOperator.MoreThanOrEqual);
                        }
                    }
                    if (itemArray.Length > 1)
                    {
                        result = DateTime.TryParse(itemArray[1].Trim(), out value);
                        if (result)
                        {
                            searchCondition.AddCondition(fieldName, value.AddDays(1), SqlOperator.LessThan);
                        }
                    }
                }
            }
            return searchCondition;
        }
        
        /// <summary>
        /// 去除字符中的null字样及控股
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string TrimNull(this string input)
        {
            string result = "";
            if (!string.IsNullOrEmpty(input))
            {
                result = (input.Trim() == "null") ? "" : input.Trim();
            }
            return result;
        }


        /// <summary>
        /// 将json数据反序列化为Dictionary
        /// </summary>
        /// <param name="jsonData">json数据</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictObject(this string jsonData)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(jsonData))
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                try
                {
                    //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                    dict = jss.Deserialize<Dictionary<string, string>>(jsonData);
                }
                catch (Exception ex)
                {
                    LogTextHelper.Error(ex);
                    //throw;//简化显示，暂时不需抛出异常
                }
            }
            return dict;
        }
    }
}