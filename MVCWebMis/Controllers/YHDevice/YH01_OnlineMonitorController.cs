using System.Web.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using YH.Pager.Entity;
using IOT.MVCWebMis.BLL;
using IOT.MVCWebMis.Entity;

namespace IOT.MVCWebMis.Controllers
{
    public class YH01_OnlineMonitorController : BusinessController<YH01_DeviceDataRec, YH01_DeviceDataRecInfo>
    {
        public YH01_OnlineMonitorController() : base()
        {
        }


        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var sort = GetSortOrder();
            List<YH01_DeviceDataRecInfo> list = null;
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
        private List<ExpandoObject> ConvertObjectList(List<YH01_DeviceDataRecInfo> list)
        {
            //如果需要修改字段显示，则参考下面代码处理
            List<ExpandoObject> objList = new List<ExpandoObject>();
            foreach (YH01_DeviceDataRecInfo info in list)
            {
                dynamic obj = new ExpandoObject();

                obj.ID = info.ID;
                obj.Timestamp = info.Timestamp;
                obj.Stamptime = info.Stamptime;
                obj.Appid = info.Appid;
                obj.Serviceid = info.Serviceid;
                obj.Deviceid = info.Deviceid;
                obj.Cpm = info.Cpm;
                obj.Bendi = info.Bendi;
                obj.Position = info.Position;
                obj.Efficiency = info.Efficiency;
                obj.Time = info.Time;
                obj.Recordtime = info.Recordtime;
                obj.DeviceNo = info.DeviceNo;
                //参考转义代码
                //obj.Name = BLLFactory<YH01_DeviceDataRec>.Instance.GetNameByID(info.ID);

                objList.Add(obj);
            }

            return objList;
        }
    }
}
