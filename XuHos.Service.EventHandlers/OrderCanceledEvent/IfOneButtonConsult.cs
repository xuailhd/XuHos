using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.Entity;

namespace XuHos.Service.EventHandlers.OrderCanceledEvent
{
    /// <summary>
    /// 如果是一键咨询，需要从队列中移除（医生从队列中领单）
    /// </summary>
    public class IfOneButtonConsult : IEventHandler<EventBus.Events.OrderCanceledEvent>
    {
        XuHos.BLL.OrderService orderService = new XuHos.BLL.OrderService("");

        public bool Handle(EventBus.Events.OrderCanceledEvent evt)
        {
            if (evt == null)
                return true;

            var order = orderService.GetOrder(evt.OrderNo);
            if (order != null)
            {
                if (order.OrderType == Common.Enum.EnumProductType.video || order.OrderType == Common.Enum.EnumProductType.Phone)
                {
                    var grabService = new XuHos.BLL.Sys.Implements.SysGrabService<string>(nameof(UserOPDRegister));
                    grabService.CancelTask(order.OrderOutID);

                    return true;
                }
            }

            return true;


        }
    }
}
