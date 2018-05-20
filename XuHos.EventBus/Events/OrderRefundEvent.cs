using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{

    /// <summary>
    /// 订单退款
    /// </summary>
    public class OrderRefundEvent: BaseEvent, IEvent
    {
        public string OrderNo { get; set; }

        public string TradeNo { get; set; }

        public EnumPayType PayType { get; set; }
    }
}
