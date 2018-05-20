using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.Entity;

namespace XuHos.Service.EventHandlers.OrderCompletedEvent
{
    /// <summary>
    /// 如果是一键咨询，需要从队列中移除（医生从队列中领单）
    /// </summary>
    public class IfOneButtonConsult : IEventHandler<EventBus.Events.OrderCompletedEvent>
    {
        XuHos.BLL.UserOPDRegisterService opdService = new BLL.UserOPDRegisterService("");

        public bool Handle(EventBus.Events.OrderCompletedEvent evt)
        {
            if (evt == null)
                return true;

            var orderService = new XuHos.BLL.OrderService("");
            var order = orderService.GetOrder(evt.OrderNo);

            if (order == null)
                return true;

            var opd = opdService.Single(order.OrderOutID);
          
            if (opd == null)
                return true;
            
            if (opd.IsUseTaskPool && (order.OrderType == Common.Enum.EnumProductType.video || order.OrderType == Common.Enum.EnumProductType.Phone))
            {
                var grabService = new XuHos.BLL.Sys.Implements.SysGrabService<string>(nameof(UserOPDRegister));
                grabService.FinishTask(order.OrderOutID,opd.DoctorID);
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
