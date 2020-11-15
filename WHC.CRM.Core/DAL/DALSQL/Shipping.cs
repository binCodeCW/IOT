using System.Collections;
using System.Data;
using System.Collections.Generic;

using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using WHC.CRM.Entity;
using WHC.CRM.IDAL;

namespace WHC.CRM.DALSQL
{
    /// <summary>
    /// 客户发货地址信息
    /// </summary>
	public class Shipping : BaseDALSQL<ShippingInfo>, IShipping
	{
		#region 对象实例及构造函数

		public static Shipping Instance
		{
			get
			{
				return new Shipping();
			}
		}
		public Shipping() : base("T_CRM_Shipping","ID")
        {
            this.SortField = "CreateTime";
            this.IsDescending = true;
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override ShippingInfo DataReaderToEntity(IDataReader dataReader)
		{
			ShippingInfo info = new ShippingInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Customer_ID = reader.GetString("Customer_ID");
			info.HandNo = reader.GetString("HandNo");
			info.ReceiveAddress = reader.GetString("ReceiveAddress");
			info.ReceiveZipCode = reader.GetString("ReceiveZipCode");
			info.ReceiveMan = reader.GetString("ReceiveMan");
			info.ReceiveMobile = reader.GetString("ReceiveMobile");
			info.ReceivePhone = reader.GetString("ReceivePhone");
			info.Seq = reader.GetString("Seq");
			info.Note = reader.GetString("Note");
			info.Creator = reader.GetString("Creator");
			info.CreateTime = reader.GetDateTime("CreateTime");
			info.Editor = reader.GetString("Editor");
			info.EditTime = reader.GetDateTime("EditTime");
            info.Dept_ID = reader.GetString("Dept_ID");
            info.Company_ID = reader.GetString("Company_ID");			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(ShippingInfo obj)
		{
		    ShippingInfo info = obj as ShippingInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("Customer_ID", info.Customer_ID);
 			hash.Add("HandNo", info.HandNo);
 			hash.Add("ReceiveAddress", info.ReceiveAddress);
 			hash.Add("ReceiveZipCode", info.ReceiveZipCode);
 			hash.Add("ReceiveMan", info.ReceiveMan);
 			hash.Add("ReceiveMobile", info.ReceiveMobile);
 			hash.Add("ReceivePhone", info.ReceivePhone);
 			hash.Add("Seq", info.Seq);
 			hash.Add("Note", info.Note);
 			hash.Add("Creator", info.Creator);
 			hash.Add("CreateTime", info.CreateTime);
 			hash.Add("Editor", info.Editor);
 			hash.Add("EditTime", info.EditTime);
            hash.Add("Dept_ID", info.Dept_ID);
            hash.Add("Company_ID", info.Company_ID); 				
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
            dict.Add("ID", "编号");
            dict.Add("Customer_ID", "客户名称");
            dict.Add("HandNo", "编号");
            dict.Add("ReceiveAddress", "收货地址");
            dict.Add("ReceiveZipCode", "收货邮编");
            dict.Add("ReceiveMan", "收货人");
            dict.Add("ReceiveMobile", "收货人手机");
            dict.Add("ReceivePhone", "收货人固话");
            dict.Add("Seq", "排序序号");
            dict.Add("Note", "备注");
            dict.Add("Creator", "创建人");
            dict.Add("CreateTime", "创建时间");
            dict.Add("Editor", "编辑人");
            dict.Add("EditTime", "编辑时间");
            dict.Add("Dept_ID", "所属部门");
            dict.Add("Company_ID", "所属公司");
            #endregion

            return dict;
        }

    }
}