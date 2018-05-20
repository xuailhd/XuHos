using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.BLL.Sys.DTOs.Request;
using XuHos.EventBus.Events;
using XuHos.BLL.Sys.Implements;

namespace XuHos.Service.EventHandlers.ChannelSendGroupMsgEvent
{
    public class IfCustomMsgSurvey : IEventHandler<EventBus.Events.ChannelSendGroupMsgEvent<BLL.Sys.DTOs.Request.RequestIMCustomMsgSurvey>>
    {
        XuHos.Integration.QQCloudy.IMHelper imservice = new XuHos.Integration.QQCloudy.IMHelper();
        ConversationRoomService roomService = new ConversationRoomService();

        public bool Handle(ChannelSendGroupMsgEvent<RequestIMCustomMsgSurvey> evt)
        {
            try
            {
                return imservice.SendGroupCustomMsg(evt.ChannelID, evt.FromAccount,evt.Msg);
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
            catch (Exception Ex)
            {
                XuHos.Common.LogHelper.WriteError(Ex);
                return false;
            }
        }
    }
}
