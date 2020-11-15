﻿using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

using YH.Security.Entity;
using YH.Security.IDAL;
using YH.Framework.Commons;
using System.Collections.Generic;

namespace YH.Security.DALSQL
{
    public class SystemType : YH.Framework.ControlUtil.BaseDALSQL<SystemTypeInfo>, ISystemType
    {
        #region 对象实例及构造函数

        public static SystemType Instance
        {
            get
            {
                return new SystemType();
            }
        }
        public SystemType()
            : base("T_ACL_SystemType", "OID")
        {
            SortField = "OID";
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override SystemTypeInfo DataReaderToEntity(IDataReader dataReader)
        {
            SystemTypeInfo systemTypeInfo = new SystemTypeInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            systemTypeInfo.OID = reader.GetString("OID");
            systemTypeInfo.Name = reader.GetString("Name");
            systemTypeInfo.CustomID = reader.GetString("CustomID");
            systemTypeInfo.Authorize = reader.GetString("Authorize");
            systemTypeInfo.Note = reader.GetString("Note");

            return systemTypeInfo;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(SystemTypeInfo obj)
        {
            SystemTypeInfo info = obj as SystemTypeInfo;
            Hashtable hash = new Hashtable();

            hash.Add("OID", info.OID);
            hash.Add("Name", info.Name);
            hash.Add("CustomID", info.CustomID);
            hash.Add("Authorize", info.Authorize);
            hash.Add("Note", info.Note);

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
            dict.Add("OID", "系统标识");
            dict.Add("Name", "系统名称");
            dict.Add("CustomID", "客户编码");
            dict.Add("Authorize", "授权编码");
            dict.Add("Note", "备注");
            #endregion

            return dict;
        }

        /// <summary>
        /// 指定具体的列表显示字段
        /// </summary>
        /// <returns></returns>
        public override string GetDisplayColumns()
        {
            return "OID,Name,CustomID,Authorize,Note";
        }

        /// <summary>
        /// 根据系统OID获取对应的系统信息
        /// </summary>
        /// <param name="oid">系统OID</param>
        /// <returns></returns>
        public SystemTypeInfo FindByOID(string oid)
        {
            string condition = string.Format("OID='{0}'", oid);
            return base.FindSingle(condition);
        }

        public bool VerifySystem(string serialNumber, string typeID, int authorizeAmount)
        {
            Database db = CreateDatabase();
            DbCommand command = null;

            bool flag = false;
            string sql = string.Format("SELECT Count(ID) As Records FROM T_ACL_SystemAuthorize WHERE SystemType_OID='{0}' ", typeID);
            command = db.GetSqlStringCommand(sql);
            int num = Convert.ToInt32(db.ExecuteScalar(command).ToString());
            if (num <= authorizeAmount)
            {
                sql = string.Format("SELECT * FROM T_ACL_SystemAuthorize WHERE Content='{0}'  And SystemType_OID='{1}' ", serialNumber, typeID);

                command = db.GetSqlStringCommand(sql);
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    flag = reader.Read();
                    reader.Close();
                }

                if (!flag)
                {
                    flag = num < authorizeAmount;
                    if (flag)
                    {
                        sql = string.Format("INSERT INTO T_ACL_SystemAuthorize (SystemType_OID,Content) VALUES ('{0}', '{1}') ", typeID, serialNumber);
                        command = db.GetSqlStringCommand(sql);
                        db.ExecuteNonQuery(command);
                    }
                }
            }
            return flag;
        }
    }
}