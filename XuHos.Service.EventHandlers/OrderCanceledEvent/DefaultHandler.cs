using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL;
using XuHos.Common.Cache;
using XuHos.Common.Enum;

namespace XuHos.Service.EventHandlers.OrderCanceledEvent
{
    /// <summary>
    /// 订单取消后开始退款
    
    /// 日期：2017年6月18日
    /// </summary>
    public class DefaultHandler : IEventHandler<EventBus.Events.OrderCanceledEvent>
    {
        OrderService service = new OrderService("");

        public bool Handle(EventBus.Events.OrderCanceledEvent evt)
        {
            try
            {
                if (evt == null)
                {
                    return true;
                }

                //查询出所有交易日志
                var logs = service.QueryOrderTradeLogs(evt.OrderNo);

                using (XuHos.EventBus.MQChannel mqChannel = new MQChannel())
                {
                    mqChannel.BeginTransaction();

                    //循环所哟交易日志
                    foreach (var trade in logs)
                    {
                        //如果不是线下付款的方式，并且已经交易成功了则开始退款
                        if (trade.PayType != EnumPayType.OfflinePay && 
                            trade.TradeStatus != EnumTradeState.TRADE_NOT_EXIST &&
                            trade.TradeStatus != EnumTradeState.WAIT_BUYER_PAY)
                        {
                            mqChannel.Publish<EventBus.Events.OrderRefundEvent>(new EventBus.Events.OrderRefundEvent()
                            {
                                OrderNo = trade.OrderNo,
                                PayType = trade.PayType,
                                TradeNo = trade.TradeNo
                            });
                        }
                      
                        
                    }

                    mqChannel.Commit();
                    return true;
                }
              
            }
            catch (Exception E)
            {
                XuHos.Common.LogHelper.WriteError(E);
               
            }

            return false;
        }
    }
}
