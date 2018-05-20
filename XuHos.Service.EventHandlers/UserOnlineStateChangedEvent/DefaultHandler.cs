using XuHos.BLL;
using XuHos.BLL.Sys.Implements;
using XuHos.Common;
using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.EventHandlers.UserOnlineStateChangedEvent
{
    /// <summary>
    /// 用户在线状态变更默认处理事件
    
    /// 日期：2017年4月22日
    /// </summary>
    public class DefaultHandler : IEventHandler<EventBus.Events.UserOnlineStateChangedEvent>
    {
        public bool Handle(EventBus.Events.UserOnlineStateChangedEvent evt)
        {
            try
            {
                if (evt == null || evt.UserID== "@TLS#ERROR")
                {
                    return true;
                }

                var userService = new BLL.Sys.Implements.ConversationIMUidService("");
                
                var userids = userService.GetUserIdByUids(new int[] { int.Parse(evt.UserID)});
                if (userids.Count > 0)
                {
                    ConversationRoomService bllRoom = new ConversationRoomService();
                    var heartService = new HeartbeatService();
                    var userId = userids[0];
                    if (evt.Action == "Logout")
                    {

                        //断开连接时设置房间状态（断开）
                        bllRoom.Disconnection(userId);
                        heartService.SetAppHeartBeat(userId, false);
                    }
                    else if (evt.Action == "Login")
                    {
                        heartService.SetAppHeartBeat(userId, true);
                    }
                }
                else
                {
                    return true;
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
