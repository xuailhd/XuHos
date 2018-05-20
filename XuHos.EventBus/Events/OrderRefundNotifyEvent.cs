using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 订单退款通知时间
    
    /// 日期：2017年4月12日
    /// </summary>
    public class OrderRefundNotifyEvent: BaseEvent, IEvent
    {
        public string OrderNo { get; set; }

        public EnumPayType PayType { get; set; }
    }
}
