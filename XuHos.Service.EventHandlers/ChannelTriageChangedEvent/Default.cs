using XuHos.BLL;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.DTOs.Request;
using XuHos.BLL.Sys.Implements;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.DTO;
using XuHos.Entity;
using XuHos.EventBus;
using XuHos.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.EventHandlers.ChannelTriageChangedEvent
{
    /// <summary>
    /// 频道分诊被改变时
    /// </summary>
    public class DefaultHandler : IEventHandler<EventBus.Events.ChannelTriageChangeEvent>
    {
        ConversationRoomService roomService = new ConversationRoomService();


        public bool Handle(EventBus.Events.ChannelTriageChangeEvent evt)
        {
            try
            {
                if (evt == null)
                {
                    return true;
                }

                #region 设置分诊编号 
                var room = roomService.GetChannelInfo(evt.ChannelID);
           
                room.TriageID = XuHos.Common.Utility.SeqIDHelper.GetSeqId();
                //修改就诊时间和开始就诊时间
                if (!roomService.CompareAndSetChannelInfo(room))
                {
                    return false;
                }
                #endregion

                #region 发送候诊队列通知
                roomService.SendWaitingQueueChangeNotice(evt.DoctorID);
                #endregion
            }
            catch(Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                return false;
            }

            return true;
        }
    }


}
