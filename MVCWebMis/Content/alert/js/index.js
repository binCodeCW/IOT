var scn_data = {
    deviceTotal: 0,
    alarm: { alarm: 0, fault: 0 },
    dtu: { on: 150, off: 150 },
    plc: { on: 10, off: 10 },
    industy: { v1: 10, v2: 11, v3: 12, v3: 14, v4: 15, v5: 17, v6: 18 },
    online: { v1: 10, v2: 11, v3: 12, v3: 14, v4: 15, v5: 17, v6: 18 },
    almMsg: [{ msg: "2017年5月4日市A区12#机器气压过高报警" },
        { msg: "上海市A区12#机器气压过高报警" },
        { msg: "江苏省12#机器气压过高报警" },
        { msg: "河南省郑州市B区12#机器气压过高报警" },
        { msg: "河南省郑州市B区12#机器气压过高报警" },
    ],
    deviceAlarmInfo: ["1", "2", "3", "4", "5"],
    msgCnt: [{ msg: 100, alm: 20 },
        { msg: 200, alm: 40 },
        { msg: 300, alm: 50 },
        { msg: 400, alm: 35 },
        { msg: 400, alm: 40 },
        { msg: 400, alm: 11 },
        { msg: 400, alm: 66 },
        { msg: 100, alm: 77 },
        { msg: 200, alm: 88 },
        { msg: 300, alm: 22 },
        { msg: 400, alm: 99 },
        { msg: 400, alm: 100 },
        { msg: 400, alm: 111 },
        { msg: 400, alm: 222 },
        { msg: 100, alm: 333 },
        { msg: 200, alm: 11 },
        { msg: 300, alm: 33 },
        { msg: 400, alm: 55 },
        { msg: 400, alm: 77 },
        { msg: 400, alm: 90 }
    ],
    map: [{ area: "山东", cnt: 20 },
        { area: "浙江", cnt: 40 },
        { area: "江苏", cnt: 50 },
        { area: "辽宁", cnt: 50 }
    ],
    factoryHeader: [
        { "categories": "单位名" },
        { "categories": "网关数" },
        { "categories": "设备数" },
        { "categories": "数据点" },
        { "categories": "报警" },
        { "categories": "操作" }
    ],
    factory: [
        { "company": "宝钢", "dtuCnt": 200, "plcCnt": 400, "dataCnt": 5000, "alarm": "无" },
        { "company": "造纸厂", "dtuCnt": 3000, "plcCnt": 2000, "dataCnt": 1000, "alarm": "无" },
        { "company": "锅炉厂", "dtuCnt": 1500, "plcCnt": 1000, "dataCnt": 500, "alarm": "无" },
        { "company": "锅炉二厂", "dtuCnt": 1500, "plcCnt": 300, "dataCnt": 1200, "alarm": "温度上限报警>120" },
        { "company": "锅炉三厂", "dtuCnt": 1000, "plcCnt": 800, "dataCnt": 200, "alarm": "无" },
        { "company": "锅炉三厂", "dtuCnt": 1000, "plcCnt": 800, "dataCnt": 200, "alarm": "无" },
        { "company": "锅炉三厂", "dtuCnt": 1000, "plcCnt": 800, "dataCnt": 200, "alarm": "无" },
        { "company": "锅炉三厂", "dtuCnt": 1000, "plcCnt": 800, "dataCnt": 200, "alarm": "无" },
        { "company": "锅炉三厂", "dtuCnt": 1000, "plcCnt": 800, "dataCnt": 200, "alarm": "无" },
        { "company": "锅炉三厂", "dtuCnt": 1000, "plcCnt": 800, "dataCnt": 200, "alarm": "无" }
    ],
    last7dayDeviceAlarm: {
        series: [{ name: "line", data: [] }, { name: "报警数", data: [] }, { name: "处理数", data: [] }]
    }
};
var vm = new Vue({
    el: '#content',
    data: scn_data,
    mounted: function () {
        var that = this;
        this.loadDeviceTotal();
        this.loadDeviceRatio();
        this.loadDeviceTypeReport();
        this.loadAlarm();
        this.loadDeviceAlarmInfo();
        this.loadLast7DayDeviceAlarmInfo();
        if (this.timer) {
            clearInterval(this.timer);
        } else {
            this.timer = setInterval(function () {
                var now = new Date().toLocaleString();
                console.log("10秒输出一次:" + now);
                that.loadDeviceTotal();
                that.loadDeviceRatio();
                that.loadDeviceTypeReport();
                that.loadAlarm();
                that.loadDeviceAlarmInfo();
                that.loadLast7DayDeviceAlarmInfo();
            }, 10 * 1000);
        }
    },
    destroyed: function () {
        clearInterval(this.timer);
    },
    methods: {
        details: function () {

        },
        loadDeviceTotal: function (f) {
            // 加载设备总数
            var that = this;
            var url = "/YH_DeviceInfo/FindWithPager?rows=1&page=1&sortOrder=asc";
            $.getJSON(url, function (res) {
                console.log("设备总数数据: %o", res || 0);
                that.deviceTotal = res.total || 0;
            });
        },
        //
        loadDeviceRatio: function () {
            // 加载设备概况(在线离线比例)
            var that = this;
            var url = "/YH_DeviceInfo/FindWithPager?rows=1&page=1&sortOrder=asc&WHC_OLINESTATUS=true";
            $.getJSON(url, function (res) {
                var offlineUrl = "/YH_DeviceInfo/FindWithPager?rows=1&page=1&sortOrder=asc&WHC_OLINESTATUS=false";
                $.getJSON(offlineUrl, function (offlineRes) {
                    var data = [];
                    data.push({ device_status: "offline", device_total: offlineRes.total || 0 });
                    data.push({ device_status: "online", device_total: res.total || 0 });
                    console.log("设备概况数据: %o", data);
                    that.refreshChart1(data);
                });
            });
        },
        //
        refreshChart1: function (data) {
            // 基于准备好的dom，初始化echarts实例
            var myChart = echarts.getInstanceByDom(document.getElementById("chart_1")) || echarts.init(document.getElementById('chart_1'));
            var option = {
                title: {
                    text: '设备状态统计',
                    top: 35,
                    left: 20,
                    textStyle: {
                        fontSize: 18,
                        color: '#fff'
                    }
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b}: {c} ({d}%)",

                },
                legend: {
                    right: 20,
                    top: 35,
                    data: ['离线', '在线'],
                    textStyle: {
                        color: '#fff'
                    }
                },
                series: [{
                    name: '设备状态',
                    type: 'pie',
                    radius: ['0', '60%'],
                    center: ['50%', '60%'],
                    color: ['#708090', '#4682B4', '#2ca3fd'],
                    label: {
                        normal: {
                            formatter: '{b}\n{d}%'
                        },

                    },
                    data: [{
                        value: 60,
                        name: '离线'
                    },
                    {
                        value: 150,
                        name: '在线',
                        selected: true
                    }
                    ]
                }]
            };
            option.series[0].data = [];
            $.each(data, function (index, item) {
                option.series[0].data.push({
                    value: item.device_total,
                    name: item.device_status == "online" ? "在线" : "离线"
                });
            });
            // 使用刚指定的配置项和数据显示图表。
            myChart.setOption(option);
            window.addEventListener("resize", function () {
                myChart.resize();
            });
        },
        loadDeviceTypeReport: function () {
            // 加载设备类型统计数据
            var url = "/YH_DeviceInfo/DeviceTypeReport";
            $.getJSON(url, function (res) {
                console.log("设备类型统计数据: %o", res);
                var html = "";
                $.each(res, function (index, item) {
                    html += "<tr>";
                    html += "    <td>" + item.DEVICENAME || '' + "</td>";
                    html += "    <td>" + item.DEVICETYPE || '' + "</td>";
                    html += "    <td>" + item.ONLINETOTAL || 0 + "</td>";
                    html += "    <td>" + item.LASTTIME || '' + "</td>";
                    html += "</tr>";
                });

                // 先清空后加载
                $("#tList").empty();
                $("#tList").append(html);
            });
        },
        loadAlarm: function () {
            var that = this;
            var url = "/YH_DeviceAlarm/FindWithPager?rows=1&page=1&sortOrder=asc";
            $.getJSON(url, function (res) {
                console.log("报警统计数据: %o", res);
                that.alarm.alarm = res.total || 0;
            });
        },
        loadDeviceAlarmInfo: function () {
            var that = this;
            var url = "/YH_AlarmMap/DeviceAlarmInfoReport";
            $.getJSON(url, function (res) {
                console.log("近期报警数据: %o", res);
                var arr = [];
                $.each(res, function (index, item) {
                    // 记录时间 仪器编号 仪器类型 报警信息
                    //arr.push("".concat(item.recordtime.split(" ")[0], " ", item.deviceno, " ", item.DeviceTypeName, " ", item.ErrorText));
                    // 记录时间 仪器编号 报警信息
                    arr.push("".concat(item.recordtime.split(" ")[0], " ", item.deviceno, " ", item.ErrorText));
                })
                that.deviceAlarmInfo = arr;
            });
        },
        loadLast7DayDeviceAlarmInfo: function () {
            var that = this;
            var url = "/YH_DeviceAlarm/Last7DayDeviceAlarmInfo"
            $.getJSON(url, function (res) {
                console.log("近7天数据: %o", res);
                var myChart = echarts.getInstanceByDom(document.getElementById("container4")) || echarts.init(document.getElementById("container4"));
                var category = [];
                var lineData = [];
                var barData = [];
                $.each(res, function (index, item) {
                    category.push(item.RECORDDATE);
                    lineData.push(item.ERRORNUM);
                    barData.push(1);
                });
                var options = {
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'shadow'
                        }
                    },
                    legend: {
                        data: ['处理数', '报警数'],
                        textStyle: {
                            color: '#ccc'
                        }
                    },
                    xAxis: {
                        data: category,
                        axisLine: {
                            lineStyle: {
                                color: '#ccc'
                            }
                        }
                    },
                    yAxis: {
                        splitLine: { show: false },
                        axisLine: {
                            lineStyle: {
                                color: '#ccc'
                            }
                        }
                    },
                    series: [{
                        name: '报警数',
                        type: 'line',
                        smooth: true,
                        showAllSymbol: true,
                        symbol: 'emptyCircle',
                        symbolSize: 15,
                        data: lineData
                    }, {
                        name: '处理数',
                        type: 'bar',
                        barWidth: 10,
                        itemStyle: {
                            normal: {
                                barBorderRadius: 5,
                                color: new echarts.graphic.LinearGradient(
                                    0, 0, 0, 1,
                                    [
                                        { offset: 0, color: '#14c8d4' },
                                        { offset: 1, color: '#43eec6' }
                                    ]
                                )
                            }
                        },
                        data: barData
                    }]
                }
                myChart.setOption(options, true);
            })
        }
    }
})