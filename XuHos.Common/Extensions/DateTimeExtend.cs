using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Extensions
{
    public static class DateTimeExtend
    {
        /// <summary>
        /// 转换成时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ToTimeStamp(this System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        public static long ToJavascriptTimestamp(this System.DateTime input)
        {
            return ((input - (new System.DateTime(1970, 1, 1, 0, 0, 0, 0))).Ticks / 10000);

            //System.TimeSpan span = new System.TimeSpan(System.DateTime.Parse("1/1/1970").Ticks);
            //System.DateTime time = input.Subtract(span);
            //return (long)(time.Ticks / 10000);
        }
    }
}
