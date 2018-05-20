using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus;

namespace XuHos.Service.EventHandlers
{
    public static class BundleConfig
    {
        static XuHos.EventBus.MQChannel channel = new EventBus.MQChannel();

        static XuHos.BLL.Sys.Implements.SysEventService eventService = new BLL.Sys.Implements.SysEventService();

        public static void Register()
        {
            channel.IfAckThen((eventId, queueName) =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(eventId))
                    {
                        eventService.UpdateFinishedState(eventId, queueName);
                    }
                }
                catch (Exception ex)
                {
                    XuHos.Common.LogHelper.WriteError(ex);
                }

            }).IfNackThen((eventId, queueName, ex, eventObj) =>
            {
                var requeue = true;

                try
                {
                    if (ex != null)
                    {
                        XuHos.Common.LogHelper.WriteError(ex);
                    }

                    if (!string.IsNullOrEmpty(eventId))
                    {
                        //记录重试次数(在阀值内则重新写队列)
                        requeue = eventService.IncrementRetryCount(eventId, queueName);

                    }
                    else
                    {
                        //触发消息（DB赤持久化）
                        if (eventService.TriggerEvent(eventObj))
                        {
                            //消息不需要重新写入队列
                            requeue = false;
                        }
                    }
                }
                catch
                {
                    XuHos.Common.LogHelper.WriteError(ex);
                }

                //打印错误日志，然后重新写入队列
                return requeue;
            });



            #region UserOperatorLogEvent

            channel.Subscribe<XuHos.EventBus.Events.UserOperatorLogEvent>(new UserOperatorLogEvent.DefaultHandler());


            #endregion

            #region UserOnlineStateChangedEvent

            channel.Subscribe<XuHos.EventBus.Events.UserOnlineStateChangedEvent>(new UserOnlineStateChangedEvent.DefaultHandler());


            #endregion

            #region  UserRegisterEvent

            channel.Subscribe<XuHos.EventBus.Events.UserRegisteredEvent>(new UserRegisterEvent.SendRegisterSMS());

            channel.Subscribe<XuHos.EventBus.Events.UserRegisteredEvent>(new UserRegisterEvent.SyncIMUid());

            #endregion

            #region ChannelStateChangedEvent

            channel.Subscribe<XuHos.EventBus.Events.ChannelStateChangedEvent>(new ChannelStateChangedEvent.DefaultHandler());

            #endregion

            #region ChanneCreateEvent
            channel.Subscribe<XuHos.EventBus.Events.ChannelCreateEvent>(new ChanneCreateEvent.DefaultHandler());
            #endregion

            #region  ChannelNewMsgEvent
            channel.Subscribe<XuHos.EventBus.Events.ChannelNewMsgEvent>(new ChannelNewMsgEvent.DefaultHandler());
            #endregion

            #region ChannelSendGroupMsgEvent
            channel.Subscribe<EventBus.Events.ChannelSendGroupMsgEvent<BLL.Sys.DTOs.Request.RequestIMCustomMsgSurvey>>(new ChannelSendGroupMsgEvent.IfCustomMsgSurvey());
            channel.Subscribe<EventBus.Events.ChannelSendGroupMsgEvent<BLL.Sys.DTOs.Request.RequestCustomMsgRoomDurationChanged>>(new ChannelSendGroupMsgEvent.IfRoomDurationChangedGroupMsg());
            channel.Subscribe<EventBus.Events.ChannelSendGroupMsgEvent<string>>(new ChannelSendGroupMsgEvent.IfTextMsg());
            channel.Subscribe<EventBus.Events.ChannelSendGroupMsgEvent<DTO.ResponseUserFileDTO>>(new ChannelSendGroupMsgEvent.IfImageMsg());
            #endregion

            #region ChannelChargingEvent

            channel.Subscribe<XuHos.EventBus.Events.ChannelChargingEvent>(new ChannelChargingEvent.DefaultHandler());


            #endregion

            #region ChannelTriageChangedEvent
            channel.Subscribe<XuHos.EventBus.Events.ChannelTriageChangeEvent>(new ChannelTriageChangedEvent.DefaultHandler());
            #endregion

            #region ChannelC2CCreateEvent
            channel.Subscribe<XuHos.EventBus.Events.ChannelC2CCreateEvent>(new ChannelC2CCreateEvent.DefaultHandler());
            #endregion

            #region ChannelEnteredEvent
            channel.Subscribe<XuHos.EventBus.Events.ChannelEnteredEvent>(new ChannelEnteredEvent.IfDoctorEntered());
            #endregion

            #region SMSSendEvent
            //channel.Subscribe<XuHos.EventBus.Events.SMSSendEvent>(new SMSSendEvent.InsertLog());

            channel.Subscribe<XuHos.EventBus.Events.SMSSendEvent>(new SMSSendEvent.SendSMS());

            #endregion

            #region OrderCreateEvent

            channel.Subscribe<XuHos.EventBus.Events.OrderCreateEvent>(new OrderCreateEvent.DefaultHandler());


            #endregion

            #region OrderPayNotifyEvent

            channel.Subscribe<XuHos.EventBus.Events.OrderPayNotifyEvent>(new OrderPayNotifyEvent.DefaultHandler());

            #endregion

            #region OrderCancelEvent

            channel.Subscribe<XuHos.EventBus.Events.OrderCanceledEvent>(new OrderCanceledEvent.IfOneButtonConsult());

            #endregion

            #region OrderPayCompletedEvent
            channel.Subscribe<XuHos.EventBus.Events.OrderPayCompletedEvent>(new OrderPayCompletedEvent.SendBizNotice());


            channel.Subscribe<XuHos.EventBus.Events.OrderPayCompletedEvent>(new OrderPayCompletedEvent.IfRenewUpgradePublishChannelDurationChangeEvent());


            channel.Subscribe<XuHos.EventBus.Events.OrderPayCompletedEvent>(new OrderPayCompletedEvent.IfRenewUpgradeUpdateOrderDetail());

            channel.Subscribe<XuHos.EventBus.Events.OrderPayCompletedEvent>(new OrderPayCompletedEvent.OrderLogisticDelivery());

            channel.Subscribe<XuHos.EventBus.Events.OrderPayCompletedEvent>(new OrderPayCompletedEvent.PublishChanneCreateEvent());

            channel.Subscribe<XuHos.EventBus.Events.OrderPayCompletedEvent>(new OrderPayCompletedEvent.IfOneButtonConsult());

            channel.Subscribe<XuHos.EventBus.Events.OrderPayCompletedEvent>(new OrderPayCompletedEvent.UpdateMonitorIndex());


            #endregion

            #region OrderEvaluationEvent
            channel.Subscribe<XuHos.EventBus.Events.OrderEvaluationEvent>(new OrderEvaluationEvent.InsertEvaluation());
            #endregion

            #region OrderEvaluationCompletedEvent

            channel.Subscribe<XuHos.EventBus.Events.OrderEvaluationCompletedEvent>(new OrderEvaluationCompletedEvent.UpdateOrderEvaluationState());
            #endregion

            #region OrderCompletedEvent

            channel.Subscribe<XuHos.EventBus.Events.OrderCompletedEvent>(new OrderCompletedEvent.DefaultHandler());

            channel.Subscribe<XuHos.EventBus.Events.OrderCompletedEvent>(new OrderCompletedEvent.IfOneButtonConsult());

            #endregion

            #region OrderCanceledEvent
            channel.Subscribe<XuHos.EventBus.Events.OrderCanceledEvent>(new OrderCanceledEvent.DefaultHandler());
            #endregion OrderCanceledEvent

            #region OrderCancelingEvent
            channel.Subscribe<XuHos.EventBus.Events.OrderCancelingEvent>(new OrderCancelingEvent.DefaultHandler());
            #endregion OrderCancelingEvent

            #region OrderRefundEvent
            channel.Subscribe<XuHos.EventBus.Events.OrderRefundEvent>(new OrderRefundEvent.DefaultHandler());
            #endregion OrderRefundEvent

            #region OrderLogisticStateChangeEvent

            channel.Subscribe<XuHos.EventBus.Events.OrderLogisticStateChangeEvent>(new OrderLogisticStateChangeEvent.DefaultHandler());

            #endregion
            //新增的处理程序请依次注册，后续版本考虑自动加载处理
        }
    }
}
