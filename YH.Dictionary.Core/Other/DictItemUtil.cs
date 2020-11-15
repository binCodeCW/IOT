﻿using WHC.Dictionary.BLL;

using System.Collections.Generic;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;

namespace WHC.Dictionary
{
    public class DictItemUtil
    {
        /// <summary>
        /// 根据字典类型获取对应的CListItem集合
        /// </summary>
        /// <param name="dictTypeName"></param>
        /// <returns></returns>
        public static CListItem[] GetDictByDictType(string dictTypeName)
        {
            List<CListItem> itemList = new List<CListItem>();
            Dictionary<string, string> dict = BLLFactory<DictData>.Instance.GetDictByDictType(dictTypeName);
            foreach (string key in dict.Keys)
            {
                itemList.Add(new CListItem(key, dict[key]));
            }
            return itemList.ToArray();
        }
    }
}
