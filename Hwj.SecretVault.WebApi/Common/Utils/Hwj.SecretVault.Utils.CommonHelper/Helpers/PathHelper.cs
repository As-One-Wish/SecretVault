using Hwj.SecretVault.Utils.CommonHelper.Extensions;
using System;
using System.IO;

namespace Hwj.SecretVault.Utils.CommonHelper.Helpers
{
    /// <summary>
    /// 路径工具类
    /// </summary>
    public static class PathHelper
    {
        #region 属性

#if __CORE__ // 根据条件编译选择了不同的代码块。这是为了处理在 .NET Core/.NET 5+ 和传统 .NET Framework 中获取应用程序基目录的方式不同的问题
        public static string BaseDirectory { get; set; } = AppContext.BaseDirectory;
#else
        public static string BaseDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
#endif

        #endregion 属性

        #region 路径操作辅助

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private static string GetAbsolutePath(string path, int mode)
        {
            // 处理路径分隔符，兼容Windows和Linux
            char sep = Path.DirectorySeparatorChar;
            char sep2 = sep == '/' ? '\\' : '/';
            path = path.Replace(sep2, sep);

            string dir = "";
            switch (mode)
            {
                case 1:
                    dir = BaseDirectory; break;
                case 2:
#if __CORE__ // 已弃用
                    dir = AppContext.BaseDirectory; // .NET5及更高版本中
#else
                    dir = AppDomain.CurrentDomain.BaseDirectory; // 传统 .NET Framework
#endif
                    break;
#if !__CORE__
                case 3:
                    dir = Environment.CurrentDirectory; break;
#endif
                default:
                    break;
            }

            if (dir.IsNullOrWhiteSpace()) return Path.GetFullPath(path);
            // 处理网络路径
            if (path.StartsWith(@"\\")) return Path.GetFullPath(path);

            if (/*path[0]==sep||*/path[0] == sep2 || !Path.IsPathRooted(path))
            {
                path = path.TrimStart('~');
                path = path.TrimStart(sep);
                path = Path.Combine(dir, path);
            }

            return Path.GetFullPath(path);
        }

        /// <summary>
        /// 获取文件或目录的全路径，过滤相对目录
        /// </summary>
        /// <remarks>
        /// 不确保目录后面一定有分隔符，是否有分隔符由原始路径末尾决定
        /// </remarks>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFullPath(this string path)
        {
            if (path.IsNullOrWhiteSpace()) return path;
            return GetAbsolutePath(path, 1);
        }

        /// <summary>
        /// 获取文件和目录基于应用程序域及目录的全路径，过滤相对目录
        /// </summary>
        /// <remarks>
        /// 不确保目录后面一定有分隔符，是否有分隔符由原始路径末尾决定
        /// </remarks>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetBasePath(this string path)
        {
            if (path.IsNullOrWhiteSpace()) return path;
            return GetAbsolutePath(path, 2);
        }

        /// <summary>
        /// 获取物理路径
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <returns>物理绝对路径</returns>
        public static string GetPhysicalPath(this string path)
        {
            string physicalPath = BaseDirectory;
            if (!path.IsNullOrWhiteSpace())
            {
                path = path.Replace("~", "").Replace("/", @"\").TrimStart('\\').TrimEnd('\\');
                physicalPath = Path.Combine(physicalPath, path.Substring("\\").Contains(".") ? path : path + @"\");
            }

            return physicalPath;
        }

        #endregion 路径操作辅助
    }
}