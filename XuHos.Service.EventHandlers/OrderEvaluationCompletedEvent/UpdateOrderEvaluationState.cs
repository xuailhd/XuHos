using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL;
using XuHos.Common.Cache;

namespace XuHos.Service.EventHandlers.OrderEvaluationCompletedEvent
{
    /// <summary>
    /// 默认处理订单评价状态
    /// </summary>
    public class UpdateOrderEvaluationState : IEventHandler<EventBus.Events.OrderEvaluationCompletedEvent>
    {
        XuHos.BLL.OrderService orderService = new OrderService("");

        public bool Handle(EventBus.Events.OrderEvaluationCompletedEvent evt)
        {
            if (evt == null)
                return true;

            try
            {
                var order = orderService.Single<XuHos.Entity.Order>(a => a.OrderOutID == evt.OuterID);
                if (order != null)
                {
                    order.IsEvaluated = true;
                    var result = orderService.Update(order);
                    if (result == true)
                    {
                        var CacheKey_Order = new XuHos.Common.Cache.Keys.EntityCacheKey<DTO.Platform.OrderDTO>(XuHos.Common.Cache.Keys.StringCacheKeyType.Order, order.OrderNo);
                        CacheKey_Order.RemoveCache();
                    }
                    return result;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Common.LogHelper.WriteError(ex);
            }

            return false;
        }
    }
}
