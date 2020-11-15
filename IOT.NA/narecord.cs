using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT.NA
{
    public class narecord
    {
        public static NASDK currsdk = null;
        public static string tokenstring = "......";
        public static bool getTokennow(string ip, int port, string appid, string appsecret)
        {
            tokenstring = "......";
            currsdk = new NASDK(ip, port, appid, appsecret, "iot3rd.p12", "IoM@1234");
            TokenResult token = currsdk.getToken();
            if (token == null)
            {
                return false;
            }
            else
            {
                tokenstring = token.accessToken;
                return true;
            }
        }

        /// <summary>
        /// 获取历史记录，结构体 HistoryDataResult 
        /// </summary>
        /// <param name="ip">HTTPS接入方式IP</param>
        /// <param name="port">HTTPS接入方式端口</param>
        /// <param name="appid">项目应用ID</param>
        /// <param name="appsecret">项目应用密钥</param>
        /// <param name="deviceid">设备ID</param>
        /// <param name="serviceid">服务ID</param>
        /// <param name="pageno">页码，返回数据量多时有分页，默认0</param>
        /// <param name="pagesize">数据行数，和页码配合使用，建议数量大一点</param>
        /// <param name="startTime">查询的起始时间，格式 yyyyMMddTHHmmssZ</param>
        /// <param name="endTime">查询的中止时间，格式 yyyyMMddTHHmmssZ</param>
        /// <returns></returns>
        public static HistoryDataResult GetMsg(string ip, int port, string appid, string appsecret, string deviceid, string serviceid, int pageno, int pagesize, string startTime, string endTime)
        {
            if (getTokennow(ip, port, appid, appsecret))
                return currsdk.queryHistoryData(tokenstring, deviceid, serviceid, pageno, pagesize, startTime, endTime);
            else
                return null;
        }

        /*
        private void button1_Click(object sender, EventArgs e)
        {
            DataGridView dataGrid = new DataGridView();
            dataGrid.DataSource = GetMsg("180.101.147.89", 8743, "KZiFlwIzbN8XTqI8zzQP7Ag7Qcoa", "vpFfei3hES3CQcY4w5pRaLWkkHka", "72792c1b-bb70-498d-9a73-794649b380c7", "ConsumablesControl", 0, 1000, "19900101T000000Z", DateTime.Now.ToString("yyyyMMddTHHmmssZ")).deviceDataHistoryDTOs;            
            //统计所有行数 = GetMsg(...).totalCount.ToString();
            //txtHDPageNo.Text =GetMsg(...).pageNo.ToString();
            //txtHDPageSize.Text =GetMsg(...).pageSize.ToString();
            //dgvHisData.DataSource = GetMsg(...).deviceDataHistoryDTOs;
        }
        */
    }
}
