using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    public enum EnumDereplicationType
    {
        /// <summary>
        /// 订单完成，入账
        /// </summary>
        OrderFinishAccount = 31,

        /// <summary>
        /// 取消订单，退款
        /// </summary>
        OrderCanceledAccount = 32,

        /// <summary>
        /// 订单余额已支付
        /// </summary>
        OrderBlancePayed = 33,

        /// <summary>
        /// 线下付款已支付
        /// </summary>
        OfflinePayed = 34,

        /// <summary>
        /// 线下支付退款
        /// </summary>
        OfflineRefund = 35,

        /// <summary>
        /// 订单消息通知护士
        /// </summary>
        NewOrderNoticeNurse = 36,

        /// <summary>
        /// 准备看诊
        /// </summary>
        RoomReadyClinic = 37,

    }
}
