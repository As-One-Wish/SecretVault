using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using SysMD5 = System.Security.Cryptography.MD5;

namespace Info.Storage.Utils.CommonHelper.Helpers
{
    /// <summary>
    /// 加密解密静态工具类
    /// </summary>
    public static class CryptHelper
    {
        #region MD5加密

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="type">加密类型[16/32/46/64]，默认32位</param>
        /// <returns></returns>
        public static string MD5(string source, int type = 32)
        {
            string result = string.Empty;

            try
            {
                byte[] bytes = SysMD5.Create().ComputeHash(Encoding.UTF8.GetBytes(source));
                StringBuilder sb = new StringBuilder();
                foreach (var item in bytes)
                {
                    switch (type)
                    {
                        case 16:
                        case 32:
                            sb.Append(item.ToString("x2"));
                            break;

                        case 48:
                            sb.Append(item.ToString("x3"));
                            break;

                        case 64:
                            sb.Append(item.ToString("x4"));
                            break;
                    }
                }
                result = sb.ToString().ToUpper();
                if (type == 16) result = result.Substring(8, 16);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "MD5加密");
            }
            return result;
        }

        #endregion MD5加密

        #region 自定义加密/解密

        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="source">加密字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="IsMD5">密钥是否通过MD5再加密</param>
        /// <returns></returns>
        public static string Encrypt(string source, string key, bool IsMD5 = true)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                // 使用DES算法进行加密
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(source);

                key = IsMD5 ? MD5(key).Substring(0, 8) : key;
                des.Key = Encoding.UTF8.GetBytes(key);
                des.IV = Encoding.UTF8.GetBytes(key);

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                foreach (byte b in ms.ToArray())
                    result.AppendFormat("{0:X2}", b);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "自定义加密函数");
            }
            return result.ToString();
        }

        /// <summary>
        /// 解密函数
        /// </summary>
        /// <param name="source">加密字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="IsMD5">密钥是否通过MD5再加密</param>
        /// <returns></returns>
        public static string Decrypt(string source, string key, bool IsMD5 = true)
        {
            string result = string.Empty;
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                int len = source.Length / 2;
                byte[] inputByteArray = new byte[len];

                for (int i, x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(source.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }

                key = IsMD5 ? MD5(key).Substring(0, 8) : key;
                des.Key = Encoding.UTF8.GetBytes(key);
                des.IV = Encoding.UTF8.GetBytes(key);

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                result = Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "自定义解密函数");
            }
            return result;
        }

        #endregion 自定义加密/解密
    }
}