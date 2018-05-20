using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL;
using XuHos.BLL.Sys.Implements;
using XuHos.Common.Cache;

namespace XuHos.Service.EventHandlers.OrderPayCompletedEvent
{
    /// <summary>
    /// 如果续费订单支付完成后则发布频道服务时长变更事件
    
    /// 日期：2017年4月20日
    /// 
    /// 前置条件：订单支付完成
    /// 后置条件：发送频道服务时长变更事件
    /// </summary>
    public class IfRenewUpgradePublishChannelDurationChangeEvent : IEventHandler<EventBus.Events.OrderPayCompletedEvent>
    {
        ConversationRoomService roomService = new ConversationRoomService();
        OrderService orderService = new OrderService("");

        public bool Handle(EventBus.Events.OrderPayCompletedEvent evt)
        {
            if (evt != null && evt.OrderNo != "" && evt.OrderType == Common.Enum.EnumProductType.RenewUpgrade)
            {
                var LockName = $"{typeof(IfRenewUpgradePublishChannelDurationChangeEvent)}:{evt.OrderNo}";

                var lockValue = Guid.NewGuid().ToString("N");

                //获取分布式锁，获取锁失败时进行锁等待（锁超时时间5秒）
                if (LockName.Lock(lockValue, TimeSpan.FromSeconds(5)))
                {
                    try
                    {
                        //当前续费订单外部订单编号，是原始订单的订单编号
                        var renewUpgradeOrder = orderService.GetOrder(evt.OrderNo);

                        if (renewUpgradeOrder != null)
                        {
                            XuHos.Common.LogHelper.WriteDebug($"订单续费:订单{evt.OrderNo}支付完成，需发布服务时长变更通知");

                            //当前续费订单外部订单编号，是原始订单的订单编号
                            var origOrder = orderService.GetOrder(renewUpgradeOrder.OrderOutID);

                            if (origOrder != null)
                            {

                                var Duration = 0;
                                #region 计算时长(如果续费订单是通过套餐购买则时长则按照套餐中定义的，如果不是则按照系统的时长计算)
                                if (Duration <= 0)
                                {
                                    if (origOrder.OrderType == Common.Enum.EnumProductType.video || origOrder.OrderType == Common.Enum.EnumProductType.Phone)
                                    {
                                        //30分钟
                                        Duration = 60 * 30;
                                    }
                                    else
                                    {
                                        //一天
                                        Duration = 60 * 60 * 24;
                                    }
                                }
                                #endregion

                                var room = roomService.GetChannelInfo(origOrder.OrderOutID);

                                if (room != null && room.RoomState == Common.Enum.EnumRoomState.AlreadyVisit)
                                {
                                    XuHos.Common.LogHelper.WriteDebug($"订单续费：订单{evt.OrderNo}支付完成，就诊已经结束，续费操作无效");

                                    //忽略无效的续费
                                    return true;
                                }
                                //原订单已经完成，这时候续费无效。续费订单会进行退款处理
                                else if (origOrder.OrderState == Common.Enum.EnumOrderState.Finish)
                                {
                                    XuHos.Common.LogHelper.WriteDebug($"订单续费：订单{evt.OrderNo}支付完成，原订单{origOrder.OrderNo}已经完成，续费操作无效");
                                    //忽略无效的续费
                                    return true;
                                }
                                else
                                {
                                    XuHos.Common.LogHelper.WriteDebug($"订单续费：订单{evt.OrderNo}支付完成，发送服务时长变更通知");

                                    using (MQChannel channel = new MQChannel())
                                    {
                                        return channel.Publish<EventBus.Events.ChannelDurationChangeEvent>(new EventBus.Events.ChannelDurationChangeEvent()
                                        {
                                            Duration = Duration,
                                            ServiceID = origOrder.OrderOutID,
                                            OrderNo = evt.OrderNo,
                                            NewUpgradeOrderNo = evt.OrderOutID
                                        });
                                    }
                                }
                            }
                            else
                            {
                                XuHos.Common.LogHelper.WriteDebug($"订单续费：订单{evt.OrderNo}支付完成，原订单不存在");
                                return true;
                            }
                        }
                        else
                        {
                            XuHos.Common.LogHelper.WriteDebug($"订单续费：订单{evt.OrderNo}支付完成，订单不存在");
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        XuHos.Common.LogHelper.WriteError(ex);
                        return false;
                    }
                    finally
                    {
                        LockName.UnLock(lockValue);
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
