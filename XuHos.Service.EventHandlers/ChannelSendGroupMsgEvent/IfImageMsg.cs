using XuHos.DTO;
using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL.Sys.Implements;

namespace XuHos.Service.EventHandlers.ChannelSendGroupMsgEvent
{
    public class IfImageMsg : IEventHandler<EventBus.Events.ChannelSendGroupMsgEvent<ResponseUserFileDTO>>
    {
        XuHos.Integration.QQCloudy.IMHelper imservice = new XuHos.Integration.QQCloudy.IMHelper();
        ConversationRoomService roomService = new ConversationRoomService();
        XuHos.Common.Config.Sections.IMGStore storeConfig = null;

        public IfImageMsg()
        {
            //文件存储配置
            storeConfig = XuHos.BLL.Sys.Implements.SysConfigService.Get<XuHos.Common.Config.Sections.IMGStore>();
        }

        public bool Handle(ChannelSendGroupMsgEvent<ResponseUserFileDTO> evt)
        {
            try
            {
                //发送图片消息
                if (evt.Msg.FileType == 0)
                {
                    return imservice.SendGroupImageMsg(evt.ChannelID, evt.FromAccount, evt.Msg.FileID, evt.Msg.FileUrl);
                }
                //发送图片消息
                else if (evt.Msg.FileType == 1)
                {
                    return imservice.SendGroupFileMsg(evt.ChannelID, evt.FromAccount, evt.Msg.FileID, evt.Msg.FileSize, evt.Msg.FileName);
                }
                //发送音频消息
                else if (evt.Msg.FileType == 2)
                {
                    using (var filestream = XuHos.Common.Storage.Manager.Instance.OpenFile("Audios", evt.Msg.FileUrl))
                    {
                        Task.WaitAll(filestream);
                  
                        var Second = Convert.ToInt32(XuHos.Common.Utility.AudioHelper.TotalSeconds(filestream.Result));
                        return imservice.SendGroupAudioMsg(evt.ChannelID, evt.FromAccount, evt.Msg.FileID, evt.Msg.FileSize, Second);
                     
                    }
                }
                else
                {
                    return true;
                }
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
