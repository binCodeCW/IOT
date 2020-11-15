using System;
using System.Data.Common;
using System.Collections.Generic;

using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// Web收藏夹功能
    /// </summary>
	public class WebFavorite : BaseBLL<WebFavoriteInfo>
    {
        public WebFavorite() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
                    
        /// <summary>
        /// 移除单条记录
        /// </summary>
        /// <param name="userid">所属用户</param>
        /// <param name="id">记录ID</param>
        /// <returns></returns>
        public bool RemoveItem(string userid, string id)
        {
            var result = false;
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(id))
            {
                var condition = string.Format("Creator='{0}' AND ID='{1}'", userid, id);
                result = BLLFactory<WebFavorite>.Instance.DeleteByCondition(condition);
            }
            return result;
        }

        /// <summary>
        /// 重新更新收藏夹列表
        /// </summary>
        /// <param name="userid">所属用户</param>
        /// <param name="list">收藏夹列表</param>
        /// <returns></returns>
        public CommonResult EditFavorite(string userid, List<CListItem> list)
        {
            CommonResult result = new CommonResult();
            DbTransaction trans = BLLFactory<WebFavorite>.Instance.CreateTransaction();
            if (trans != null)
            {
                try
                {
                    //先删除就记录
                    var condition = string.Format("Creator='{0}'", userid);
                    BLLFactory<WebFavorite>.Instance.DeleteByCondition(condition, trans);

                    //逐条添加记录
                    int i = list.Count;
                    foreach (CListItem item in list)
                    {
                        WebFavoriteInfo info = new WebFavoriteInfo();
                        info.Title = item.Text;
                        info.Url = item.Value;
                        info.Seq = i--;
                        info.Creator = userid;

                        BLLFactory<WebFavorite>.Instance.Insert(info, trans);
                    }

                    trans.Commit();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.Message;
                    trans.Rollback();
                    LogHelper.Error(ex);
                }
            }
            return result;
        }

        /// <summary>
        /// 更新向上或者向下的顺序
        /// </summary>
        /// <param name="id">记录的ID</param>
        /// <param name="moveUp">往上，还是往下移动，往上则为true</param>
        /// <returns></returns>
        public bool UpDown(string id, bool moveUp)
        {
            //设置排序的规则
            bool IsDescending = true;

            bool result = false;
            WebFavoriteInfo info = FindByID(id);
            if (info != null)
            {
                //构建查询的条件
                string condition = "";
                if (IsDescending)
                {
                    condition = string.Format("Seq {0} {1}", moveUp ? ">" : "<", info.Seq);
                }
                else
                {
                    condition = string.Format("Seq {0} {1}", moveUp ? "<" : ">", info.Seq);
                }

                var list = baseDal.Find(condition);
                decimal newSeq = 0M;
                switch (list.Count)
                {
                    case 0:
                        newSeq = info.Seq;//已在顶部或者底部，顺序默认不变
                        break;

                    case 1:
                        //上面或者下面有一个记录
                        if (IsDescending)
                        {
                            newSeq = moveUp ? (list[0].Seq + 1M) : (list[0].Seq - 1M);
                        }
                        else
                        {
                            newSeq = !moveUp ? (list[0].Seq + 1M) : (list[0].Seq - 1M);
                        }
                        break;

                    case 2:
                        //中间区域,取平均值
                        newSeq = (list[0].Seq + list[1].Seq) / 2M;
                        break;

                    default:
                        //多于两个的情况
                        if (moveUp)
                        {
                            newSeq = (list[list.Count - 2].Seq + list[list.Count - 1].Seq) / 2M;
                        }
                        else
                        {
                            newSeq = (list[0].Seq + list[1].Seq) / 2M;
                        }
                        break;
                }

                //统一修改顺序
                info.Seq = newSeq;
                result = Update(info, info.ID);
            }

            return result;
        }
    }
}
