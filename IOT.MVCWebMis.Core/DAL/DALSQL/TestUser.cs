﻿using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using YH.Framework.Commons;
using YH.Framework.ControlUtil;
using Microsoft.Practices.EnterpriseLibrary.Data;
using IOT.MVCWebMis.Entity;
using IOT.MVCWebMis.IDAL;

namespace IOT.MVCWebMis.DALSQL
{
    /// <summary>
    /// 为测试用的数据表
    /// </summary>
    public class TestUser : BaseDALSQL<TestUserInfo>, ITestUser
    {
        #region 对象实例及构造函数

        public static TestUser Instance
        {
            get
            {
                return new TestUser();
            }
        }
        public TestUser()
            : base("TB_TestUser", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override TestUserInfo DataReaderToEntity(IDataReader dataReader)
        {
            TestUserInfo info = new TestUserInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetString("ID");
            info.Name = reader.GetString("Name");
            info.Mobile = reader.GetString("Mobile");
            info.Email = reader.GetString("Email");
            info.Homepage = reader.GetString("Homepage");
			info.Hobby = reader.GetString("Hobby");
			info.Gender = reader.GetString("Gender");
            info.Age = reader.GetInt32("Age");
            info.BirthDate = reader.GetDateTime("BirthDate");
            info.Height = reader.GetDecimal("Height");
            //info.Portrait = reader.GetBytes("Portrait");
            info.Note = reader.GetString("Note");
            info.Editor = reader.GetString("Editor");
            info.EditTime = reader.GetDateTime("EditTime");
            info.Creator = reader.GetString("Creator");
            info.CreateTime = reader.GetDateTime("CreateTime");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(TestUserInfo obj)
        {
            TestUserInfo info = obj as TestUserInfo;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("Name", info.Name);
            hash.Add("Mobile", info.Mobile);
            hash.Add("Email", info.Email);
            hash.Add("Homepage", info.Homepage);
 			hash.Add("Hobby", info.Hobby);
 			hash.Add("Gender", info.Gender);
            hash.Add("Age", info.Age);
            hash.Add("BirthDate", info.BirthDate);
            hash.Add("Height", info.Height);
            //hash.Add("Portrait", info.Portrait);
            hash.Add("Note", info.Note);
            hash.Add("Editor", info.Editor);
            hash.Add("EditTime", info.EditTime);
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
            dict.Add("Name", "姓名");
            dict.Add("Mobile", "手机");
            dict.Add("Email", "邮箱");
            dict.Add("Homepage", "主页");
             dict.Add("Hobby", "兴趣爱好");
             dict.Add("Gender", "性别");
            dict.Add("Age", "年龄");
            dict.Add("BirthDate", "出生日期");
            dict.Add("Height", "身高");
            dict.Add("Portrait", "肖像");
            dict.Add("Note", "备注");
            dict.Add("Editor", "编辑人");
            dict.Add("EditTime", "编辑时间");
            dict.Add("Creator", "创建人");
            dict.Add("CreateTime", "创建时间");
            #endregion

            return dict;
        }

        /// <summary>
        /// 根据个人图片枚举类型获取图片数据
        /// </summary>
        /// <param name="imagetype">图片枚举类型</param>
        /// <returns></returns>
        public byte[] GetPersonImageBytes(string id, string imagetype = "个人肖像")
        {
            byte[] imageBytes = null;
            string fieldName = GetFieldNameByImageType(imagetype);
            if (!string.IsNullOrEmpty(fieldName))
            {
                string sql = string.Format("Select {0} from {1} where Id = '{2}' ", fieldName, tableName, id);
                Database db = CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(sql);

                using (IDataReader reader = db.ExecuteReader(dbCommand))
                {
                    if (reader.Read())
                    {
                        imageBytes = (reader.IsDBNull(reader.GetOrdinal(fieldName))) ? null : (byte[])reader[0];
                    }
                }
            }

            return imageBytes;
        }

        /// <summary>
        /// 根据图片枚举类型获取对应的字段名称
        /// </summary>
        /// <param name="imageType">图片枚举类型</param>
        /// <returns></returns>
        private string GetFieldNameByImageType(string imageType)
        {
            string fieldName = "Portrait";
            switch (imageType)
            {
                case "个人肖像":
                    fieldName = "Portrait";
                    break;
            }
            return fieldName;
        }

        /// <summary>
        /// 更新个人相关图片数据
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="imageBytes">图片字节数组</param>
        /// <param name="imagetype">图片类型</param>
        /// <returns></returns>
        public bool UpdatePersonImageBytes(string id, byte[] imageBytes, string imagetype = "个人肖像")
        {
            bool result = false;
            string fieldName = GetFieldNameByImageType(imagetype);
            if (!string.IsNullOrEmpty(fieldName))
            {
                string sql = string.Format("update {0} set {1}=@image where Id = '{2}' ", tableName, fieldName, id);
                Database db = CreateDatabase();
                DbCommand dbCommand = db.GetSqlStringCommand(sql);
                db.AddInParameter(dbCommand, "image", DbType.Binary, imageBytes);
                result = db.ExecuteNonQuery(dbCommand) > 0;
            }
            return result;
        }

    }
}