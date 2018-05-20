using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.Common.Enum;
using XuHos.Common;
using XuHos.BLL.Sys.Implements;

namespace XuHos.Service.EventHandlers.OrderPayCompletedEvent
{
    /// <summary>
    /// 默认处理
    /// </summary>
    public class PublishChanneCreateEvent : IEventHandler<EventBus.Events.OrderPayCompletedEvent>
    {
        public bool Handle(EventBus.Events.OrderPayCompletedEvent evt)
        {
            try
            {
                if (evt!=null && evt.OrderNo != "")
                {
                    XuHos.BLL.OrderService orderService = new BLL.OrderService("");
                    ConversationRoomService roomService = new ConversationRoomService();
                 
                  
                        if (evt.OrderType == EnumProductType.ImageText ||
                            evt.OrderType == EnumProductType.video ||
                            evt.OrderType == EnumProductType.Phone ||
                            evt.OrderType == EnumProductType.Consultation)
                        {
                            if (string.IsNullOrEmpty(evt.OrderOutID))
                            {
                                var order = orderService.GetOrder(evt.OrderNo);
                                evt.OrderOutID = order.OrderOutID;                            
                            }

                            var room = roomService.GetChannelInfo(evt.OrderOutID);

                            if (room != null)
                            {
                                using (XuHos.EventBus.MQChannel mqChannel = new MQChannel())
                                {
                                    return mqChannel.Publish(new EventBus.Events.ChannelCreateEvent()
                                    {

                                        ServiceID = room.ServiceID,
                                        ServiceType = room.ServiceType,
                                        ChannelID = room.ChannelID
                                    });
                                }
                            }
                            else
                            {
                                var order = orderService.GetOrder("",evt.OrderOutID);
                                if (order.OrderState == EnumOrderState.Canceled)
                                {

                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    
                }
            }
            catch (Exception E)
            {

                LogHelper.WriteError(E);
                return false;

            }

            return true;
        }
    }
}
