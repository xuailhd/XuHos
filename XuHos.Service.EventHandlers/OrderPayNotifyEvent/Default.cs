using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.EventHandlers.OrderPayNotifyEvent
{
    public class DefaultHandler : IEventHandler<EventBus.Events.OrderPayNotifyEvent>
    {
        public bool Handle(EventBus.Events.OrderPayNotifyEvent evt)
        {
            try
            {
                if (evt == null)
                    return true;

                XuHos.BLL.OrderService service = new BLL.OrderService("");
           
                return service.PayCompleted(evt.OrderNo, evt.TradeNo, evt.PayType, evt.SelllerID);
            }
            catch (Exception E)
            {
                LogHelper.WriteError(E);
            }

            return false;
        }
    }


}
