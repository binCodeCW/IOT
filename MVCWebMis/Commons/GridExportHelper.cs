using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web;
using grsvr6Lib;
using Newtonsoft.Json;

namespace IOT.MVCWebMis.Commons
{
    /// <summary>
    /// 对Grid++报表处理的辅助类
    /// </summary>
    public class GridExportHelper : IDisposable
    {
        GridppReportServer Report { get; set; }
        public string ReportPath { get; set; }

        public GridExportHelper()
        {
            this.Report = new GridppReportServer();
        }

        public GridExportHelper(string reportPath) : this()
        {
            this.ReportPath = reportPath;
        }

        /// <summary>
        /// 导出PDF
        /// </summary>
        /// <typeparam name="T">列表对象类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="filePath">存储路径</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool ExportPdf<T>(List<T> list, string filePath, HttpContextBase context)
        {
            //从对应文件中载入报表模板数据
            Report.LoadFromFile(this.ReportPath);

            //构建一个合法格式的对象，进行序列化
            var table = new
            {
                Table = list
            };
            var json = JsonConvert.SerializeObject(table, Formatting.Indented);
            Report.LoadDataFromXML(json);

            IGRExportOption ExportOption = Report.PrepareExport(GRExportType.gretPDF);
            var exportPdf = Report.ExportToBinaryObject();
            Report.UnprepareExport();

            var succeeded = exportPdf.SaveToFile(filePath);
            return succeeded;
        }

        /// <summary>
        /// 导出PDF
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filePath">存储路径</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool ExportPdf(object obj, string filePath, HttpContextBase context)
        {
            //从对应文件中载入报表模板数据
            Report.LoadFromFile(this.ReportPath);

            //构建一个合法格式的对象，进行序列化
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Report.LoadDataFromXML(json);

            IGRExportOption ExportOption = Report.PrepareExport(GRExportType.gretPDF);
            var exportPdf = Report.ExportToBinaryObject();
            Report.UnprepareExport();

            var succeeded = exportPdf.SaveToFile(filePath);
            return succeeded;
        }

        /// <summary>
        /// 导出PDF
        /// </summary>
        /// <typeparam name="T">列表对象类型</typeparam>
        /// <param name="list">列表对象</param>
        /// <param name="filePath">存储路径</param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool ExportPdf(string json, string filePath, HttpContextBase context)
        {
            //从对应文件中载入报表模板数据
            Report.LoadFromFile(this.ReportPath);

            //加载JSON对象
            Report.LoadDataFromXML(json);

            IGRExportOption ExportOption = Report.PrepareExport(GRExportType.gretPDF);
            var exportPdf = Report.ExportToBinaryObject();
            Report.UnprepareExport();

            var succeeded = exportPdf.SaveToFile(filePath);
            return succeeded;
        }

        public void Dispose()
        {
            if(this.Report != null)
            {
                this.Report.Clear();
                Marshal.ReleaseComObject(this.Report);
            }
        }
    }
}