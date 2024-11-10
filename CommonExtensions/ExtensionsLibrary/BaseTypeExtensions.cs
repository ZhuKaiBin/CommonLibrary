using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionsLibrary
{
    public static class BaseTypeExtensions
    {
        /// <summary>
        /// 字符串转整型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ParseInt(this string s)
        {
            int amount;
            if (!System.Int32.TryParse(s, out amount))
            {
                return 0;
            }
            return amount;
        }

        /// <summary>
        /// 字符串转Decimal
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static decimal ParseDecimal(this string s)
        {
            decimal amount;
            if (!System.Decimal.TryParse(s, out amount))
            {
                return 0;
            }
            return amount;
        }

        /// <summary>
        /// 字符串转Decimal
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static decimal? ParseDecimalAllowNull(this string s)
        {
            decimal amount;
            if (!System.Decimal.TryParse(s, out amount))
            {
                return null;
            }
            return amount;
        }

        /// <summary>
        /// 把货币单位为“分”转换为“元”
        /// </summary>
        /// <returns></returns>
        public static decimal ParseFenToYuan(this string money)
        {
            return money.ParseDecimal() / 100;
        }

        /// <summary>
        /// 替换第一个字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string s, string oldValue, string newValue)
        {
            int pos = s.IndexOf(oldValue);
            if (pos < 0)
            {
                return s;
            }
            string newstr = s.Substring(0, pos) + newValue + s.Substring(pos + oldValue.Length);
            return newstr;
        }

        /// <summary>
        /// 替换第一个字符串
        /// </summary>
        /// <param name="str">原始数据</param>
        /// <param name="oldValue">需替换值</param>
        /// <param name="newValue">替换值</param>
        /// <param name="val">替换后数据</param>
        /// <returns></returns>
        public static bool ReplaceFirst(this string str, string oldValue, string newValue, out string val)
        {
            val = str;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            int index = str.IndexOf(oldValue);
            if (index < 0)
            {
                return false;
            }

            val = str.Substring(0, index) + newValue + str.Substring(index + oldValue.Length);
            return true;
        }

        /// <summary>
        /// 给图片路径添加Http
        /// </summary>
        /// <param name="path">图片途径</param>
        /// <returns></returns>
        //public static string AddHttpToPath(this string path)
        //{
        //    if (string.IsNullOrEmpty(path))
        //    {
        //        return string.Empty;
        //    }
        //    if (!path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        //    {
        //        path = "http://" + path;
        //    }
        //    return path.SubstringByByte(300);
        //}

        /// <summary>
        /// 字符串转long
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ParseLong(this string s)
        {
            long amount;
            if (!long.TryParse(s, out amount))
            {
                return 0;
            }
            return amount;
        }

        /// <summary>
        /// 字符串转long
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long? ParseLongAllowNull(this string s)
        {
            long amount;
            if (!long.TryParse(s, out amount))
            {
                return null;
            }
            return amount;
        }

        public static bool? ParseBool(this object obj)
        {
            if (obj == null)
                return null;
            if (obj is bool)
                return (bool)obj;
            if (bool.TryParse(obj.ToString(), out bool r))
            {
                return r;
            }
            return null;
        }

        public static bool ParseBool(this object obj, bool defaultValue)
        {
            return ParseBool(obj) ?? defaultValue;
        }

        public static string ToStringParseZeroToEmpty(this int i)
        {
            return i == 0 ? String.Empty : i.ToString();
        }

        public static string ToStringParseZeroToEmpty(this decimal d)
        {
            return d == 0 ? String.Empty : d.ToString();
        }

        //新增
    }
}