using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 订单付款通知
    
    /// 日期：2017年4月12日
    /// </summary>
    public class OrderPayNotifyEvent: BaseEvent, IEvent
    {
        public string OrderNo { get; set;}

        public string TradeNo { get; set; }

        public EnumPayType PayType { get; set; }

        public string SelllerID { get; set; }

        /// <summary>
        /// 订单机构编号
        /// </summary>
        public string OrgID { get; set; }
    }
}
