using System.Collections.Generic;
using System.Web.Optimization;

namespace IOT.MVCWebMis
{
    /// <summary>
    /// BundleConfig用来将js和css进行合并与压缩，（多个文件可以打包成一个文件），并且可以区分调试和非调试。
    /// 在调试时不进行压缩，以原始方式显示出来，以方便查找问题。
    /// </summary>
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var Layout = IOT.MVCWebMis.Controllers.ConfigData.Layout.ToString();//可选值: layout,layout2,layout3

            //为了减少太多的Bundles命名，定义的CSS的Bundle为："~/Content/css"、"~/Content/jquerytools"
            //定义的Script的Bundles为："~/bundles/jquery"、"~/bundles/jquerytools"
            StyleBundle css_metronic = new StyleBundle("~/metronic/css");

            //开始全局必需样式引用
            css_metronic.Include("~/Content/metronic/assets/global/plugins/font-awesome/css/font-awesome.min.css",
               "~/Content/metronic/assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
               "~/Content/metronic/assets/global/plugins/bootstrap/css/bootstrap.min.css",
               "~/Content/metronic/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css",

            #region 样式文件
               //开启页面样式引用
               "~/Content/metronic/assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css",
               "~/Content/metronic/assets/global/plugins/bootstrap-daterangepicker/daterangepicker.min.css",
               "~/Content/metronic/assets/global/plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css",
               "~/Content/metronic/assets/global/plugins/morris/morris.css",
               "~/Content/metronic/assets/global/plugins/fullcalendar/fullcalendar.min.css",
               //"~/Content/metronic/assets/global/plugins/jqvmap/jqvmap/jqvmap.css",

               //其他页面样式
               "~/Content/metronic/assets/global/plugins/bootstrap-select/css/bootstrap-select.min.css",
               "~/Content/metronic/assets/global/plugins/select2/css/select2.min.css",
               "~/Content/metronic/assets/global/plugins/select2/css/select2-bootstrap.min.css",
               "~/Content/metronic/assets/global/plugins/jquery-multi-select/css/multi-select.css",
               "~/Content/metronic/assets/global/plugins/icheck/skins/all.css",
               "~/Content/metronic/assets/global/plugins/bootstrap-sweetalert/sweetalert.css",
               "~/Content/metronic/assets/global/plugins/cubeportfolio/css/cubeportfolio.css",

               //引用bootstrap-table样式
               "~/Content/metronic/assets/global/plugins/bootstrap-table/bootstrap-table.min.css",

               "~/Content/metronic/assets/global/plugins/datatables/datatables.min.css",
               "~/Content/metronic/assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.css",
               //"~/Content/metronic/assets/global/plugins/bootstrap-modal/css/bootstrap-modal-bs3patch.css",
               //"~/Content/metronic/assets/global/plugins/bootstrap-modal/css/bootstrap-modal.css",
               "~/Content/metronic/assets/global/plugins/jstree/dist/themes/default/style.min.css",

               //主题全局样式
               "~/Content/metronic/assets/global/css/components-rounded.css",
               "~/Content/metronic/assets/global/css/plugins.min.css",
               //主题布局样式
               "~/Content/metronic/assets/layouts/" + Layout + "/css/layout.css",
               "~/Content/metronic/assets/layouts/" + Layout + "/css/themes/default.min.css",
               "~/Content/metronic/assets/layouts/" + Layout + "/css/custom.min.css"

               //增加自定义图标样式
               //"~/Content/icons-customed/16/icon.css",
               //"~/Content/icons-customed/24/icon.css",
               //"~/Content/icons-customed/32/icon.css" 
            #endregion
               );

            ScriptBundle js_metronic = new ScriptBundle("~/metronic/js");
            js_metronic.Orderer = new AsIsBundleOrderer();//按添加先后次序排列，否则容易出现：Uncaught ReferenceError: jQuery is not defined
            js_metronic.Include(
                //核心JS插件
                "~/Content/metronic/assets/global/plugins/bootstrap/js/bootstrap.min.js",
                //"~/Content/metronic/assets/global/plugins/js.cookie.min.js", //页面已加
                "~/Content/metronic/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",//
                "~/Content/metronic/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/Content/metronic/assets/global/plugins/jquery.blockui.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",

            #region 脚本文件
                //页面级JS插件
                "~/Content/metronic/assets/global/plugins/moment.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-datepicker/locales/bootstrap-datepicker.zh-CN.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-daterangepicker/daterangepicker.min.js",
               "~/Content/metronic/assets/global/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js",
               "~/Content/metronic/assets/global/plugins/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN.js",
                "~/Content/metronic/assets/global/plugins/morris/morris.min.js",
                "~/Content/metronic/assets/global/plugins/morris/raphael-min.js",
                "~/Content/metronic/assets/global/plugins/counterup/jquery.waypoints.min.js",
                "~/Content/metronic/assets/global/plugins/counterup/jquery.counterup.min.js",
                "~/Content/metronic/assets/global/plugins/fullcalendar/fullcalendar.min.js",
                "~/Content/metronic/assets/global/plugins/flot/jquery.flot.min.js",
                "~/Content/metronic/assets/global/plugins/flot/jquery.flot.resize.min.js",
                "~/Content/metronic/assets/global/plugins/flot/jquery.flot.categories.min.js",
                "~/Content/metronic/assets/global/plugins/jquery-easypiechart/jquery.easypiechart.min.js",
                "~/Content/metronic/assets/global/plugins/jquery.sparkline.min.js",
                //"~/Content/metronic/assets/global/plugins/jqvmap/jqvmap/jquery.vmap.js",
                //"~/Content/metronic/assets/global/plugins/jqvmap/jqvmap/maps/jquery.vmap.russia.js",
                //"~/Content/metronic/assets/global/plugins/jqvmap/jqvmap/maps/jquery.vmap.world.js",
                //"~/Content/metronic/assets/global/plugins/jqvmap/jqvmap/maps/jquery.vmap.europe.js",
                //"~/Content/metronic/assets/global/plugins/jqvmap/jqvmap/maps/jquery.vmap.germany.js",
                //"~/Content/metronic/assets/global/plugins/jqvmap/jqvmap/maps/jquery.vmap.usa.js",
                //"~/Content/metronic/assets/global/plugins/jqvmap/jqvmap/data/jquery.vmap.sampledata.js",

                "~/Content/metronic/assets/global/plugins/fuelux/js/spinner.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-touchspin/bootstrap.touchspin.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-select/js/bootstrap-select.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-select/js/i18n/defaults-zh_CN.min.js",
                "~/Content/metronic/assets/global/plugins/select2/js/select2.full.min.js",
                "~/Content/metronic/assets/global/plugins/select2/js/i18n/zh-CN.js",
                "~/Content/metronic/assets/global/plugins/jquery-multi-select/js/jquery.multi-select.js",

                "~/Content/metronic/assets/global/plugins/datatables/datatables.min.js",
                "~/Content/metronic/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js",

                //引用bootstrap-table
                "~/Content/metronic/assets/global/plugins/bootstrap-table/bootstrap-table.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-table/locale/bootstrap-table-zh-CN.min.js",

                "~/Content/metronic/assets/global/plugins/jquery-validation/js/jquery.validate.min.js",
                "~/Content/metronic/assets/global/plugins/jquery-validation/js/additional-methods.min.js",
                "~/Content/metronic/assets/global/plugins/jquery-validation/js/localization/messages_zh.min.js",

                "~/Content/metronic/assets/global/plugins/bootbox/bootbox.min.js",
                //"~/Content/metronic/assets/global/plugins/bootstrap-modal/js/bootstrap-modalmanager.js",
                //"~/Content/metronic/assets/global/plugins/bootstrap-modal/js/bootstrap-modal.js",

                "~/Content/metronic/assets/global/plugins/jstree/dist/jstree.min.js",
                "~/Content/metronic/assets/global/plugins/icheck/icheck.min.js",
                "~/Content/metronic/assets/global/plugins/bootstrap-sweetalert/sweetalert.min.js",
                "~/Content/metronic/assets/global/plugins/cubeportfolio/js/jquery.cubeportfolio.min.js",

                "~/Content/metronic/assets/global/scripts/app.min.js",
                "~/Content/metronic/assets/pages/scripts/dashboard.min.js",
                "~/Content/metronic/assets/pages/scripts/table-datatables-managed.min.js",
                "~/Content/metronic/assets/pages/scripts/components-select2.min.js",

                //主题样式脚本
                "~/Content/metronic/assets/layouts/" + Layout + "/scripts/layout.min.js",
                "~/Content/metronic/assets/layouts/" + Layout + "/scripts/demo.min.js",
                "~/Content/metronic/assets/layouts/global/scripts/quick-sidebar.min.js",
                "~/Content/metronic/assets/layouts/global/scripts/quick-nav.min.js"
            #endregion
            );

            //引入多Tab展示插件btabs
            //css_metronic.Include("~/Content/MyPlugins/bTabs/b.tabs.css");
            //js_metronic.Include("~/Content/MyPlugins/bTabs/b.tabs.min.js");

            //引用分页控件控件paginator
            js_metronic.Include("~/Content/MyPlugins/bootstrap-paginator/bootstrap-paginator.js");

            //引用消息提示控件toastr
            css_metronic.Include("~/Content/metronic/assets/global/plugins/bootstrap-toastr/toastr.min.css");
            js_metronic.Include("~/Content/metronic/assets/global/plugins/bootstrap-toastr/toastr.min.js");

            //引用消息提示控件jNotify
            css_metronic.Include("~/Content/JQueryTools/jNotify/jquery/jNotify.jquery.css");
            js_metronic.Include("~/Content/JQueryTools/jNotify/jquery/jNotify.jquery.js");

            //内置标签的tagsinput控件应用
            css_metronic.Include("~/Content/metronic/assets/global/plugins/bootstrap-tagsinput/bootstrap-tagsinput.css");
            js_metronic.Include("~/Content/metronic/assets/global/plugins/bootstrap-tagsinput/bootstrap-tagsinput.js");

            //Tag标签的控件应用
            css_metronic.Include("~/Content/JQueryTools/Tags-Input/jquery.tagsinput.css");
            js_metronic.Include("~/Content/JQueryTools/Tags-Input/jquery.tagsinput.js");

            //添加对uploadify控件的支持
            css_metronic.Include("~/Content/JQueryTools/uploadify/uploadify.css");
            js_metronic.Include("~/Content/JQueryTools/uploadify/jquery.uploadify.js");

            //添加LODOP控件支持
            js_metronic.Include("~/Content/JQueryTools/LODOP/CheckActivX.js");

            //添加对bootstrap-summernote的支持
            css_metronic.Include("~/Content/metronic/assets/global/plugins/bootstrap-summernote/summernote.css");
            js_metronic.Include("~/Content/metronic/assets/global/plugins/bootstrap-summernote/summernote.min.js");
            js_metronic.Include("~/Content/metronic/assets/global/plugins/bootstrap-summernote/lang/summernote-zh-CN.min.js");

            //添加对bootstrap-fileinput控件的支持
            css_metronic.Include("~/Content/MyPlugins/bootstrap-fileinput/css/fileinput.min.css");
            js_metronic.Include("~/Content/MyPlugins/bootstrap-fileinput/js/fileinput.min.js");
            js_metronic.Include("~/Content/MyPlugins/bootstrap-fileinput/js/locales/zh.js");

            //添加对fancybox控件的支持
            css_metronic.Include("~/Content/metronic/assets/global/plugins/fancybox/source/jquery.fancybox.css");
            css_metronic.Include("~/Content/metronic/assets/pages/css/portfolio.css");
            js_metronic.Include("~/Content/metronic/assets/global/plugins/jquery-mixitup/jquery.mixitup.min.js");
            js_metronic.Include("~/Content/metronic/assets/global/plugins/fancybox/source/jquery.fancybox.pack.js");

            bundles.Add(css_metronic);
            bundles.Add(js_metronic);

            //当进行css和JS压缩处理，不过会导致图片路径有问题，停用！！
            //BundleTable.EnableOptimizations = true;
        }
    }

    /// <summary>
    /// 自定义Bundles排序
    /// </summary>
    internal class AsIsBundleOrderer : IBundleOrderer
    {
        public virtual IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
    internal static class BundleExtensions
    {
        public static Bundle ForceOrdered(this Bundle sb)
        {
            sb.Orderer = new AsIsBundleOrderer();
            return sb;
        }
    }
}