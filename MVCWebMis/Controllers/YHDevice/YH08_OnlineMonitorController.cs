using System.Web.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using YH.Pager.Entity;
using IOT.MVCWebMis.BLL;
using IOT.MVCWebMis.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class YH08_OnlineMonitorController : BusinessController<YH08_DeviceDataRec, YH08_DeviceDataRecInfo>
    {
        public YH08_OnlineMonitorController() : base()
        {
        }


        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();
            List<YH08_DeviceDataRecInfo> list = null;
            if (sort != null && !string.IsNullOrEmpty(sort.SortName))
            {
                list = baseBLL.FindWithPager(where, pagerInfo, sort.SortName, sort.IsDesc);
            }
            else
            {
                list = baseBLL.FindWithPager(where, pagerInfo);
            }

            //转换ExpandoObject对象列表
            var objList = ConvertObjectList(list);

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = objList }; //如果使用转义，请使用objList对象
            return ToJsonContent(result);
        }

        /// <summary>
        /// 转换集合为ExpandoObject集合,便于Web界面显示
        /// </summary>
        /// <param name="list">对象列表</param>
        /// <returns></returns>
        private List<ExpandoObject> ConvertObjectList(List<YH08_DeviceDataRecInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (YH08_DeviceDataRecInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.Timestamp = info.Timestamp;
                obj.Stamptime = info.Stamptime;
                obj.Appid = info.Appid;
                obj.Serviceid = info.Serviceid;
                obj.Deviceid = info.Deviceid;
                obj.Usagecount = info.Usagecount;
                obj.Dob = info.Dob;
                obj.M_lC12B = info.M_lC12B;
                obj.M_lC12S = info.M_lC12S;
                obj.CheckChannel = info.CheckChannel;
                obj.Cbc12 = info.Cbc12;
                obj.Csc12 = info.Csc12;
                obj.Cbc13 = info.Cbc13;
                obj.Csc13 = info.Csc13;
                obj.Recordtime = info.Recordtime;
                obj.Deviceno = info.Deviceno;
                //参考转义代码
                //obj.Name = BLLFactory<YH08_DeviceDataRec>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }
    }
}
