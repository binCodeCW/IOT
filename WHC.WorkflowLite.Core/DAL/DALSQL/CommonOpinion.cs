using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.WorkflowLite.Entity;
using WHC.WorkflowLite.IDAL;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;

namespace WHC.WorkflowLite.DALSQL
{
	/// <summary>
    /// CommonOpinion 的摘要说明。
	/// </summary>
    public class CommonOpinion : BaseDALSQL<CommonOpinionInfo>, ICommonOpinion
	{
		#region 对象实例及构造函数

		public static CommonOpinion Instance
		{
			get
			{
				return new CommonOpinion();
			}
		}
		public CommonOpinion() : base("TBAPP_COMMON_OPINION","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override CommonOpinionInfo DataReaderToEntity(IDataReader dataReader)
		{
			CommonOpinionInfo commonOpinionInfo = new CommonOpinionInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);

            commonOpinionInfo.ID = reader.GetString("ID");
			commonOpinionInfo.FormScene = reader.GetString("FORM_SCENE");
			commonOpinionInfo.BelongUser = reader.GetString("BELONG_USER");
			commonOpinionInfo.Opinion = reader.GetString("OPINION");
			commonOpinionInfo.Seq = reader.GetInt32("SEQ");
			
			return commonOpinionInfo;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(CommonOpinionInfo obj)
		{
		    CommonOpinionInfo info = obj as CommonOpinionInfo;
			Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
 			hash.Add("FORM_SCENE", info.FormScene);
 			hash.Add("BELONG_USER", info.BelongUser);
 			hash.Add("OPINION", info.Opinion);
 			hash.Add("SEQ", info.Seq);
 				
			return hash;
		}

    }
}