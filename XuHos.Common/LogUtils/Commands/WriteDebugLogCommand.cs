using KMEHosp.Hystrix;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMEHosp.Common.Log.Commands
{
    class WriteDebugLogCommand : HystrixCommand<bool>
    {
        readonly static ILog log = LogManager.GetLogger("DefaultLogger");

        string info = "";
        Exception ex = null;

        public WriteDebugLogCommand(string info, Exception ex)
            : base(HystrixCommandSetter.WithGroupKey("Log")
                .AndCommandKey("WriteDebugLog")
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
            this.info = info;
            this.ex = ex;
        }

        protected override bool Run()
        {
            log.Debug(info, ex);
            return true;
        }

        protected override bool GetFallback()
        {
            return false;
        }
    }
}
