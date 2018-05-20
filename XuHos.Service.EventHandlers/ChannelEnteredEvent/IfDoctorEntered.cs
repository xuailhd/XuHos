using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.BLL;
using XuHos.BLL.Sys.DTOs.Request;
using XuHos.BLL.Sys.Implements;
using XuHos.Common;
using XuHos.Common.Cache;
using XuHos.Common.Enum;
using XuHos.EventBus;
using XuHos.EventBus.Events;
using XuHos.Integration.QQCloudy;

namespace XuHos.Service.EventHandlers.ChannelEnteredEvent
{
    public class IfDoctorEntered : IEventHandler<EventBus.Events.ChannelEnteredEvent>
    {
        public bool Handle(EventBus.Events.ChannelEnteredEvent evt)
        {
            if (evt == null || string.IsNullOrEmpty(evt.UserID))
                return true;

            try
            {
                var service = new XuHos.BLL.Doctor.Implements.DoctorService();
                string doctorID = service.GetDoctorIDByUserID(evt.UserID);
                if (string.IsNullOrEmpty(doctorID))
                    return true;

                ConversationRoomService roomService = new ConversationRoomService();
                var room = roomService.GetChannelInfo(evt.ChannelID);
                if (room == null)
                    return true;

                // 判断医生是否已经进入过诊室
                var enteredStateCacheKey = new XuHos.Common.Cache.Keys.EntityListCacheKey<string>(Common.Cache.Keys.StringCacheKeyType.Channel_EnteredState, evt.ChannelID.ToString());
                List<string> enteredStates = enteredStateCacheKey.FromCache();
                if (enteredStates == null)
                    enteredStates = new List<string>();

                // 如果医生已进入过诊室
                if (enteredStates.Exists(x => x == evt.UserID))
                    return true;

                enteredStates.Add(evt.UserID);

                // 将机器人加入诊室
                //int robotIdentifier = 0;
                //if (!AddRobotToChannel(evt.ChannelID, robotIdentifier))
                //    return false;
                int? doctorIdentifier = roomService.GetChannelUsersInfo(evt.ChannelID)?.Where(x => x.UserID == evt.UserID).Select(x => x.identifier).FirstOrDefault();
                if (!doctorIdentifier.HasValue)
                    return true;

                using (MQChannel channle = new MQChannel())
                {
                    if (channle.Publish<EventBus.Events.ChannelSendGroupMsgEvent<string>>(new EventBus.Events.ChannelSendGroupMsgEvent<string>()
                    {
                        ChannelID = evt.ChannelID,
                        FromAccount = doctorIdentifier.Value,//robotIdentifier,
                        Msg = "您好，我是康美网络医院在线医生，很高兴为您服务!"
                    }))
                    {
                        // 记录进入诊室状态
                        enteredStates.ToCache(enteredStateCacheKey);
                        return true;
                    }
                    else
                        return false;
                }

            }
            catch (Exception e)
            {
                LogHelper.WriteError(e);
                return false;
            }
        }

        /// <summary>
        /// 添加机器人到诊室
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="identifier"></param>
        /// <param name="name"></param>
        /// <param name="photoUrl"></param>
        /// <returns></returns>
        public bool AddRobotToChannel(int channelID, int identifier = 0, string name = "医生助理", string photoUrl = "")
        {
            IMHelper imService = new IMHelper();
            ConversationRoomService roomService = new ConversationRoomService();

            if (!imService.AddGroupMember(channelID, new List<int>() { identifier }))
                return false;

            //新增群组成员
            if (!roomService.GetChannelUsersInfo(channelID).Exists(x => x.identifier == identifier))
            {
                if (!roomService.InsertChannelMembers(channelID, new List<RequestChannelMemberDTO>() { new RequestChannelMemberDTO() {
                    Identifier = identifier,
                    UserID = "",
                    UserMemberID = "",
                    UserType = EnumUserType.SysRobot,
                    PhotoUrl = photoUrl,
                    UserENName = name,
                    UserCNName = name
                }}))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
