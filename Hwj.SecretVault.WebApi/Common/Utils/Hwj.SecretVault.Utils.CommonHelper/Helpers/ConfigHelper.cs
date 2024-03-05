using Microsoft.Extensions.Configuration;
using System.IO;

namespace Info.Storage.Utils.CommonHelper.Helpers
{
    /// <summary>
    /// Config 工具类
    /// </summary>
    public static class ConfigHelper
    {
        #region 公有属性

        /// <summary>
        /// App配置
        /// </summary>
        public static IConfigurationRoot Configuration { get; private set; }

        #endregion 公有属性

        #region 构造函数

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ConfigHelper()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // 需要安装 Microsoft.AspNetCore 包
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        #endregion 构造函数
    }
}