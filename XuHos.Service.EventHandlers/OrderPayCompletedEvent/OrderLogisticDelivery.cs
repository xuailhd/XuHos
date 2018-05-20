using XuHos.Common.Cache;
using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.EventHandlers.OrderPayCompletedEvent
{
    public class OrderLogisticDelivery : IEventHandler<EventBus.Events.OrderPayCompletedEvent>
    {
        XuHos.BLL.OrderService orderService = new BLL.OrderService("");

        public bool Handle(EventBus.Events.OrderPayCompletedEvent evt)
        {
            if (evt == null || evt.OrderNo == "")
                return true;

            var LockName = $"{typeof(OrderLogisticDelivery)}:{evt.OrderNo}";

            var lockValue = Guid.NewGuid().ToString("N");

            //获取分布式锁，获取锁失败时进行锁等待（锁超时时间5秒）
            if (LockName.Lock(lockValue, TimeSpan.FromSeconds(5)))
            {
                try
                {
                    return orderService.LogisticWithDelivery(evt.OrderNo);
                }
                catch (Exception ex)
                {
                    XuHos.Common.LogHelper.WriteError(ex);
                    return false;
                }
                finally
                {
                    LockName.UnLock(lockValue);
                }
            }
            else
            {
                return false;
            }            
        }
    }


}
