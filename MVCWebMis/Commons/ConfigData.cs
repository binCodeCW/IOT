namespace IOT.MVCWebMis.Controllers
{
    /// <summary>
    /// 系统定义的一些常用变量
    /// </summary>
    public class ConfigData
    {
        /// <summary>
        /// 权限管理模块的BLL所在程序集名称
        /// </summary>
        public const string SecurityAssembly = "WHC.Security.Core";

        /// <summary>
        /// Web开发框架所属的系统类型定义
        /// </summary>
        public const string SystemType = "Bootstrap";

        /// <summary>
        /// 配置视图的样式布局
        /// 修改此处的时候，记得把对应的布局文件，如_Layout1.cshtml内容复制到_Layout.cshtml这个标准模板里面
        /// </summary>
        public static LayoutType Layout = LayoutType.Layout;
    }

    /// <summary>
    /// 布局样式枚举
    /// </summary>
    public enum LayoutType
    {
        Layout,
        Layout2,
        Layout3
    }
}