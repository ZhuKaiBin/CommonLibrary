using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionsLibrary
{
    public static class StringExtensions
    {
        public static bool IsEqual(this string str, string value, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            return string.Equals(str, value, comparisonType);
        }

        /// <summary>
        /// 移除指定开头字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveStartsWith(this string str, string prefix, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            return str.StartsWith(prefix, comparisonType) ? str.Remove(0, prefix.Length) : str;
        }

        /// <summary>
        /// 批量移除指定开头字符串
        /// </summary>
        /// <param name="list">字符串集合</param>
        /// <param name="prefix">前缀</param>
        /// <param name="isContainsSource">返回结果是否包含原字符串</param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static HashSet<string> RemoveStartsWith(this IEnumerable<string> list, string prefix, bool isContainsSource = true, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            HashSet<string> result = new HashSet<string>();
            if (list.Any())
            {
                foreach (var str in list)
                {
                    if (isContainsSource)
                    {
                        result.Add(str);
                    }
                    if (str.StartsWith(prefix, comparisonType))
                    {
                        result.Add(str.Remove(0, prefix.Length));
                    }
                }
            }
            return result;
        }

        public static string ToJstBCD(this string str)
        {
            if (str.IsNullOrEmpty())
                return str;

            Dictionary<char, char> map = new Dictionary<char, char>();
            map.Add('Ａ', 'A');
            map.Add('Ｂ', 'B');
            map.Add('Ｃ', 'C');
            map.Add('Ｄ', 'D');
            map.Add('Ｅ', 'E');
            map.Add('Ｆ', 'F');
            map.Add('Ｇ', 'G');
            map.Add('Ｈ', 'H');
            map.Add('Ｉ', 'I');
            map.Add('Ｊ', 'J');
            map.Add('Ｋ', 'K');
            map.Add('Ｍ', 'M');
            map.Add('Ｎ', 'N');
            map.Add('Ｏ', 'O');
            map.Add('Ｐ', 'P');
            map.Add('Ｑ', 'Q');
            map.Add('Ｒ', 'R');
            map.Add('Ｓ', 'S');
            map.Add('Ｔ', 'T');
            map.Add('Ｕ', 'U');
            map.Add('Ｖ', 'V');
            map.Add('Ｗ', 'W');
            map.Add('Ｘ', 'X');
            map.Add('Ｙ', 'Y');
            map.Add('Ｚ', 'Z');
            map.Add('０', '0');
            map.Add('１', '1');
            map.Add('２', '2');
            map.Add('３', '3');
            map.Add('４', '4');
            map.Add('５', '5');
            map.Add('６', '6');
            map.Add('７', '7');
            map.Add('８', '8');
            map.Add('９', '9');

            map.Add('ａ', 'a');
            map.Add('ｂ', 'b');
            map.Add('ｃ', 'c');
            map.Add('ｄ', 'd');
            map.Add('ｅ', 'e');
            map.Add('ｆ', 'f');
            map.Add('ｇ', 'g');
            map.Add('ｈ', 'h');
            map.Add('ｉ', 'i');
            map.Add('ｊ', 'j');
            map.Add('ｋ', 'k');
            map.Add('ｍ', 'm');
            map.Add('ｎ', 'n');
            map.Add('ｏ', 'O');
            map.Add('ｐ', 'p');
            map.Add('ｑ', 'q');
            map.Add('ｒ', 'r');
            map.Add('ｓ', 's');
            map.Add('ｔ', 't');
            map.Add('ｕ', 'u');
            map.Add('ｖ', 'v');
            map.Add('ｗ', 'w');
            map.Add('ｘ', 'x');
            map.Add('ｙ', 'y');
            map.Add('ｚ', 'z');

            map.Add('－', '-');
            map.Add('＿', '_');
            map.Add('／', '/');
            map.Add('，', ',');

            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (map.ContainsKey(c[i]))
                {
                    c[i] = map[c[i]];
                }
            }

            return new string(c);
        }

        public static string[] Split(this string str, string splitStr)
        {
            return str.Split(new string[] { splitStr }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static long To_Long(this object obj, long defaultVal = 0)
        {
            try
            {
                return Convert.ToInt64(obj);
            }
            catch
            {
                return defaultVal;
            }
        }

        public static decimal To_Decimal(this object obj, decimal defaultVal = 0)
        {
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return defaultVal;
            }
        }

        public static DateTime To_Date(this object obj, DateTime defaultVal)
        {
            try
            {
                return Convert.ToDateTime(obj);
            }
            catch
            {
                return defaultVal;
            }
        }

        public static double To_Double(this object obj, double defaultVal = 0)
        {
            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return defaultVal;
            }
        }

        /// <summary>
        /// 将字符串转换为枚举，会忽略字符串空格，如果字符串是数字对应的有枚举，也会转换成功
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static TEnum TryParseEnum<TEnum>(this string value, TEnum defaultValue = default, bool ignoreCase = true) where TEnum : struct
        {
            // "".TryParseEnum(App.Admin); //失败默认值 App.Admin
            // "".TryParseEnum<App>(); //失败默认值 App.None
            // "xxx".TryParseEnum(App.Admin); //失败默认值 App.Admin
            //
            // "1".TryParseEnum(App.Admin); //成功 App.ERP
            // " 1 ".TryParseEnum(App.Admin); //成功 App.ERP
            //
            // "WMS".TryParseEnum(App.Admin);//成功 App.WMS
            // "wms".TryParseEnum(App.Admin); //成功 App.WMS
            // "Wms".TryParseEnum(App.Admin); //成功 App.WMS
            // " WmS ".TryParseEnum(App.Admin); //成功 App.WMS

            if (!Enum.TryParse(value, ignoreCase, out TEnum result))
            {
                result = defaultValue;
            }
            return result;
        }

        public static int To_Int(this object obj, int defaultVal = 0)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return defaultVal;
            }
        }

        public static T To_Convert<T>(this object obj, T defaultVal = default(T))
        {
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch
            {
                return defaultVal;
            }
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            if (value == null)
            {
                return true;
            }
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 将字符串数组分割成整形数组
        /// </summary>
        /// <param name="str">字符数组（例如："1,2,3"）</param>
        /// <param name="separator">分隔符（例如：','）</param>
        /// <returns>返回整形数组</returns>
        public static int[] ToIntArray(this string str, char separator)
        {
            string[] ar = str.Split(separator);
            System.Collections.Generic.List<int> ints = new System.Collections.Generic.List<int>();
            foreach (var item in ar)
            {
                int v;
                if (int.TryParse(item, out v))
                    ints.Add(v);
            }
            return ints.ToArray();
        }

        public static T TrySplitValueIndex<T>(this string str, int vIndex, params char[] separator)
        {
            if (separator == null || separator.Length == 0)
            {
                separator = new char[2]
                {
                    ',',
                    '，'
                };
            }
            var t = str.Split(separator);
            if (t.Length > vIndex)
            {
                var r = t[vIndex];
                if (!string.IsNullOrWhiteSpace(r))
                {
                    return Hxj.Helper.ValueHelper.TryParseValue<T>(r.Trim());
                }
            }
            return default;
        }

        public static T[] TrySplitValue<T>(this string str, params char[] separator)
        {
            return Hxj.Helper.StringHelper.TrySplitValue<T>(str, separator);
        }

        /// <summary>
        /// json序列化，排除Null值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string JsonSerializeWithNullIgnore(this object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public static string EmptyValue(this string s, string defaultValue)
        {
            return string.IsNullOrEmpty(s) ? defaultValue : s;
        }

        /// <summary>
        /// 匹配忽略大小写
        /// </summary>
        /// <param name="source"></param>
        /// <param name="toCheck"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}