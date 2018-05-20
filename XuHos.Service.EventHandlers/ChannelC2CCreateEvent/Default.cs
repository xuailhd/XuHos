using XuHos.BLL.User.DTOs.Response;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.EventHandlers.ChannelC2CCreateEvent
{
   
    public class DefaultHandler : IEventHandler<EventBus.Events.ChannelC2CCreateEvent>
    {
        BLL.Sys.Implements.ConversationRoomService roomService = new BLL.Sys.Implements.ConversationRoomService();

        BLL.User.Implements.UserMemberService memberService = new BLL.User.Implements.UserMemberService();

        BLL.User.Implements.UserService userService = new BLL.User.Implements.UserService();

        XuHos.Integration.QQCloudy.IMHelper imHelper = new XuHos.Integration.QQCloudy.IMHelper();

    

        public bool Handle(EventBus.Events.ChannelC2CCreateEvent evt)
        {
            try
            {
                if (evt == null)
                {
                    return true;
                }

                if (string.IsNullOrEmpty(evt.FromUserID))
                {
                    return true;
                }

                if (evt.AddFriendItem == null || evt.AddFriendItem.Count <= 0)
                {
                    return true;
                }

                //获取当前用户信息
                var FromUserInfo = userService.GetUserInfoByUserId(evt.FromUserID);
                //用户存在
                if (FromUserInfo != null)
                {
                    var ToUserInfos = new Dictionary<string, ResponseUserDTO>();               
                    
                    //创建房间请求参数
                    var createC2CChannelRequest = new BLL.Sys.DTOs.Request.RequestConversactionApplyAddFriendDTO()
                    {
                        FromUserIdentifier = FromUserInfo.identifier,
                        FromUserMemberID = FromUserInfo.MemberID,
                        FromUserType = FromUserInfo.UserType,
                        FromUserID=FromUserInfo.UserID,
                        FromUserName=FromUserInfo.UserCNName,                 
                        AddFriendItem = new List<BLL.Sys.DTOs.Request.RequestConversactionApplyAddFriendDTO.AddFriendAccount>()
                    };

                    //添加群组请求参数
                    var requestParamsCreateGroup = new List<int>() { FromUserInfo.identifier };
                    //添加好友请求参数
                    var requestParamsApplyAddFriend= new List<XuHos.Integration.QQCloudy.Models.AddFriendAccount>();

                    //循环好友项
                    evt.AddFriendItem.ForEach(a =>
                    {
                        //好友好友的信息
                        var ToUserInfo = userService.GetUserInfoByUserId(a.ToUserID);

                        //好友存在
                        if (ToUserInfo != null)
                        {

                            createC2CChannelRequest.AddFriendItem.Add(new BLL.Sys.DTOs.Request.RequestConversactionApplyAddFriendDTO.AddFriendAccount()
                            {
                                AddType = "Add_Type_Both",
                                AddWording = a.AddWording,
                                ForceAddFlags =1,
                                GroupName = a.GroupName,
                                Remark = a.Remark,
                                ToUserID=ToUserInfo.UserID,
                                ToUserIdentifier = ToUserInfo.identifier,
                                ToUserMemberID = ToUserInfo.MemberID,
                                ToUserType = ToUserInfo.UserType,
                                ToUserName=ToUserInfo.UserCNName                    
                            });


                            requestParamsApplyAddFriend.Add(new XuHos.Integration.QQCloudy.Models.AddFriendAccount
                            {
                                AddWording = a.AddWording,                            
                                AddSource= "AddSource_Type_WEB",//需要前缀AddSource_Type_
                                GroupName = a.GroupName,
                                Remark = a.Remark,
                                To_Account = ToUserInfo.identifier.ToString()

                            });

                            requestParamsCreateGroup.Add(ToUserInfo.identifier);
                        }
                    });

                    //写入数据库
                    var ChannelInfoList = roomService.ApplyAddFriend(createC2CChannelRequest);

                    using (XuHos.EventBus.MQChannel mqChannel = new EventBus.MQChannel())
                    {
                        mqChannel.BeginTransaction();

                        foreach (var item in ChannelInfoList)
                        {
                            var room = item.Value;

                            //根据Uid 获取好友信息
                            var friend = requestParamsApplyAddFriend.Find(a => a.To_Account == item.Key.ToString());

                            //如果房间还未启用才调用请求否则忽略
                            if (!room.Enable)
                            {
                                //发送好友附言消息
                                if (!mqChannel.Publish<EventBus.Events.ChannelSendGroupMsgEvent<string>>(new EventBus.Events.ChannelSendGroupMsgEvent<string>()
                                {
                                    ChannelID = room.ChannelID,
                                    FromAccount = FromUserInfo.identifier,
                                    Msg = friend.AddWording
                                }))
                                {
                                    return false;
                                }

                                //发布房间创建完成的领域消息
                                if (!mqChannel.Publish<EventBus.Events.ChannelCreatedEvent>(new EventBus.Events.ChannelCreatedEvent()
                                {
                                    ChannelID = room.ChannelID,
                                    ServiceID = room.ServiceID,
                                    ServiceType = room.ServiceType

                                }))
                                {
                                    return false;
                                }

                                //新增好友
                                if (!imHelper.ApplyAddFriend(FromUserInfo.identifier.ToString(), requestParamsApplyAddFriend))
                                {
                                    return false;
                                }

                                //创建群组
                                if (!imHelper.CreateGroup(room.ChannelID, room.ChannelID.ToString(), room.ServiceType, requestParamsCreateGroup, "", ""))
                                {
                                    return false;
                                }

                                //设置房间已经启用
                                room.Enable = true;

                                //更新房间状态
                                if (!roomService.CompareAndSetChannelInfo(room))
                                {
                                    return false;
                                }
                            }
                        }

                        mqChannel.Commit();
                    }

                    return true;
                }
            }
            catch (Exception E)
            {
                LogHelper.WriteError(E);
            }

            return false;
        }
    }


}
