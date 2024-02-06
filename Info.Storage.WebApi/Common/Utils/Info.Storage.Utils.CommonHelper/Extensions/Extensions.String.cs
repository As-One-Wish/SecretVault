namespace Info.Storage.Utils.CommonHelper.Extensions
{
    /// <summary>
    /// string 扩展类
    /// </summary>
    public static partial class Extensions
    {
        #region 判断字符串是否为空

        /// <summary>
        /// 是否空或者空白字符串
        /// </summary>
        /// <param name="this"></param>
        /// <returns>bool</returns>
        public static bool IsNullOrWhiteSpace(this string @this) => string.IsNullOrWhiteSpace(@this);

        #endregion 判断字符串是否为空

        #region 字符串截取

        /// <summary>
        /// 从当前字符串开头移除另一字符串，不区分大小写，循环多次匹配前缀
        /// </summary>
        /// <param name="this">当前字符串</param>
        /// <param name="starts">另一字符串</param>
        /// <returns>string</returns>
        public static string TrimStart(this string @this, params string[] starts)
        {
            if (@this.IsNullOrWhiteSpace()) return @this;
            if (starts == null || starts.Length < 1 || starts[0].IsNullOrWhiteSpace()) return @this;
            for (int i = 0; i < starts.Length; i++)
            {
                if (@this.StartsWith(starts[i], StringComparison.OrdinalIgnoreCase))
                {
                    @this = @this.Substring(starts[i].Length);
                    if (@this.IsNullOrWhiteSpace()) break;
                    // 从头开始
                    i = -1;
                }
            }
            return @this;
        }

        /// <summary>
        /// 从当前字符串结尾移除另一字符串，不区分大小写，循环多次匹配前缀
        /// </summary>
        /// <param name="this">当前字符串</param>
        /// <param name="ends">另一字符串</param>
        /// <returns>string</returns>
        public static string TrimEnd(this string @this, params string[] ends)
        {
            if (@this.IsNullOrWhiteSpace()) return @this;
            if (ends == null || ends.Length < 1 || ends[0].IsNullOrWhiteSpace()) return @this;
            for (int i = 0; i < ends.Length; i++)
            {
                if (@this.EndsWith(ends[i], StringComparison.OrdinalIgnoreCase))
                {
                    @this = @this.Substring(0, @this.Length - ends[i].Length);
                    if (@this.IsNullOrWhiteSpace()) break;
                    // 从头开始
                    i = -1;
                }
            }
            return @this;
        }

        /// <summary>
        /// 从分割符开始向尾部截取字符串
        /// </summary>
        /// <param name="this">源字符串</param>
        /// <param name="separator">分隔符</param>
        /// <param name="lastIndexOf">true:从最后一个匹配的分隔符开始截取;false:从第一个匹配的分隔符开始截取,默认为true</param>
        /// <returns>string</returns>
        public static string Substring(this string @this, string separator, bool lastIndexOf = true)
        {
            int start = (lastIndexOf ? @this.LastIndexOf(separator) : @this.IndexOf(separator)) + separator.Length;
            int length = @this.Length - start;
            return @this.Substring(start, length);
        }

        /// <summary>
        /// 根据开始和结束字符串截取字符串
        /// </summary>
        /// <param name="this">源字符串</param>
        /// <param name="begin">开始字符串</param>
        /// <param name="end">结束字符串</param>
        /// <param name="beginIsIndexOf">开始字符串是否是IndexOf，默认为true，否则LastIndexOf</param>
        /// <param name="endIsIndexOf">结束字符串是否是IndexOf，默认为true，否则LastIndexOf</param>
        /// <returns>string</returns>
        public static string Substring(this string @this, string begin, string end, bool beginIsIndexOf = true, bool endIsIndexOf = true)
        {
            if (@this.IsNullOrWhiteSpace() || begin.IsNullOrWhiteSpace() || end.IsNullOrWhiteSpace()) return "";

            int beginIndex = beginIsIndexOf ? @this.IndexOf(begin) : @this.LastIndexOf(begin);
            if (beginIndex == -1) return "";
            beginIndex += begin.Length;

            int endIndex = endIsIndexOf ? @this.IndexOf(end) : @this.LastIndexOf(end);
            if (endIndex == -1) return "";

            return @this.Substring(beginIndex, endIndex - beginIndex);
        }

        #endregion 字符串截取
    }
}