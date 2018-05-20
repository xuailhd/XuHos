using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL;
using XuHos.Common.Cache;
using XuHos.Common.Enum;

namespace XuHos.Service.EventHandlers.OrderRefundEvent
{
    /// <summary>
    /// 订单退款
    /// </summary>
    public class DefaultHandler : IEventHandler<EventBus.Events.OrderRefundEvent>
    {
        OrderService service = new OrderService("");

        public bool Handle(EventBus.Events.OrderRefundEvent evt)
        {
            try
            {
                if (evt == null)
                {
                    return true;
                }

                if (evt.PayType == Common.Enum.EnumPayType.None)
                {
                    return true;
                }

                return service.Refund(evt.OrderNo, evt.PayType, evt.TradeNo);
            }
            catch (Exception E)
            {

                XuHos.Common.LogHelper.WriteError(E);
                return false;
            }
        }
    }
}
