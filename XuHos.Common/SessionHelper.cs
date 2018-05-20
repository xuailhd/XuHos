using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XuHos.Common
{
   public class SessionHelper
    {
        /// <summary>
        /// 根据会话名称返回会话值
        /// </summary>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        public static object GetSession(string sessionName)
        {
            var sessionData = HttpContext.Current.Session[sessionName];
            if (sessionData != null)
            {
                return sessionData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置会话
        /// </summary>
        /// <param name="sessionName">会话名称</param>
        /// <param name="sessionData">会话值</param>
        /// <param name="outMinute">过期时间，不赋值，则为默认20分钟</param>
        public static void SetSession(string sessionName,object sessionData, int outMinute=0)
        {
            var sessionData0 = GetSession(sessionName);
            if (sessionData0 != null)
            {
                HttpContext.Current.Session.Remove(sessionName);
            }
            HttpContext.Current.Session.Add(sessionName, sessionData);
            if (outMinute > 0)
            {
                HttpContext.Current.Session.Timeout = outMinute;
            }
        }

        /// <summary>
        /// 根据会话名称移除会话
        /// </summary>
        /// <param name="sessionName"></param>
        public static void RemoveSession(string sessionName)
        {
            var sessionData0 = GetSession(sessionName);
            if (sessionData0 != null)
            {
                HttpContext.Current.Session.Remove(sessionName);
            }
        }
    }
}
