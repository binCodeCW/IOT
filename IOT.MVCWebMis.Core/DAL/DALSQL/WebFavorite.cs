using System.Collections;
using System.Data;
using System.Collections.Generic;

using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using IOT.MVCWebMis.Entity;
using IOT.MVCWebMis.IDAL;

namespace IOT.MVCWebMis.DALSQL
{
    /// <summary>
    /// WebFavorite
    /// </summary>
	public class WebFavorite : BaseDALSQL<WebFavoriteInfo>, IWebFavorite
	{
		#region 对象实例及构造函数

		public static WebFavorite Instance
		{
			get
			{
				return new WebFavorite();
			}
		}
		public WebFavorite() : base("TB_WebFavorite","ID")
		{
            this.SortField = "Seq";
            this.IsDescending = true;
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override WebFavoriteInfo DataReaderToEntity(IDataReader dataReader)
		{
			WebFavoriteInfo info = new WebFavoriteInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Title = reader.GetString("Title");
			info.Url = reader.GetString("Url");
			info.Seq = reader.GetDecimal("Seq");
			info.Creator = reader.GetString("Creator");
			info.CreateTime = reader.GetDateTime("CreateTime");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(WebFavoriteInfo obj)
		{
		    WebFavoriteInfo info = obj as WebFavoriteInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Title", info.Title);
 			hash.Add("Url", info.Url);
 			hash.Add("Seq", info.Seq);
 			hash.Add("Creator", info.Creator);
 			hash.Add("CreateTime", info.CreateTime);
 				
			return hash;
		}

        /// <summary>
        /// 获取字段中文别名（用于界面显示）的字典集合
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> GetColumnNameAlias()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            #region 添加别名解析
            //dict.Add("ID", "编号");
            dict.Add("ID", "");
             dict.Add("Title", "标题");
             dict.Add("Url", "URL地址");
             dict.Add("Seq", "排序");
             dict.Add("Creator", "创建人");
             dict.Add("CreateTime", "创建时间");
             #endregion

            return dict;
        }

    }
}