using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

namespace XuHos.Common.Pay.WxPay
{
    internal class Log
    {

        /**
         * 向日志文件写入调试信息
         * @param className 类名
         * @param content 写入内容
         */
        public static void Debug(string className, string content)
        {
            XuHos.Common.LogHelper.WriteDebug(content);
        }

        /**
        * 向日志文件写入运行时信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Info(string className, string content)
        {
            XuHos.Common.LogHelper.WriteInfo(content);
        }

        /**
        * 向日志文件写入出错信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Error(string className, string content)
        {
            XuHos.Common.LogHelper.WriteError(new Exception(content));
        }
    }
}