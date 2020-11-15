﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Newtonsoft.Json;

namespace YH.Framework.Language
{
    /// <summary>
    /// 基于JSON的多语言处理类
    /// </summary>
    public class JsonLanguage
    {
        //普通局部变量
        private static object syncRoot = new Object();
        private static JsonLanguage instance = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static JsonLanguage Default
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new JsonLanguage();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        private Dictionary<string,string> resources = null;

        /// <summary>
        /// 用来放置语言键的对照字典
        /// </summary>
        private Dictionary<string, string> LangKeyDict = new Dictionary<string, string>();

        /// <summary>
        /// 私有函数
        /// </summary>
        private JsonLanguage()
        {
            LoadLanguage();
        }

        #region 加载多语言的JSON内容

        /// <summary>
        /// 根据语言初始化信息。
        /// 加载对应语言的JSON信息，把翻译信息存储在全属性resources里面。
        /// </summary>
        /// <param name="language">默认的语言类型，如zh-Hans，en-US等</param>
        public void LoadLanguage(string language = "")
        {
            if (string.IsNullOrEmpty(language))
            {
                language = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            }

            this.resources = new Dictionary<string, string>();
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("lang/{0}", language));
            if (Directory.Exists(dir))
            {
                var jsonFiles = Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories);
                foreach (string file in jsonFiles)
                {
                    LoadFile(file);
                }
            }
        }

        /// <summary>
        /// 加载单一的文件
        /// </summary>
        /// <param name="file">JSON格式文件</param>
        private void LoadFile(string file)
        {
            var content = File.ReadAllText(file, Encoding.UTF8);
            if (!string.IsNullOrEmpty(content))
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                foreach (string key in dict.Keys)
                {
                    //遍历集合如果语言资源键值不存在，则创建，否则更新
                    if (!resources.ContainsKey(key))
                    {
                        resources.Add(key, dict[key]);
                    }
                    else
                    {
                        resources[key] = dict[key];
                    }
                }
            }
        } 
        #endregion

        #region 翻译语言资源Json文件

        /// <summary>
        /// 为了减少翻译工作的繁琐，我们可以先保留翻译文本的空值，使用百度API进行翻译后，序列号内容到JSON文件。
        /// 然后我们在开发过程中的时候调整翻译文本内容即可（本接口目的是为了开发翻译内容方便）。
        /// </summary>
        /// <param name="sorted">是否对字典进行排序</param>
        /// <param name="language">当前的语言缩写词</param>
        public void Translate(bool sorted = true, string language = "")
        {
            string from = "zh";
            string to = "en";

            if (string.IsNullOrEmpty(language))
            {
                language = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            }

            //根据当前的语言获取API接口转换到的语言缩写
            if (!string.IsNullOrEmpty(language))
            {
                to = language.Substring(0, language.IndexOf('-'));//如果en-Us则获得en
            }

            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("lang\\{0}", language));
            if (Directory.Exists(dir))
            {
                var jsonFiles = Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories);
                foreach (string file in jsonFiles)
                {
                    Debug.WriteLine(file);//测试获得的文件地址
                    TranslateFile(file, from, to, sorted);
                }
            }

            //重新加载
            LoadLanguage();
        }

        /// <summary>
        /// 对单一的文件中的空白翻译内容，使用接口进行翻译
        /// </summary>
        /// <param name="file">JSON格式文件</param>
        private void TranslateFile(string file, string from = "zh", string to = "en",bool sorted = true)
        {
            var content = File.ReadAllText(file, Encoding.UTF8);
            if (!string.IsNullOrEmpty(content))
            {
                bool modified = false;
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                String[] keyStr = dict.Keys.ToArray<String>();
                for (int i = 0; i < dict.Keys.Count; i++)
                {
                    var key = keyStr[i];

                    //遍历集合进行翻译
                    var value = dict[key];
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        //如果值为空，那么调用翻译接口处理
                        var newValue = TranslationHelper.Translate(key, from, to);
                        if (!string.IsNullOrWhiteSpace(newValue))
                        {
                            dict[key] = newValue;

                            modified = true;//设置修改标志
                        }
                    }
                }

                //重新写入内容
                //if (modified)
                {
                    string newContent = "";
                    if (sorted)
                    {
                        //进行排序
                        SortedDictionary<string, string> sortedDict = new SortedDictionary<string, string>(dict);
                        newContent = JsonConvert.SerializeObject(sortedDict, Formatting.Indented);
                    }
                    else
                    {
                        //不排序
                        newContent = JsonConvert.SerializeObject(dict, Formatting.Indented);
                    }
                    //写入原来的文件，覆盖
                    File.WriteAllText(file, newContent, Encoding.UTF8);

                    Debug.WriteLine(newContent);
                    Debug.WriteLine(string.Format("文件[{0}]内容已更新", file));//调试更新信息
                }
            }
        } 

        #endregion


        /// <summary>
        /// 根据对应的键值获取对应的语言参考
        /// </summary>
        /// <param name="key">传递过来的键值参考，如：姓名</param>
        /// <returns></returns>
        public string GetString(string key)
        {
            //如果是中文，那么：key= 你好，如果是英文，那么：key=hello
            if(!LangKeyDict.ContainsKey(key))
            {
                LangKeyDict.Add(key, key);
            }
            string newKey = LangKeyDict[key];//如：你好

            //默认返回结果就是传递的值
            string result = newKey;
            if(resources != null && resources.ContainsKey(newKey))
            {
                result = resources[newKey] as string;
                //如果集合记录为空，那么默认用原来的值
                if(string.IsNullOrWhiteSpace(result))
                {
                    result = newKey;
                }
            }

            var langKeyValue = StringUtil.ToProperCase(result);

            //如：langKeyValue = hello
            if (!LangKeyDict.ContainsKey(langKeyValue))
            {
                LangKeyDict.Add(langKeyValue, key);
            }
            return langKeyValue;
        }

        /// <summary>
        /// 用于判断有对应语言的资源文件
        /// </summary>
        public bool HasResource
        {
            get
            {
                if(this.resources != null && this.resources.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

    }
}
