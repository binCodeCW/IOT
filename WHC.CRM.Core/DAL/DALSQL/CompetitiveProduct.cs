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
    /// 竞争对手产品信息
    /// </summary>
	public class CompetitiveProduct : BaseDALSQL<CompetitiveProductInfo>, ICompetitiveProduct
	{
		#region 对象实例及构造函数

		public static CompetitiveProduct Instance
		{
			get
			{
				return new CompetitiveProduct();
			}
		}
		public CompetitiveProduct() : base("T_CRM_CompetitiveProduct","ID")
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
		protected override CompetitiveProductInfo DataReaderToEntity(IDataReader dataReader)
		{
			CompetitiveProductInfo info = new CompetitiveProductInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetString("ID");
			info.Competitor_ID = reader.GetString("Competitor_ID");
			info.HandNo = reader.GetString("HandNo");
			info.MaterialCode = reader.GetString("MaterialCode");
			info.BarCode = reader.GetString("BarCode");
			info.ProductType = reader.GetString("ProductType");
			info.ProductName = reader.GetString("ProductName");
			info.PinyinCode = reader.GetString("PinyinCode");
			info.Specification = reader.GetString("Specification");
			info.Model = reader.GetString("Model");
			info.Unit = reader.GetString("Unit");
			info.Color = reader.GetString("Color");
			info.ProductSize = reader.GetString("ProductSize");
			info.PurchasePrice = reader.GetDecimal("PurchasePrice");
			info.SalePrice = reader.GetDecimal("SalePrice");
			info.Manufacture = reader.GetString("Manufacture");
			info.AttachGUID = reader.GetString("AttachGUID");
			info.Note = reader.GetString("Note");
			info.Status = reader.GetInt32("Status");
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
        protected override Hashtable GetHashByEntity(CompetitiveProductInfo obj)
        {
            CompetitiveProductInfo info = obj as CompetitiveProductInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("Competitor_ID", info.Competitor_ID);
            hash.Add("HandNo", info.HandNo);
            hash.Add("MaterialCode", info.MaterialCode);
            hash.Add("BarCode", info.BarCode);
            hash.Add("ProductType", info.ProductType);
            hash.Add("ProductName", info.ProductName);
            hash.Add("PinyinCode", info.PinyinCode);
            hash.Add("Specification", info.Specification);
            hash.Add("Model", info.Model);
            hash.Add("Unit", info.Unit);
            hash.Add("Color", info.Color);
            hash.Add("ProductSize", info.ProductSize);
            hash.Add("PurchasePrice", info.PurchasePrice);
            hash.Add("SalePrice", info.SalePrice);
            hash.Add("Manufacture", info.Manufacture);
            hash.Add("AttachGUID", info.AttachGUID);
            hash.Add("Note", info.Note);
            hash.Add("Status", info.Status);
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
            dict.Add("Competitor_ID", "竞争对手");
            dict.Add("HandNo", "编号");
            dict.Add("MaterialCode", "物料编码");
            dict.Add("BarCode", "条形码");
            dict.Add("ProductType", "产品类型");
            dict.Add("ProductName", "产品名称");
            dict.Add("PinyinCode", "拼音码");
            dict.Add("Specification", "产品规格");
            dict.Add("Model", "产品型号");
            dict.Add("Unit", "标准单位");
            dict.Add("Color", "颜色");
            dict.Add("ProductSize", "尺寸");
            dict.Add("PurchasePrice", "成本价");
            dict.Add("SalePrice", "销售价");
            dict.Add("Manufacture", "供应商");
            dict.Add("AttachGUID", "附件组别ID");
            dict.Add("Note", "备注");
            dict.Add("Status", "停用状态[0正常1停用]");
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