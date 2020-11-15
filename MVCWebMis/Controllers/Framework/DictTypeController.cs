using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WHC.Dictionary.BLL;
using WHC.Dictionary.Entity;
using YH.Framework.Commons;
using YH.Framework.Commons.Collections;
using YH.Framework.ControlUtil;

namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// 数据字典大类的控制器
    /// </summary>
    public class DictTypeController :  BusinessController<DictType, DictTypeInfo>
    {
        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(DictTypeInfo info)
        {
            //留给子类对参数对象进行修改
            info.Editor = CurrentUser.ID.ToString();
            info.LastUpdated = DateTime.Now;
        }

        protected override void OnBeforeUpdate(DictTypeInfo info)
        {
            //留给子类对参数对象进行修改
            info.Editor = CurrentUser.ID.ToString();
            info.LastUpdated = DateTime.Now;
        }
        #endregion

        /// <summary>
        /// 用作下拉列表的菜单Json数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDictJson()
        {
            List<DictTypeInfo> list = baseBLL.GetAll();
            list = CollectionHelper<DictTypeInfo>.Fill("-1", 0, list, "PID", "ID", "Name");

            List<CListItem> itemList = new List<CListItem>();
            foreach (DictTypeInfo info in list)
            {
                itemList.Add(new CListItem(info.Name, info.ID));
            }
            itemList.Add(new CListItem("无", "-1"));

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        #region EasyUI的树形列表
        /// <summary>
        /// 获取树形展示数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeJson()
        {
            List<TreeData> treeList = new List<TreeData>();
            List<DictTypeInfo> typeList = BLLFactory<DictType>.Instance.Find("PID='-1' ");
            foreach (DictTypeInfo info in typeList)
            {
                TreeData node = new TreeData(info.ID, info.PID, info.Name);
                GetTreeJson(info.ID, node);

                treeList.Add(node);
            }
            return ToJsonContent(treeList);
        }

        /// <summary>
        /// 递归获取树形信息
        /// </summary>
        /// <returns></returns>
        private void GetTreeJson(string PID, TreeData treeNode)
        {
            string condition = string.Format("PID='{0}' ", PID);
            List<DictTypeInfo> nodeList = BLLFactory<DictType>.Instance.Find(condition);
            StringBuilder content = new StringBuilder();

            foreach (DictTypeInfo model in nodeList)
            {
                TreeData node = new TreeData(model.ID, model.PID, model.Name);
                treeNode.children.Add(node);

                GetTreeJson(model.ID, node);
            }
        } 
        #endregion

        #region BootStrap JSTree的树形列表

        /// <summary>
        /// 获取树形展示数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJsTreeJson(bool update = false)
        {
            #region 使用MemoryCache缓存提高速度

            System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
            string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, "");

            if(update)
            {
                MemoryCacheHelper.RemoveItem(key);
            }

            var result = MemoryCacheHelper.GetCacheItem<ActionResult>(key,
                delegate()
                {
                    List<JsTreeData> treeList = new List<JsTreeData>();
                    List<DictTypeInfo> typeList = BLLFactory<DictType>.Instance.Find("PID='-1' ");
                    foreach (DictTypeInfo info in typeList)
                    {
                        JsTreeData node = new JsTreeData(info.ID, info.Name, "fa fa-file icon-state-warning icon-lg");
                        GetJsTreeJson(info.ID, node);

                        treeList.Add(node);
                    }
                    return ToJsonContent(treeList);
                },
                new TimeSpan(0, 30, 0));//30分钟过期

            return result;

            #endregion
        }

        /// <summary>
        /// 递归获取树形信息
        /// </summary>
        /// <returns></returns>
        private void GetJsTreeJson(string PID, JsTreeData treeNode)
        {
            string condition = string.Format("PID='{0}' ", PID);
            List<DictTypeInfo> nodeList = BLLFactory<DictType>.Instance.Find(condition);
            StringBuilder content = new StringBuilder();

            foreach (DictTypeInfo model in nodeList)
            {
                JsTreeData node = new JsTreeData(model.ID, model.Name, "fa fa-file icon-state-warning icon-lg");
                treeNode.children.Add(node);

                GetJsTreeJson(model.ID, node);
            }
        }
        #endregion

    }
}
