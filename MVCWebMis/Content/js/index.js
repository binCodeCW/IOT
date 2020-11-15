$(function() {
    var map = new BMap.Map('container');
    // 地图自定义样式
    map.setMapStyle({
        styleJson: [{
            "featureType": "water",
            "elementType": "all",
            "stylers": {
                "color": "#044161"
            }
        }, {
            "featureType": "land",
            "elementType": "all",
            "stylers": {
                "color": "#091934"
            }
        }, {
            "featureType": "boundary",
            "elementType": "geometry",
            "stylers": {
                "color": "#064f85"
            }
        }, {
            "featureType": "railway",
            "elementType": "all",
            "stylers": {
                "visibility": "off"
            }
        }, {
            "featureType": "highway",
            "elementType": "geometry",
            "stylers": {
                "color": "#004981"
            }
        }, {
            "featureType": "highway",
            "elementType": "geometry.fill",
            "stylers": {
                "color": "#005b96",
                "lightness": 1
            }
        }, {
            "featureType": "highway",
            "elementType": "labels",
            "stylers": {
                "visibility": "on"
            }
        }, {
            "featureType": "arterial",
            "elementType": "geometry",
            "stylers": {
                "color": "#004981",
                "lightness": -39
            }
        }, {
            "featureType": "arterial",
            "elementType": "geometry.fill",
            "stylers": {
                "color": "#00508b"
            }
        }, {
            "featureType": "poi",
            "elementType": "all",
            "stylers": {
                "visibility": "off"
            }
        }, {
            "featureType": "green",
            "elementType": "all",
            "stylers": {
                "color": "#056197",
                "visibility": "off"
            }
        }, {
            "featureType": "subway",
            "elementType": "all",
            "stylers": {
                "visibility": "off"
            }
        }, {
            "featureType": "manmade",
            "elementType": "all",
            "stylers": {
                "visibility": "off"
            }
        }, {
            "featureType": "local",
            "elementType": "all",
            "stylers": {
                "visibility": "off"
            }
        }, {
            "featureType": "arterial",
            "elementType": "labels",
            "stylers": {
                "visibility": "off"
            }
        }, {
            "featureType": "boundary",
            "elementType": "geometry.fill",
            "stylers": {
                "color": "#029fd4"
            }
        }, {
            "featureType": "building",
            "elementType": "all",
            "stylers": {
                "color": "#1a5787"
            }
        }, {
            "featureType": "label",
            "elementType": "all",
            "stylers": {
                "visibility": "off"
            }
        }, {
            "featureType": "poi",
            "elementType": "labels.text.fill",
            "stylers": {
                "color": "#ffffff"
            }
        }, {
            "featureType": "poi",
            "elementType": "labels.text.stroke",
            "stylers": {
                "color": "#1e1c1c"
            }
        }, {
            "featureType": "administrative",
            "elementType": "labels",
            "stylers": {
                "visibility": "on"
            }
        }, {
            "featureType": "road",
            "elementType": "labels",
            "stylers": {
                "visibility": "off"
            }
        }]
    });
    // 创建地图实例
    var point = new BMap.Point(116.404, 39.915);
    // 创建点坐标
    map.centerAndZoom(point, 5);
    // 初始化地图， 设置中心点坐标和地图级别
    var marker = new BMap.Marker(point);
    map.enableScrollWheelZoom(true); //启用滚轮放大缩小
    //地图、卫星、混合模式切换
    map.addControl(new BMap.MapTypeControl({
        mapTypes: [BMAP_NORMAL_MAP, BMAP_SATELLITE_MAP, BMAP_HYBRID_MAP]
    }));
    //向地图中添加缩放控件
    var ctrlNav = new window.BMap.NavigationControl({
        anchor: BMAP_ANCHOR_TOP_LEFT,
        type: BMAP_NAVIGATION_CONTROL_LARGE
    });
    map.addControl(ctrlNav);
    //向地图中添加缩略图控件
    var ctrlOve = new window.BMap.OverviewMapControl({
        anchor: BMAP_ANCHOR_BOTTOM_RIGHT,
        isOpen: 1
    });
    map.addOverlay(marker);
})