using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 订单完成
    /// </summary>
    public class OrderCompletedEvent: BaseEvent, IEvent
    {
        public string OrderNo { get; set; }
    }
}
