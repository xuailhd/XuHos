using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL.Sys.Implements;
using XuHos.Common.Cache;

namespace XuHos.Service.EventHandlers.OrderPayCompletedEvent
{
    /// <summary>
    /// 如果续费订单支付完成后则更新原订单明细
    
    /// 日期：2017年4月21日
    /// </summary>
    public class IfRenewUpgradeUpdateOrderDetail : IEventHandler<EventBus.Events.OrderPayCompletedEvent>
    {
        XuHos.BLL.OrderService orderService = new BLL.OrderService("");
        ConversationRoomService roomService = new ConversationRoomService();

        public bool Handle(EventBus.Events.OrderPayCompletedEvent evt)
        {
            if (evt != null && evt.OrderNo != "" && evt.OrderType == Common.Enum.EnumProductType.RenewUpgrade)
            {
                var LockName = $"{typeof(IfRenewUpgradeUpdateOrderDetail)}:{evt.OrderNo}";
                var lockValue = Guid.NewGuid().ToString("N");

                //获取分布式锁，获取锁失败时进行锁等待（锁超时时间5秒）
                if (LockName.Lock(lockValue, TimeSpan.FromSeconds(5)))
                {
                    try
                    {
                        XuHos.Common.LogHelper.WriteDebug($"订单续费:订单{evt.OrderNo}支付完成,需更新原订单明细");

                        //通过外部订单编号查询原订单信息
                        var origOrder = orderService.GetOrder(evt.OrderOutID);

                        if (origOrder != null)
                        {
                            var room = roomService.GetChannelInfo(origOrder.OrderOutID);

                            //服务倒计时结束或者已经就诊
                            if (room != null && (room.Duration - room.TotalTime <= 0 || room.RoomState == Common.Enum.EnumRoomState.AlreadyVisit))
                            {
                                XuHos.Common.LogHelper.WriteDebug($"订单续费：订单{evt.OrderNo}支付完成，就诊已经结束,原订单已经完成需申请退款");
                                return orderService.Cancel(evt.OrderNo);
                            }
                            //当原订单已经完成时，用户对订单进行续费那么则进行退款
                            else if (origOrder.OrderState == Common.Enum.EnumOrderState.Finish)
                            {
                                XuHos.Common.LogHelper.WriteDebug($"订单续费：订单{evt.OrderNo}支付完成，原订单已经完成需申请退款");
                                var order = orderService.GetOrder(evt.OrderNo);
                                if (order.OrderState == Common.Enum.EnumOrderState.Finish)
                                {
                                    return true;
                                }
                                else
                                {
                                    return orderService.Cancel(evt.OrderNo);
                                }
                            }
                            else
                            {
                                XuHos.Common.LogHelper.WriteDebug($"订单续费:{evt.OrderNo}支付完成，修改原订单明细");
                                return orderService.RenewUpgradePayCompleted(evt.OrderNo);
                            }
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
                    //获取锁失败，进行重试
                    return false;
                }
            }

            return true;
        }
    }
}
