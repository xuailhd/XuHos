using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 订单状态（state：-1=待确认、0=待支付、1=已支付、2=已完成、3=已取消）
    /// </summary>
    [Description("订单状态")]
    public enum EnumOrderState
    {
        [Description("待确认")]
        /// <summary>
        /// 待支付
        /// </summary>
        NoConfirm = -1,
        [Description("待支付")]
        /// <summary>
        /// 待支付
        /// </summary>
        NoPay = 0,
        /// <summary>
        /// 已支付
        /// </summary>
        [Description("已支付")]
        Paid = 1,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Finish = 2,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Canceled = 3
    }
}
