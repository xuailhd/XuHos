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

namespace XuHos.Service.EventHandlers.ChannelStateChangedEvent
{
    public class DefaultHandler : IEventHandler<EventBus.Events.ChannelStateChangedEvent>
    {
        ConversationRoomService roomService = new ConversationRoomService();
        OrderService orderService = new OrderService("");
        BLL.User.Implements.UserService userService = new BLL.User.Implements.UserService();
        BLL.Sys.Implements.ConversationIMUidService uidService = new BLL.Sys.Implements.ConversationIMUidService("");
        XuHos.Integration.QQCloudy.IMHelper imService = new XuHos.Integration.QQCloudy.IMHelper();

        public bool Handle(EventBus.Events.ChannelStateChangedEvent evt)
        {
            DTO.ConversationRoomDTO room = null;

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

                //获取房间信息
                room = roomService.GetChannelInfo(evt.ChannelID);
 
                #region  参数校验：房间不存在的则不允许修改
                if (room == null)
                {
                    return true;
                }
                #endregion

                var userInfo = userService.GetUserInfoByUserId(evt.FromUserID);

                var CurrentOperatorUserIdentifier = userInfo.identifier;

                var RoomStateChangeMsgDesc = "";
                var RoomOperatorType = "";
                var RoomOperatorRemark = "";

                //结束看诊
                if (evt.State == EnumRoomState.AlreadyVisit)
                {
                    if (userInfo.UserType == EnumUserType.Doctor)
                    {
                        #region 停止计费
                        if (room.ChargingState == EnumRoomChargingState.Started && !roomService.PauseCharging(evt.ChannelID))
                        {
                            return false;
                        }
                        #endregion

                        #region 更新订单状态

                        if (room.ServiceType == EnumDoctorServiceType.AudServiceType ||
                        room.ServiceType == EnumDoctorServiceType.VidServiceType ||
                        room.ServiceType == EnumDoctorServiceType.PicServiceType ||
                        room.ServiceType == EnumDoctorServiceType.Consultation)
                        {
                            //订单完成
                            if (!orderService.Complete("", room.ServiceID))
                            {
                                XuHos.Common.LogHelper.WriteWarn($"订单完成失败,ServiceID={room.ServiceID}");
                                return false;
                            }
                        }
                        #endregion


                        #region 更新监控指标（记录服务时长，总耗时，就诊是否结束标志）                      
                        var order = orderService.GetOrder("", room.ServiceID);
                        if (order != null)
                        {
                            SysMonitorIndexService service = new SysMonitorIndexService();
                            var values = new Dictionary<string, string>();
                            values.Add("VisitingServiceChargingState",room.ChargingState.ToString());//就诊暂停标志
                            values.Add("VisitingServiceDurationSeconds", room.Duration.ToString());//就诊服务时长
                            values.Add("VisitingServiceElapsedSeconds", room.TotalTime.ToString());//就诊消耗时长                            
                            values.Add("VisitingRoomState", room.RoomState.ToString());//就诊暂停标志
                            if (!service.InsertAndUpdate(new RequestSysMonitorIndexUpdateDTO()
                            {
                                Category = "UserConsult",
                                OutID = order.OrderNo,
                                Values = values
                            }))
                            {
                                return false;
                            }
                        }
                        #endregion

                        //语音、视频看诊
                        if (room.ServiceType == EnumDoctorServiceType.AudServiceType ||
                            room.ServiceType == EnumDoctorServiceType.VidServiceType)
                        {
                            var DoctorID = "";

                            #region 获取医生编号

                            if (room.ServiceType == EnumDoctorServiceType.AudServiceType || room.ServiceType == EnumDoctorServiceType.VidServiceType)
                            {
                                BLL.UserOPDRegisterService bllOPD = new UserOPDRegisterService("");

                                //获取预约信息
                                var opd = bllOPD.Single<UserOPDRegister>(room.ServiceID);
                                if (opd != null)
                                {
                                    DoctorID = opd.DoctorID;
                                }
                                else
                                {
                                    XuHos.Common.LogHelper.WriteWarn("房间 " + room.ChannelID + " 对应的预约记录不存在");
                                }
                            }

                            #endregion

                            var DoctorUid = uidService.GetDoctorIMUid(DoctorID);

                            #region 发送候诊队列通知
                            roomService.SendWaitingQueueChangeNotice(DoctorID);
                            #endregion
                        }

                        RoomOperatorRemark = $"";
                        RoomOperatorType = "Hangup";
                        RoomStateChangeMsgDesc = "医生已结束看诊，请对本次服务作出评价";
                    }
                    else
                    {
                        //无效请求
                        return true;
                    }
                }
                else if (evt.State == EnumRoomState.Waiting)
                {
                    //取消呼叫
                    if (userInfo.UserType == EnumUserType.Doctor)
                    {
                        RoomStateChangeMsgDesc = "医生取消了呼叫";
                        RoomOperatorType = "Call_Cancel";

                    }
                    else
                    {
                        RoomStateChangeMsgDesc = "患者正在候诊，等待医生呼叫";
                        RoomOperatorType = "Wait";

                        #region 修改状态并设置分诊编号
                        room.RoomState = evt.State;
                        room.TriageID = XuHos.Common.Utility.SeqIDHelper.GetSeqId();

                        //修改就诊时间和开始就诊时间
                        if (!roomService.CompareAndSetChannelInfo(room))
                        {
                            XuHos.Common.LogHelper.WriteWarn($"修改房间信息失败,ChannelID={room.ChannelID}");
                            return false;
                        }
                        #endregion

                        #region 发送患者进入诊室的通知
                        if (userInfo.UserType == EnumUserType.User || userInfo.UserType == EnumUserType.Drugstore)
                        {
                            using (XuHos.EventBus.MQChannel mqChannel = new MQChannel())
                            {
                                if (!mqChannel.Publish<EventBus.Events.UserNoticeEvent>(new EventBus.Events.UserNoticeEvent()
                                {                                 
                                    NoticeType = EnumNoticeSecondType.DoctorVidUserEnterRoomNotice,
                                    ServiceID = room.ServiceID                             
                                }))
                                {
                                    XuHos.Common.LogHelper.WriteWarn($"Publis UserNoticeEvent失败,ServiceID={room.ServiceID}");
                                    return false;
                                }
                            }
                        }
                        #endregion
                    }
                }
                else if (evt.State == EnumRoomState.Calling)
                {
                    //医生呼叫患者
                    if (userInfo.UserType == EnumUserType.Doctor)
                    {
                        RoomOperatorType = "Calling";
                        RoomStateChangeMsgDesc = "医生正在呼叫，等待患者接听";

                        using (XuHos.EventBus.MQChannel mqChannel = new MQChannel())
                        {
                            if (!mqChannel.Publish<EventBus.Events.UserNoticeEvent>(new EventBus.Events.UserNoticeEvent()
                            {
                                NoticeType = EnumNoticeSecondType.UserVidDoctorCallNotice,
                                ServiceID = room.ServiceID
                            }))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //无效请求
                        return true;
                    }
                }
                else if (evt.State == EnumRoomState.InMedicalTreatment)
                {
                    if (userInfo.UserType == EnumUserType.Doctor)
                    {
                        //无效请求
                        return true;
                    }
                    //患者接听
                    else
                    {
                        RoomOperatorType = "Calling_Answer";
                        RoomStateChangeMsgDesc = "患者已进入诊室";

                        #region 修改订单状态，用户将不能够再取消订单                 

                        if (room.ServiceType == EnumDoctorServiceType.AudServiceType ||
                        room.ServiceType == EnumDoctorServiceType.VidServiceType)
                        {
                            #region 语音/视频 修改订单状态（配送中）                           
                            if (!orderService.LogisticWithDistributionIn(room.ServiceID))
                                return false;
                            #endregion
                        }
                        else if (room.ServiceType == EnumDoctorServiceType.PicServiceType)
                        {
                            #region 图文咨询 修改订单状态（配送中）

                            BLL.OrderService bllOrder = new OrderService(evt.FromUserID);
                            if (!bllOrder.LogisticWithDistributionIn(room.ServiceID))
                                return false;
                            #endregion

                        }

                        #endregion
                     
                        #region 计算候诊耗时并更新统计指标
                        var log = roomService.GetChannelLastLog(room.ConversationRoomID, "Wait");
                        var order = orderService.GetOrder("", room.ServiceID);
                        if (log != null && order!=null)
                        {
                            //候诊耗时
                            var WaitingElapsedSeconds = (DateTime.Now - log.OperationTime).TotalSeconds;

                            RoomOperatorRemark = $"用户候诊用时{WaitingElapsedSeconds}秒";

                            SysMonitorIndexService service = new SysMonitorIndexService();
                            var values = new Dictionary<string, string>();                    
                            values.Add("WaitingElapsedSeconds", WaitingElapsedSeconds.ToString());

                            //更新候诊总耗时，指标=原指标+新指标
                            if (!service.InsertAndUpdate(new RequestSysMonitorIndexUpdateDTO()
                            {
                                Category = "UserConsult",
                                OutID = order.OrderNo,
                                Values = values
                            },true))
                            {
                                return false;
                            }
                        }

                     
                        #endregion
                    }
                }
                else if (evt.State == EnumRoomState.NoTreatment)
                {
                    if (userInfo.UserType == EnumUserType.Doctor)
                    {
                        RoomOperatorType = "Calling_Cancel";
                        RoomStateChangeMsgDesc = "医生取消了呼叫";
                    }
                    else
                    {
                        RoomOperatorType = "Waiting_Cancel";
                        RoomStateChangeMsgDesc = "患者取消了候诊";
                    }

                    //发送指令前状态是在就诊中
                    if (evt.ExpectedState == EnumRoomState.InMedicalTreatment)
                    {
                        #region 停止计费
                        if (room.ChargingState == EnumRoomChargingState.Started && !roomService.PauseCharging(evt.ChannelID))
                        {
                            return false;
                        }
                        #endregion
                    }

                }
                else if (evt.State == EnumRoomState.Disconnection)
                {
                    if (userInfo.UserType == EnumUserType.Doctor)
                    {
                        RoomOperatorType = "Leave";
                        RoomStateChangeMsgDesc = "医生已离开";
                    }
                    else
                    {
                        RoomOperatorType = "Leave";
                        RoomStateChangeMsgDesc = "患者已离开";
                    }

                    #region 停止计费
                    
                    if (room.ChargingState== EnumRoomChargingState.Started && !roomService.PauseCharging(evt.ChannelID))
                    {
                        return false;
                    }
                    #endregion
                    
                }
                else if(evt.State == EnumRoomState.WaitAgain)
                {
                    RoomOperatorType = "Waiting";
                    RoomStateChangeMsgDesc = "患者正在候诊，等待医生呼叫";
                }
               
                if (room.Enable)
                {
                    var State = room.RoomState;

                    //兼容移动端状态
                    if (State == EnumRoomState.WaitAgain)
                    {
                        State = EnumRoomState.Waiting;
                    }

                    ///写入日志
                    if (!roomService.InsertChannelLog(room.ConversationRoomID,
                        userInfo.UserID,
                        userInfo.UserCNName,
                        RoomOperatorType, 
                        RoomStateChangeMsgDesc,
                        RoomOperatorRemark))
                    {
                        return false;
                    }

                    if (!imService.SendGroupCustomMsg(evt.ChannelID, CurrentOperatorUserIdentifier, new BLL.Sys.DTOs.Request.RequestCustomMsgRoomStateChanged()
                    {
                        Data = new RequestConversationRoomStatusDTO()
                        {
                            ChannelID = evt.ChannelID,
                            State = State,
                            ServiceID = room.ServiceID,
                            ServiceType = room.ServiceType,
                            Duration = room.Duration,
                            ChargingState=room.ChargingState,
                            TotalTime=room.TotalTime,
                            DisableWebSdkInteroperability=room.DisableWebSdkInteroperability
                        },
                        Desc = RoomStateChangeMsgDesc
                    }))
                    {
                        return false;
                    }
                    
                }
            }
            catch (XuHos.Integration.QQCloudy.InvalidGroupException)
            {
                if (room != null)
                {
                    room.Enable = false;
                    if (roomService.CompareAndSetChannelInfo(room))
                    {
                        new XuHos.Service.EventHandlers.ChanneCreateEvent.DefaultHandler().Handle(new EventBus.Events.ChannelCreateEvent()
                        {
                            ChannelID = evt.ChannelID,
                            ServiceID = room.ServiceID,
                            ServiceType = room.ServiceType

                        });
                    }
                }

                return false;

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
