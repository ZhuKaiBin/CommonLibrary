using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionsLibrary
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 比较日期大小，不带时分秒
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToCompareDate(this DateTime dateTime, DateTime value)
        {
            dateTime = Convert.ToDateTime(dateTime.ToString("yyyy-MM-dd"));
            value = Convert.ToDateTime(value.ToString("yyyy-MM-dd"));
            return dateTime.CompareTo(value);
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取格式化字符串，不带时分秒，格式："yyyy-MM-dd"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToDateString(dateTime.Value);
        }

        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dateTime, string format)
        {
            return dateTime.ToString(format);
        }

        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime? dateTime, string format)
        {
            return dateTime.GetValueOrDefault(DateTime.MinValue).ToDateString(format);
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 获取格式化字符串，不带年月日，格式："HH:mm:ss"
        /// </summary>
        /// <param name="dateTime">日期</param>
        public static string ToTimeString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;
            return ToTimeString(dateTime.Value);
        }

        /// <summary>
        /// 当前最早时间，格式："yyyy-MM-dd 00:00:00"
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToEarliestDateTime(this DateTime dateTime)
        {
            return Convert.ToDateTime(dateTime.ToString("yyyy-MM-dd 00:00:00"));
        }

        /// <summary>
        /// 当前最晚时间，格式："yyyy-MM-dd 23:59:59"
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToLatestDateTime(this DateTime dateTime)
        {
            return Convert.ToDateTime(dateTime.ToString("yyyy-MM-dd 23:59:59"));
        }

        /// <summary>
        /// 获取UNIX（秒）
        /// </summary>
        /// <returns></returns>
        public static long GetUnixTimeStamp(this DateTime datetime)
        {
            return (long)(datetime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        /// <summary>
        /// 获取UNIX（毫秒）
        /// </summary>
        /// <returns></returns>
        public static long GetUnixMilTimeStamp(this DateTime datetime)
        {
            return (long)(datetime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// 将Unix转化为DateTIme（默认值）
        /// </summary>
        /// <param name="unix">unix</param>
        /// <param name="isMillisecond">是否为毫秒（默认false）</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(this long unix, bool isMillisecond = false)
        {
            DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            if (isMillisecond == false)
            {
                unix = unix * 1000;
            }
            var date = dt.AddMilliseconds(unix);
            //DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unix);
            return date;
        }

        /// <summary>
        /// 将yyyyMMddHHmmss的字符串转化为DateTime
        /// </summary>
        /// <param name="dateTimeStr">字符串</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(this string dateTimeStr)
        {
            if (string.IsNullOrEmpty(dateTimeStr))
            {
                return DateTime.MinValue;
            }
            DateTime convertDate = DateTime.ParseExact(dateTimeStr, "yyyyMMddHHmmss", null, DateTimeStyles.AllowWhiteSpaces);
            return convertDate;
        }

        /// <summary>
        /// 将yyyyMMddHHmmss的字符串转化为DateTime,可为空
        /// </summary>
        /// <param name="dateTimeStr">字符串</param>
        /// <returns></returns>
        public static DateTime? ConvertToDateTimeAllowNull(this string dateTimeStr)
        {
            if (!DateTime.TryParseExact(dateTimeStr, "yyyyMMddHHmmss", null, DateTimeStyles.AllowWhiteSpaces, out DateTime result))
            {
                return null;
            }
            DateTime convertDate = DateTime.ParseExact(dateTimeStr, "yyyyMMddHHmmss", null, DateTimeStyles.AllowWhiteSpaces);
            return convertDate;
        }

        /// <summary>
        /// yyyyMMddHHmmssfff+0800的字符串转化为DateTime
        /// </summary>
        /// <param name="dateTimeStr">字符串</param>
        /// <returns></returns>
        public static DateTime ParseDateTimeBeiJing(this string dateTimeStr, DateTime? _default = null)
        {
            var result = DateTime.Now;
            if (!_default.HasValue || _default < DateTime.MinValue)
            {
                _default = DateTime.Now;
            }
            if (!DateTime.TryParseExact(dateTimeStr, "yyyyMMddHHmmssfff+0800", null, DateTimeStyles.AllowWhiteSpaces, out result))
            {
                return _default.Value;
            }
            return result;
        }

        /// <summary>
        /// 根据格式转换日期
        /// </summary>
        /// <param name="dateTimeStr"></param>
        /// <param name="format"></param>
        /// <param name="_default"></param>
        /// <returns></returns>
        public static DateTime ParseDateTime(this string dateTimeStr, string format, DateTime? _default = null)
        {
            var result = DateTime.Now;
            if (!_default.HasValue || _default < DateTime.MinValue)
            {
                _default = DateTime.Now;
            }
            if (!DateTime.TryParseExact(dateTimeStr, format, null, DateTimeStyles.AllowWhiteSpaces, out result))
            {
                return _default.Value;
            }
            return result;
        }

        /// <summary>
        /// 字符串转DateTime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? ParseDateTimeAllowNull(this string date)
        {
            var result = DateTime.Now;

            if (!DateTime.TryParse(date, out result))
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// 字符串转Datetime
        /// </summary>
        /// <param name="date"></param>
        /// <param name="_default"></param>
        /// <returns></returns>
        public static DateTime ParseDateTime(this string date, DateTime? _default = null)
        {
            var result = DateTime.Now;

            if (!_default.HasValue || _default < DateTime.MinValue)
            {
                _default = DateTime.Now;
            }

            if (!DateTime.TryParse(date, out result))
            {
                result = _default.Value;
            }

            return result;
        }

        /// <summary>
        /// 将有效时长毫秒转换为对应时间
        /// </summary>
        /// <param name="date">有效时长，毫秒</param>
        /// <returns></returns>
        public static DateTime? ParseValidTimeAllowNull(this string millisecond)
        {
            if (millisecond.IsNullOrEmpty())
            {
                return null;
            }
            return DateTime.Now.AddMilliseconds(millisecond.To_Long());
        }

        /// <summary>
        /// 将有效时长毫秒转换为对应时间
        /// </summary>
        /// <param name="date">有效时长，毫秒</param>
        /// <returns></returns>
        public static DateTime ParseValidTime(this string millisecond)
        {
            if (millisecond.IsNullOrEmpty())
            {
                return DateTime.MinValue;
            }
            return DateTime.Now.AddMilliseconds(Convert.ToInt64(millisecond));
        }

        public static DateTime ParseTimestamp(string date, DateTime? defaultValue = null)
        {
            if (defaultValue == null)
            {
                defaultValue = DateTime.Now;
            }

            long timestamp = 0;
            if (!long.TryParse(date, out timestamp))
            {
                return defaultValue.Value;
            }

            return DateTimeExtensions.ConvertToDateTime(timestamp, timestamp > 18934308610);
        }

        public static DateTime? ParseTimestampAllowNull(string date)
        {
            long timestamp = 0;
            if (!long.TryParse(date, out timestamp))
            {
                return null;
            }

            if (timestamp <= 0)
            {
                return null;
            }

            return DateTimeExtensions.ConvertToDateTime(timestamp, timestamp > 18934308610);
        }

        /// <summary>
        /// 截断毫秒数
        /// </summary>
        /// <param name="dt">时间</param>
        /// <param name="milliSeconds3">mssql 3毫秒精度优化</param>
        /// <returns></returns>
        public static DateTime CutOffMillisecond(this DateTime dt, bool milliSeconds3 = false)
        {
            return new DateTime(dt.Ticks - (dt.Ticks % TimeSpan.TicksPerSecond), dt.Kind);
        }

        /// <summary>
        /// UTC时间字符串转化为北京时间
        /// </summary>
        /// <param name="dateTimeStr">字符串</param>
        /// <returns></returns>
        public static DateTime ParseUtcToBeiJing(this string dateTimeStr, DateTime? _default = null)
        {
            var result = DateTime.Now;
            if (!_default.HasValue || _default < DateTime.MinValue)
            {
                _default = DateTime.Now;
            }

            result = dateTimeStr.ParseDateTime();
            if (result <= DateTime.MinValue)
            {
                return _default.Value;
            }

            return TimeZoneInfo.ConvertTimeToUtc(result, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
        }

        /// <summary>
        /// 北京时间转太平洋时间UTC
        /// </summary>
        /// <param name="dateTime">字符串</param>
        /// <returns></returns>
        public static DateTime ParseBeiJingToUtc(this DateTime dateTime)
        {
            if (dateTime <= DateTime.MinValue)
            {
                return DateTime.Now;
            }

            //必须要处理下，不然会导致报错 Kind 属性设置不正确
            dateTime = DateTime.Parse(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            return TimeZoneInfo
                .ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"),
                    TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
        }

        /// <summary>
        /// 太平洋时间UTC 转 北京时间
        /// </summary>
        /// <param name="dateTime">字符串</param>
        /// <returns></returns>
        public static DateTime ParseUtcToBeiJing(this DateTime dateTime)
        {
            if (dateTime <= DateTime.MinValue)
            {
                return DateTime.Now;
            }

            //必须要处理下，不然会导致报错 Kind 属性设置不正确
            dateTime = DateTime.Parse(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            return TimeZoneInfo
                .ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
                    TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
        }

        /// <summary>
        /// DateTime 转 long
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ParseDataTimeToLong(this DateTime dateTime)
        {
            DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            if (dateTime <= DateTime.MinValue)
            {
                dateTime = DateTime.Now;
            }

            return (long)(dateTime - dt).TotalMilliseconds;
        }

        /// <summary>
        /// 时间极值，最小值
        /// 自定义: 1970-01-01
        /// mssql: 1753-01-01
        /// mysql: 0000-01-01
        /// C#: 0000-01-01
        /// </summary>
        public static DateTime MinDateTime
        {
            get
            {
                return new DateTime(1970, 1, 1);
            }
        }

        /// <summary>
        /// 时间极值，最大值
        /// 自定义: 3000-01-01
        /// mssql: 9999-12-31
        /// mysql: 9999-12-31
        /// C#: 9999-12-31
        /// </summary>
        public static DateTime MaxDateTime
        {
            get
            {
                return new DateTime(3000, 1, 1);
            }
        }

        /// <summary>
        /// 校验时间极值，阻止传值到业务层|数据库层
        /// true:合理值，false:非法值
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool CheckDateLimit(this DateTime date)
        {
            if (date >= MinDateTime && date <= MaxDateTime)
            {
                return true;
            }
            return false;
        }
    }
}