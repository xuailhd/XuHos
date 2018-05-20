using System.ComponentModel;
using System.ServiceProcess;
using Quartz;
using Quartz.Impl;
using System;
using Logging = Common.Logging;

namespace XuHos.UI.WinSvr.JobService
{
    [RunInstaller(true)]
    public partial class JobService : ServiceBase
    {
        IScheduler scheduler;

        public string serviceType { get; set; }

        public JobService(string serviceType)
        {
            this.serviceType = serviceType;
            InitializeComponent();

        }

        public void Start()
        {
            try
            {
                XuHos.Service.Infrastructure.BundleConfig.RegisterApplicationConfig();

                if (serviceType.ToLower() == "job")
                {
                    scheduler = StdSchedulerFactory.GetDefaultScheduler();
                    scheduler.Start();
                }
                else if (serviceType.ToLower() == "mq")
                {
                    XuHos.Service.EventHandlers.BundleConfig.Register();
                }
                else if (serviceType.ToLower() == "cacheevent")
                {
                    var service = new XuHos.BLL.Sys.Implements.HeartbeatService();
                    service.ListernHeartBeat();
                }
               
            }
            catch(Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
            }
        }

        protected override void OnStart(string[] args)
        {
            Start();

        }

        protected override void OnStop()
        {
            XuHos.Common.LogHelper.WriteDebug("服务：开始 OnStop");
            if (scheduler != null)
            {
                scheduler.Shutdown(true);
            }

            XuHos.Common.LogHelper.WriteDebug("服务：结束 OnStop");
        }

    }
}
