using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 订单被取消
    /// </summary>
    public class OrderCanceledEvent : BaseEvent, IEvent
    {
        public string OrderNo { get; set; }
    }
}
