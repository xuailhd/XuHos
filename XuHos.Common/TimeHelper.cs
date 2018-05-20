using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common
{
    public class TimeHelper
    {
        /// <summary>
        /// 时间戳(Utc,格林威治时间)
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            //Utc,格林威治的当前时间
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
