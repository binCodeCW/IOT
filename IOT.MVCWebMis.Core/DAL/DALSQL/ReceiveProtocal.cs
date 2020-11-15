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
    /// 收到协议
    /// </summary>
	public class ReceiveProtocal : BaseDALSQL<ReceiveProtocalInfo>, IReceiveProtocal
	{
		#region 对象实例及构造函数

		public static ReceiveProtocal Instance
		{
			get
			{
				return new ReceiveProtocal();
			}
		}
		public ReceiveProtocal() : base("TP_ReceiveProtocal","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override ReceiveProtocalInfo DataReaderToEntity(IDataReader dataReader)
		{
			ReceiveProtocalInfo info = new ReceiveProtocalInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.FromUserId = reader.GetString("FromUserId");
			info.MsgType = reader.GetString("MsgType");
			info.Seq = reader.GetString("Seq");
			info.Content = reader.GetString("Content");
			info.Protocal = reader.GetString("Protocal");
			info.CreateTime = reader.GetDateTime("CreateTime");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(ReceiveProtocalInfo obj)
		{
		    ReceiveProtocalInfo info = obj as ReceiveProtocalInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("FromUserId", info.FromUserId);
 			hash.Add("MsgType", info.MsgType);
 			hash.Add("Seq", info.Seq);
 			hash.Add("Content", info.Content);
 			hash.Add("Protocal", info.Protocal);
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
            dict.Add("ID", "ID");
             dict.Add("FromUserId", "发送方用户ID");
             dict.Add("MsgType", "消息类型");
             dict.Add("Seq", "序号");
             dict.Add("Content", "承载的内容");
             dict.Add("Protocal", "协议数据");
             dict.Add("CreateTime", "创建时间");
             #endregion

            return dict;
        }
		
        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "ID,FromUserId,MsgType,Seq,Content,Protocal,CreateTime";
        }
    }
}