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
    /// 供应商信息
    /// </summary>
	public class Manufacturer : BaseDALSQL<ManufacturerInfo>, IManufacturer
	{
		#region 对象实例及构造函数

		public static Manufacturer Instance
		{
			get
			{
				return new Manufacturer();
			}
		}
		public Manufacturer() : base("T_CRM_Manufacturer","ID")
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
		protected override ManufacturerInfo DataReaderToEntity(IDataReader dataReader)
		{
			ManufacturerInfo info = new ManufacturerInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.HandNo = reader.GetString("HandNo");
			info.Name = reader.GetString("Name");
			info.Telephone = reader.GetString("Telephone");
			info.Mobile = reader.GetString("Mobile");
			info.Address = reader.GetString("Address");
			info.Email = reader.GetString("Email");
            info.QQ = reader.GetString("QQ");
			info.ZipCode = reader.GetString("ZipCode");
			info.Note = reader.GetString("Note");
			info.Seq = reader.GetString("Seq");
			info.AttachGUID = reader.GetString("AttachGUID");
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
        protected override Hashtable GetHashByEntity(ManufacturerInfo obj)
		{
		    ManufacturerInfo info = obj as ManufacturerInfo;
			Hashtable hash = new Hashtable(); 
			
			hash.Add("ID", info.ID);
 			hash.Add("HandNo", info.HandNo);
 			hash.Add("Name", info.Name);
 			hash.Add("Telephone", info.Telephone);
 			hash.Add("Mobile", info.Mobile);
 			hash.Add("Address", info.Address);
 			hash.Add("Email", info.Email);
            hash.Add("QQ", info.QQ);
 			hash.Add("ZipCode", info.ZipCode);
 			hash.Add("Note", info.Note);
 			hash.Add("Seq", info.Seq);
 			hash.Add("AttachGUID", info.AttachGUID);
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
            dict.Add("HandNo", "编号");
            dict.Add("Name", "供应商名称");
            dict.Add("Telephone", "供应商电话");
            dict.Add("Mobile", "供应商手机");
            dict.Add("Address", "供应商地址");
            dict.Add("Email", "电子邮件");
            dict.Add("Qq", "QQ号码");
            dict.Add("ZipCode", "邮政编码");
            dict.Add("Note", "备注");
            dict.Add("Seq", "排序序号");
            dict.Add("AttachGUID", "附件组别ID");
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