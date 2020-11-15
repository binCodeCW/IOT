using grsvr6Lib;
using System;
using System.Data;
using System.Web;

namespace IOT.MVCWebMis.Commons
{

    public class ServerUtility
    {
        /// <summary>
        /// 将报表生成的二进制数据响应给 HTPP 请求客户端
        /// </summary>
        /// <param name="context"> HTPP 请求对象</param>
        /// <param name="ExportResult">报表生成的二进制数据</param>
        /// <param name="FileName">指定下载(或保存)文件时的默认文件名称</param>
        /// <param name="ContentType">响应的ContentType</param>
        /// <param name="OpenMode">指定生成的数据打开模式，可选[inline|attachment]，"inline"表示在网页中内联显示，"attachment"表示以附件文件形式下载。如果不指定，由浏览器自动确定打开方式。</param>
        public static void ResponseBinary(HttpContextBase context, IGRBinaryObject ExportResult, string FileName, string ContentType, string OpenMode)
        {
            if (ExportResult.DataSize > 0)
            {
                string Disposition = "";

                if (OpenMode != null && OpenMode.Length > 0)
                    Disposition = OpenMode + "; ";

                Disposition += ServerUtility.EncodeAttachmentFileName(context.Request.UserAgent, FileName);

                context.Response.ContentType = ContentType;
                context.Response.AppendHeader("Content-Length", ExportResult.DataSize.ToString());
                context.Response.AppendHeader("Content-Disposition", Disposition);

                context.Response.ClearContent();

                //当发布到IIS，用InteropServices.Marshal.Copy方式复制导出的数据出现了异常，而在VS的IDE里是正常的，所以改用SaveToVariant
                //byte[] buffer = new byte[ExportResult.DataSize];
                //System.Runtime.InteropServices.Marshal.Copy((IntPtr)ExportResult.DataBuf, buffer, 0, ExportResult.DataSize);
                //context.Response.BinaryWrite(buffer);
                object Data = ExportResult.SaveToVariant();
                context.Response.BinaryWrite((byte[])Data);

                context.Response.Flush();
            }
        }

        /// <summary>
        /// 将异常信息文字响应给请求的客户端 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="MessageText"></param>
        public static void ResponseException(HttpContext context, string MessageText)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(MessageText);
        }

        /// <summary>
        /// 为了文件名中的汉字与特殊字符能正确，必须进行分浏览器处理
        /// </summary>
        /// <param name="BrowserAgent"></param>
        /// <param name="RawFileName"></param>
        /// <returns></returns>
        public static string EncodeAttachmentFileName(string BrowserAgent, string RawFileName)
        {
            System.Text.UTF8Encoding UTF8Encoding = new System.Text.UTF8Encoding();
            string EncodedFileName = HttpUtility.UrlEncode(RawFileName, UTF8Encoding);

            // 如果没有BrowserAgent，则默认使用IE的方式进行编码，因为毕竟IE还是占多数的  
            string ret = "filename=\"" + EncodedFileName + "\"";
            if (BrowserAgent != null && BrowserAgent.Length != 0)
            {
                BrowserAgent = BrowserAgent.ToLower();
                // msie 与 edge 采用默认的方式   
                if ((BrowserAgent.IndexOf("msie") == -1) && (BrowserAgent.IndexOf("edge") == -1))
                {
                    // Chrome浏览器，只能采用MimeUtility编码或ISO编码的中文输出  
                    if (BrowserAgent.IndexOf("applewebkit") != -1)
                    {
                        //EncodedFileName = MimeUtility.encodeText(RawFileName, "UTF8", "B");
                        System.Text.UTF8Encoding UTF8EncodingBOM = new System.Text.UTF8Encoding(true);
                        EncodedFileName = UTF8EncodingBOM.GetString(UTF8EncodingBOM.GetBytes(RawFileName));
                        ret = "filename=\"" + EncodedFileName + "\"";
                    }
                    // Safari浏览器，只能采用ISO编码的中文输出  
                    else if (BrowserAgent.IndexOf("safari") != -1)
                    {
                        //28591  iso-8859-1                1252   *
                        //ret = "filename=\"" + new String(filename.getBytes("UTF-8"), "ISO8859-1") + "\"";
                        byte[] UTF8Bytes = UTF8Encoding.GetBytes(RawFileName);
                        string ISO8859Text = System.Text.Encoding.GetEncoding(28591).GetString(UTF8Bytes);
                        ret = "filename=\"" + ISO8859Text + "\"";
                    }
                    // Opera浏览器只能采用filename*  
                    // FireFox浏览器，可以使用MimeUtility或filename*或ISO编码的中文输出  
                    else if ((BrowserAgent.IndexOf("opera") != -1) || (BrowserAgent.IndexOf("mozilla") != -1))
                    {
                        ret = "filename*=UTF-8''" + EncodedFileName;
                    }
                }
            }

            return ret;
        }

        private struct MatchFieldPairType
        {
            public IGRField grField;
            public int MatchColumnIndex;
        }

        // 将 DataReader 的数据转储到 Grid++Report 的记录集中
        public static void FillRecordToReport(IGridppReport Report, IDataReader dr)
        {
            MatchFieldPairType[] MatchFieldPairs = new MatchFieldPairType[Math.Min(Report.DetailGrid.Recordset.Fields.Count, dr.FieldCount)];

            //根据字段名称与列名称进行匹配，建立DataReader字段与Grid++Report记录集的字段之间的对应关系
            int MatchFieldCount = 0;
            for (int i = 0; i < dr.FieldCount; ++i)
            {
                foreach (IGRField fld in Report.DetailGrid.Recordset.Fields)
                {
                    if (String.Compare(fld.RunningDBField, dr.GetName(i), true) == 0)
                    {
                        MatchFieldPairs[MatchFieldCount].grField = fld;
                        MatchFieldPairs[MatchFieldCount].MatchColumnIndex = i;
                        ++MatchFieldCount;
                        break;
                    }
                }
            }


            // 将 DataReader 中的每一条记录转储到 Grid++Report 的记录集中去
            while (dr.Read())
            {
                Report.DetailGrid.Recordset.Append();

                for (int i = 0; i < MatchFieldCount; ++i)
                {
                    if (!dr.IsDBNull(MatchFieldPairs[i].MatchColumnIndex))
                        MatchFieldPairs[i].grField.Value = dr.GetValue(MatchFieldPairs[i].MatchColumnIndex);
                }

                Report.DetailGrid.Recordset.Post();
            }
        }

        // 将 DataTable 的数据转储到 Grid++Report 的记录集中
        public static void FillRecordToReport(IGridppReport Report, DataTable dt)
        {
            MatchFieldPairType[] MatchFieldPairs = new MatchFieldPairType[Math.Min(Report.DetailGrid.Recordset.Fields.Count, dt.Columns.Count)];

            //根据字段名称与列名称进行匹配，建立DataReader字段与Grid++Report记录集的字段之间的对应关系
            int MatchFieldCount = 0;
            for (int i = 0; i < dt.Columns.Count; ++i)
            {
                foreach (IGRField fld in Report.DetailGrid.Recordset.Fields)
                {
                    if (String.Compare(fld.Name, dt.Columns[i].ColumnName, true) == 0)
                    {
                        MatchFieldPairs[MatchFieldCount].grField = fld;
                        MatchFieldPairs[MatchFieldCount].MatchColumnIndex = i;
                        ++MatchFieldCount;
                        break;
                    }
                }
            }


            // 将 DataTable 中的每一条记录转储到 Grid++Report 的记录集中去
            foreach (DataRow dr in dt.Rows)
            {
                Report.DetailGrid.Recordset.Append();

                for (int i = 0; i < MatchFieldCount; ++i)
                {
                    if (!dr.IsNull(MatchFieldPairs[i].MatchColumnIndex))
                        MatchFieldPairs[i].grField.Value = dr[MatchFieldPairs[i].MatchColumnIndex];
                }

                Report.DetailGrid.Recordset.Post();
            }
        }

        public static uint RGBToOleColor(byte r, byte g, byte b)
        {
            return ((uint)b) * 256 * 256 + ((uint)g) * 256 + r;
        }

        public static uint ColorToOleColor(System.Drawing.Color val)
        {
            return RGBToOleColor(val.R, val.G, val.B);
        }

        //判断当前运行进程是不是64位程序
        public static bool Is64bitProcess()
        {
            //IsWow64Process
            //也可以直接用Environment.Is64BitProcess，不过需要DotNet4.0或以上版本。
            return (IntPtr.Size == 8);
        }

        //生成数据源连接串，根据当前运行环境与配置参数进行实际调整
        public static string BuildOLEDBConnectionString()
        {
            //Grid++Report的64位程序通过OLE DB连接Access与32位程序使用完全不同的数据驱动程序，
            //此数据驱动程序(AccessDatabaseEngine_X64.exe)没有跟随Windows系统自动安装，需要从
            //微软网站下载并安装, 下载地址：https://www.microsoft.com/zh-CN/download/details.aspx?id=13255
            //如果直接不能安装，要先将Ofiice卸载后再进行安装，驱动程序安装后，在重新安装Office。
            return (Is64bitProcess() ? "Provider = Microsoft.ACE.OLEDB.12.0;" : "Provider=Microsoft.Jet.OLEDB.4.0;") +
                "User ID=Admin;Data Source=" + @"C:\Grid++Report 6\Samples\Data\NorthWind.mdb";
        }
    }
}