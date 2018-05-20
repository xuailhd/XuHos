using KMEHosp.Common.Log.ApiTrack;
using KMEHosp.Hystrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMEHosp.Common.Log.Commands
{
    /// <summary>
    /// 写日志命令（支持熔断）
    /// 作者：郭明
    /// 日期：2017年6月28日
    /// </summary>
    class WriteTrackLogCommand : HystrixCommand<bool>
    {

        private static bool fallback = false;

        private ITrackLogLogAppender LogAppender = null;

        private string CollectName = "";

        private object log = null;

        public WriteTrackLogCommand(ITrackLogLogAppender LogAppender, string CollectName, object log)
            : base(HystrixCommandSetter.WithGroupKey("Log")
                .AndCommandKey("WriteTrackLog")
                .AndCommandPropertiesDefaults(
                    new HystrixCommandPropertiesSetter()

                    //使用线程池隔离模式
                    .WithExecutionIsolationStrategy(ExecutionIsolationStrategy.Thread)
                    //执行超时则打断
                    .WithExecutionIsolationThreadInterruptOnTimeout(true)
                    //执行超时时间（100毫秒）
                    .WithExecutionIsolationThreadTimeoutInMilliseconds(100)

                    //当在配置时间窗口内达到此数量的失败后，进行断开。默认20个）
                    .WithCircuitBreakerRequestVolumeThreshold(10)
                    //出错百分比阈值，当达到此阈值后，开始短路。默认50%
                    .WithCircuitBreakerErrorThresholdPercentage(50)
                    //多久以后开始尝试是否恢复，默认5s）
                    .WithCircuitBreakerSleepWindow(TimeSpan.FromSeconds(10))
                ))
        {
            this.LogAppender = LogAppender;
            this.CollectName = CollectName;
            this.log = log;
        }

        protected override bool Run()
        {
            if (LogAppender != null)
            {
                //记录本地日志
                LogAppender.WriteLog(CollectName, log);

                return true;
            }

            return true;

        }

        /// <summary>
        /// 失败时默认处理
        /// </summary>
        /// <returns></returns>
        protected override bool GetFallback()
        {
            return fallback;
        }
    }
}
