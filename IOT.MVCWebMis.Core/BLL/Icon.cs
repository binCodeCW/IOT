using System;
using System.Data.Common;
using System.Collections.Generic;

using IOT.MVCWebMis.Entity;
using YH.Framework.ControlUtil;
using YH.Framework.Commons;

namespace IOT.MVCWebMis.BLL
{
    /// <summary>
    /// 系统图表库
    /// </summary>
	public class Icon : BaseBLL<IconInfo>
    {
        public Icon() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// 批量添加图表到数据库
        /// </summary>
        /// <param name="list">包含图标样式和URL路径的</param>
        /// <returns></returns>
        public bool BatchAddIcon(List<CListItem> list, int size)
        {
            bool result = false;
            DbTransaction trans = baseDal.CreateTransaction();
            if(trans != null)
            {
                //先删除所有
                string condition = string.Format("IconSize = {0}", size);
                baseDal.DeleteByCondition(condition, trans);

                //写入数据库
                foreach (CListItem item in list)
                {
                    string iconCls = item.Text;

                    IconInfo info = new IconInfo();
                    info.IconCls = iconCls;
                    info.IconSize = size;
                    info.IconUrl = item.Value;

                    baseDal.Insert(info, trans);
                }
                try
                {
                    trans.Commit();
                    result = true;
                }
                catch(Exception ex)
                {
                    trans.Rollback();
                    throw;
                }
            }

            return result;
        }
    }
}
