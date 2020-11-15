using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT.NA
{
    public class naorder
    {
        static string tokenstring = "......";
        static NASDK currsdk = null;
        /// <summary>
        /// 获取鉴权
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        /// <returns></returns>
        public static bool getTokennow(string ip, int port, string appid, string appsecret)
        {
            tokenstring = "......";
            currsdk = new NASDK(ip, port, appid, appsecret, "D:\\test88\\Bootstrap\\MVCWebMis\\bin\\iot3rd.p12", "IoM@1234");
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
        /// 指定设备发送命令
        /// </summary>
        /// <param name="ip">HTTPS接入方式IP</param>
        /// <param name="port">HTTPS接入方式端口</param>
        /// <param name="appid">项目应用ID</param>
        /// <param name="appsecret">项目应用密钥</param>
        /// <param name="serviceid">服务名称（如耗材管控等，详见平台profile）</param>
        /// <param name="deviceid">设备ID</param>
        /// <param name="cmd">命令名称</param>
        /// <param name="attribute">命令字段属性名称</param>
        /// <param name="ifint">上报数据是否是数字</param>
        /// <param name="data">上报数据内容</param>
        /// <returns>返回代码</returns>
        public static string sendOrder(string ip, int port, string appid, string appsecret, string serviceid, string deviceid, string cmd, string attribute, bool ifint, string data)
        {
            if (getTokennow(ip, port, appid, appsecret))
                return sendnow(cmd, attribute, ifint, data, deviceid, serviceid);
            else
                return "鉴权失败，请重试";
        }

        /// <summary>
        /// 获取鉴权后发送命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="attribute"></param>
        /// <param name="ifint"></param>
        /// <param name="data"></param>
        /// <param name="deviceid"></param>
        /// <param name="serviceid"></param>
        /// <returns></returns>
        public static string sendnow(string cmd, string attribute, bool ifint, string data, string deviceid, string serviceid)
        {
            List<CommandPara> lsCmdPars = new List<CommandPara>();

            List<string> lbCmdParas = new List<string>();
            lbCmdParas.Add(attribute + ":" + data + ":" + ifint.ToString());
            for (int i = 0; i < lbCmdParas.Count; i++)
            {
                string currPara = lbCmdParas[i].ToString();
                string[] paras = currPara.Split(':');
                CommandPara currCmdPara = new CommandPara();
                currCmdPara.isNum = Convert.ToBoolean(paras[2]);
                currCmdPara.paraName = paras[0];
                currCmdPara.paraValue = paras[1];
                lsCmdPars.Add(currCmdPara);
            }
            string result = currsdk.sendCommand(tokenstring, deviceid, "", "0", serviceid, cmd, lsCmdPars);
            if (result == null)
                return "获取失败，请看日志";
            else
                return result;
        }

        //  //MessageBox.Show(  sendOrder("180.101.147.89", 8743, "KZiFlwIzbN8XTqI8zzQP7Ag7Qcoa", "vpFfei3hES3CQcY4w5pRaLWkkHka",
        //  //       "ConsumablesControl", "72792c1b-bb70-498d-9a73-794649b380c7", "DEVICE_CONTROL", "deviceControl",true,"1"));
        //     MessageBox.Show(sendOrder("180.101.147.89", 8743, "KZiFlwIzbN8XTqI8zzQP7Ag7Qcoa", "vpFfei3hES3CQcY4w5pRaLWkkHka",
        //"ConsumablesControl", "72792c1b-bb70-498d-9a73-794649b380c7", "RESET_USAGE_COUNT", "resetUsageCount", true, "11"));
    
    }
}
