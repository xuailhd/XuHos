using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 订单支付完成后
    /// </summary>
    public class OrderPayCompletedEvent: BaseEvent, IEvent
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public EnumPayType PayType { get; set; }

        /// <summary>
        /// 使用的支付特权
        /// </summary>
        public EnumPayPrivilege PayPrivilege { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public EnumProductType OrderType { get; set; }

        /// <summary>
        /// 交易编号
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 订单外部编号
        /// </summary>
        public string OrderOutID { get; set; }
    }
}
