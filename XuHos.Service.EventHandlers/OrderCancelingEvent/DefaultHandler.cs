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

namespace XuHos.Service.EventHandlers.OrderCancelingEvent
{
    /// <summary>
    /// 取消订单
    /// </summary>
    public class DefaultHandler : IEventHandler<EventBus.Events.OrderCancelingEvent>
    {
        OrderService service = new OrderService("");

        public bool Handle(EventBus.Events.OrderCancelingEvent evt)
        {
            try
            {
                if (evt == null)
                {
                    return true;
                }

                var service = new XuHos.BLL.OrderService(evt.UserID);
                var result = service.Cancel(evt.OrderNo, evt.CancelReason, evt.RefundFee);
                //发布通知
                if (result == true)
                {
                    using (XuHos.EventBus.MQChannel mqChannel = new MQChannel())
                    {
                        mqChannel.Publish<EventBus.Events.UserNoticeEvent>(new EventBus.Events.UserNoticeEvent()
                        {                      
                            ServiceID = evt.OrderNo,
                            NoticeType = EnumNoticeSecondType.DoctorStopDiagnosisCancelOrderNotice
                        });
                    }
                }

                return result;
            }
            catch (Exception E)
            {
                XuHos.Common.LogHelper.WriteError(E);
                return false;
            }

        }
    }
}
