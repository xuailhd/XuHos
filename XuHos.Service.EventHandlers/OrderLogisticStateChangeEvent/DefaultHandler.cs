using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL;

namespace XuHos.Service.EventHandlers.OrderLogisticStateChangeEvent
{
    /// <summary>
    /// 订单物流状体变更时间
    /// </summary>
    class DefaultHandler : IEventHandler<EventBus.Events.OrderLogisticStateChangeEvent>
    {
        public bool Handle(EventBus.Events.OrderLogisticStateChangeEvent evt)
        {
            try
            {
                return new OrderService("").LogisticStateUpdate(
                    evt.OrderNo, 
                    evt.LogisticState,
                    evt.LogisticCompanyName,
                    evt.LogisticWayBillNo);
            }
            catch(Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
            }

            return false;

        
        }
    }
}
