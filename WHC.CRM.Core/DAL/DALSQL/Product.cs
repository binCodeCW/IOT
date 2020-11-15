using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Pager.Entity;
using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.CRM.Entity;
using WHC.CRM.IDAL;

namespace WHC.CRM.DALSQL
{
    /// <summary>
    /// 产品信息
    /// </summary>
    public class Product : BaseDALSQL<ProductInfo>, IProduct
    {
        #region 对象实例及构造函数

        public static Product Instance
        {
            get
            {
                return new Product();
            }
        }
        public Product()
            : base("T_CRM_Product", "ID")
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
        protected override ProductInfo DataReaderToEntity(IDataReader dataReader)
        {
            ProductInfo info = new ProductInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.HandNo = reader.GetString("HandNo");
            info.MaterialCode = reader.GetString("MaterialCode");
            info.BarCode = reader.GetString("BarCode");
            info.ProductType = reader.GetString("ProductType");
            info.ProductName = reader.GetString("ProductName");
            info.PinyinCode = reader.GetString("PinyinCode");
            info.Specification = reader.GetString("Specification");
            info.Model = reader.GetString("Model");
            info.Color = reader.GetString("Color");
            info.ProductSize = reader.GetString("ProductSize");
            info.Unit = reader.GetString("Unit");
            info.CostPrice = reader.GetDecimal("CostPrice");
            info.SalePrice = reader.GetDecimal("SalePrice");
            info.Quantity = reader.GetInt32("Quantity");
            info.Manufacture_ID = reader.GetString("Manufacture_ID");
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
        protected override Hashtable GetHashByEntity(ProductInfo obj)
        {
            ProductInfo info = obj as ProductInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("HandNo", info.HandNo);
            hash.Add("MaterialCode", info.MaterialCode);
            hash.Add("BarCode", info.BarCode);
            hash.Add("ProductType", info.ProductType);
            hash.Add("ProductName", info.ProductName);
            hash.Add("PinyinCode", info.PinyinCode);
            hash.Add("Specification", info.Specification);
            hash.Add("Model", info.Model);
            hash.Add("Color", info.Color);
            hash.Add("ProductSize", info.ProductSize);
            hash.Add("Unit", info.Unit);
            hash.Add("CostPrice", info.CostPrice);
            hash.Add("SalePrice", info.SalePrice);
            hash.Add("Quantity", info.Quantity);
            hash.Add("Manufacture_ID", info.Manufacture_ID);
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
            dict.Add("HandNo", "产品编码");
            dict.Add("MaterialCode", "物料编码");
            dict.Add("BarCode", "条形码");
            dict.Add("ProductType", "产品类型");
            dict.Add("ProductName", "产品名称");
            dict.Add("PinyinCode", "拼音码");
            dict.Add("Specification", "产品规格");
            dict.Add("Model", "产品型号");
            dict.Add("Color", "颜色");
            dict.Add("ProductSize", "尺寸");
            dict.Add("Unit", "标准单位");
            dict.Add("CostPrice", "成本价");
            dict.Add("SalePrice", "销售价");
            dict.Add("Quantity", "产品数量");
            dict.Add("Manufacture_ID", "供应商");
            dict.Add("AttachGUID", "附件组别ID");
            dict.Add("Note", "备注");
            dict.Add("Status", "停用状态");
            dict.Add("Creator", "创建人");
            dict.Add("CreateTime", "创建时间");
            dict.Add("Editor", "编辑人");
            dict.Add("EditTime", "编辑时间");
            dict.Add("Dept_ID", "所属部门");
            dict.Add("Company_ID", "所属公司");
            #endregion

            return dict;
        }

                
        /// <summary>
        /// 根据产品ID，增加或减少相关产品的数量
        /// </summary>
        /// <param name="id">产品ID</param>
        /// <param name="quantity">产品数量，正数为增加，负数为减少</param>
        /// <returns></returns>
        public bool ModifyQuantity(string id, int quantity, DbTransaction trans = null)
        {
            string sql = string.Format("Update {0} set Quantity = Quantity + {1} Where ID = '{2}'", tableName, quantity, id);
            return SqlExecute(sql, trans) > 0;
        }
    }
}