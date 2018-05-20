using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 订单物流状态变更
    /// </summary>
    public class OrderLogisticStateChangeEvent: BaseEvent, IEvent
    {
        public string OrderNo { get; set; }

        public EnumLogisticState? LogisticState { get; set; }

        public string LogisticCompanyName { get; set; }

        public string LogisticWayBillNo { get; set; }
    }
}
