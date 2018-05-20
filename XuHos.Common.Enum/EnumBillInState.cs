using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 收费单状态（0=待支付，1=已支付，2=已退款）
    /// </summary>
    [Description("收费单状态")]    
    public enum EnumBillInState
    {
     
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
        /// 申请退款
        /// </summary>
        [Description("申请退款")]
        applyRefund = 2,
        /// <summary>
        /// 已退款
        /// </summary>
        [Description("已退款")]
        Refunded = 3
    }
}
