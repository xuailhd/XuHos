using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL;
using XuHos.DTO;
using XuHos.BLL.Sys.DTOs.Request;
using XuHos.BLL.Sys.Implements;
using XuHos.Common.Enum;
using XuHos.DTO.Common;

namespace XuHos.Service.EventHandlers.ChannelSendGroupMsgEvent
{
    public class IfRoomDurationChangedGroupMsg : IEventHandler<EventBus.Events.ChannelSendGroupMsgEvent<BLL.Sys.DTOs.Request.RequestCustomMsgRoomDurationChanged>>
    {
        ConversationRoomService roomService = new ConversationRoomService();

        XuHos.Integration.QQCloudy.IMHelper imService = new XuHos.Integration.QQCloudy.IMHelper();
        public bool Handle(EventBus.Events.ChannelSendGroupMsgEvent<BLL.Sys.DTOs.Request.RequestCustomMsgRoomDurationChanged> evt)
        {
            try
            {
                return imService.SendGroupCustomMsg(evt.ChannelID, evt.FromAccount, evt.Msg);
            }
            catch (XuHos.Integration.QQCloudy.InvalidGroupException)
            {
                //自动修正房间不存在的问题
                using (XuHos.EventBus.MQChannel mq = new MQChannel())
                {
                    var room = roomService.GetChannelInfo(evt.ChannelID);
                    room.Enable = false;
                    if (roomService.CompareAndSetChannelInfo(room))
                    {

                        mq.Publish<XuHos.EventBus.Events.ChannelCreateEvent>(new ChannelCreateEvent()
                        {

                            ChannelID = evt.ChannelID,
                            ServiceID = room.ServiceID,
                            ServiceType = room.ServiceType,
                        });

                      
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                return false;
            }

        }
    }
}
