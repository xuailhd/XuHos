using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 取消订单
    /// </summary>
    public class OrderCancelingEvent : BaseEvent, IEvent
    {
        public string OrderNo { get; set; }

        public string CancelReason { get; set; }
        public decimal? RefundFee { get; set; }

        public string UserID { get; set; }

    }
}
