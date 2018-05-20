using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Log.ApiTrack;

namespace XuHos.Common
{
    public class LogHelper
    {
        readonly static ILog log = null;
        static LogHelper()
        {
            try
            {
                log4net.Util.LogLog.InternalDebugging = true;
                XmlConfigurator.Configure();
                log = LogManager.GetLogger("DefaultLogger");
            }
            catch
            { }
        }

        /// <summary>
        /// 记录日志信息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteInfo(string info, string moduleid = "")
        {
            log.Info(info);
        }

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteError(Exception ex)
        {
            log.Error(ex);

        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteWarn(string info, Exception ex = null)
        {
            log.Warn(info);
        }

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteDebug(string info, Exception ex = null)
        {
            log.Debug(info,ex);
        }
        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteDebug(string info, string ex)
        {
            log.Debug(info, new Exception(ex));
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="collectName"></param>
        /// <param name="requestUri"></param>
        /// <param name="comments"></param>
        /// <param name="RequestParamters"></param>
        /// <param name="requestEnterTime"></param>
        /// <param name="Response"></param>
        public static void WriteTrackLog(
          string collectName,
          string requestUri,
          string comments,
          string RequestParamters,
          DateTime requestEnterTime,
          string Response)
        {
            try
            {

                var log = new XuHos.Common.Log.ApiTrack.ApiTrackLog()
                {
                    General = new Common.Log.ApiTrack.WebApiTrackLogGeneral()
                    {
                        requestUri = requestUri,
                        actionName = "",
                        comments = comments,
                        controllerName = "",
                        ip = "",
                        navigator = "",
                        remoteBrowser = "",
                        remoteHostName = "",
                        urlReferrer = "",
                        userId = "",
                    },
                    RequestHeaders = new Dictionary<string, string>(),
                    RequestParamters = RequestParamters,
                    Response = Response,
                    Statistics = new Common.Log.ApiTrack.WebApiTrackLogStatistics()
                    {
                        enterTime = requestEnterTime,
                        costTime = (DateTime.Now - requestEnterTime).TotalMilliseconds
                    },
                    Id = Guid.NewGuid()
                };


                WriteTrackLog(collectName, log);

            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
        }

        /// <summary>
        /// 写操作日志
        /// </summary>
        /// <param name="CollectName"></param>
        /// <param name="log"></param>
        public static void WriteTrackLog(string CollectName, object log)
        {
            if (log != null)
            {
                XuHos.Common.Log.Configuration.LogAppender.WriteLog(CollectName, log);
            }
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="CollectName"></param>
        /// <param name="log"></param>
        public static void WriteTrackLog(string CollectName, ApiTrackLog log)
        {
            if (log != null)
            {
                XuHos.Common.Log.Configuration.LogAppender.WriteLog(CollectName, log);
            }
        }
    }




}