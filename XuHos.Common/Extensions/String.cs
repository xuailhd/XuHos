using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace XuHos.Extensions
{
    using System.Text.RegularExpressions;

    public static class StringExtend
    {
        public static byte[] getBytes(this string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        public static string IfNull(this string str, string defaultStr)

        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return defaultStr;
            }
            else
            {
                return str;
            }
        }

        public static int ToInt(this string value, int defaultValue)
        {
            var result = defaultValue;
            return int.TryParse(value, out result) ? result : defaultValue;
        }

        public static int? ToNullableInt(this string value)
        {
            int result;

            if (string.IsNullOrEmpty(value) || !int.TryParse(value, out result))
            {
                return null;
            }

            return result;
        }

        public static decimal ToDecimal(this string value)
        {
            return decimal.Parse(value);
        }

        public static decimal ToDecimal(this string value, decimal defaultValue)
        {
            var result = defaultValue;
            return decimal.TryParse(value, out result) ? result : defaultValue;
        }

        public static decimal ToRoundDecimal(this string value, decimal defaultValue, int decimals)
        {
            var result = defaultValue;
            result = Math.Round(decimal.TryParse(value, out result) ? result : defaultValue, decimals);
            return result;
        }


        public static decimal? ToNullableDecimal(this string value)
        {
            decimal result;
            if (string.IsNullOrEmpty(value) || !decimal.TryParse(value, out result))
            {
                return null;
            }
            return result;
        }

        public static short? ToNullableShort(this string value)
        {
            short result;

            if (string.IsNullOrEmpty(value) || !short.TryParse(value, out result))
            {
                return null;
            }

            return result;
        }

        public static DateTime? ToNullableDateTime(this string value)
        {
            DateTime result;

            if (DateTime.TryParse(value, out result))
            {
                return result;
            }

            return null;
        }

        public static DateTime ToDateTime(this string value)
        {
            return DateTime.Parse(value);
        }

        public static byte? ToNullableByte(this string value)
        {
            byte result;

            if (string.IsNullOrEmpty(value) || !byte.TryParse(value, out result))
            {
                return null;
            }

            return result;
        }


        public static bool? ToNullableBool(this string value)
        {
            bool result;

            if (string.IsNullOrEmpty(value) || !bool.TryParse(value, out result))
            {
                return null;
            }

            return result;
        }

        public static bool ToBool(this string value)
        {
            return bool.Parse(value);
        }

        /// <summary>
        /// 去掉字符串中的html
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToNoHtmlString(this string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<img[^>]*>;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            return Htmlstring;
        }
    }
}
