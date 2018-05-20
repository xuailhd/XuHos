using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 交易状态 WAIT_BUYER_PAY（交易创建，等待买家付款）、TRADE_CLOSED（未付款交易超时关闭，或支付完成后全额退款）、TRADE_SUCCESS（交易支付成功）、TRADE_FINISHED（交易结束，不可退款）
    /// </summary>
    public enum EnumTradeState
    {
        TRADE_NOT_EXIST=-1,
        WAIT_BUYER_PAY = 0,
        TRADE_CLOSED = 1,
        TRADE_SUCCESS = 2,
        TRADE_FINISHED = 3,

    }
}
