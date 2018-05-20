using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using XuHos.Common.Cache;
namespace XuHos.Service.Jobs
{
    public class SYNC_UpdateOrderStatus : Quartz.IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var lockValue = Guid.NewGuid().ToString("N");
            if (typeof(SYNC_UpdateOrderStatus).Name.Lock(lockValue,TimeSpan.FromSeconds(5)))
            {
                try
                {
                    BLL.OrderService OrderService = new BLL.OrderService("");
                    OrderService.BatchRefreshStateAsync();
                }
                catch (Exception E)
                {
                    XuHos.Common.LogHelper.WriteError(E);
                }
                finally
                {
                    typeof(SYNC_UpdateOrderStatus).Name.UnLock(lockValue);
                }
            }
        }
    }
}
