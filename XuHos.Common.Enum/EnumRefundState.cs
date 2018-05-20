using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 退款状态 0=未退款,1=申请退款,2=已退款,3=拒绝退款,4=退款中
    /// </summary>
    [Description("退款类型")]    
    public enum EnumRefundState
    {
        /// <summary>
        /// 未退款
        /// </summary>
        [Description("未退款")]
        NoRefund = 0,
        /// <summary>
        /// 申请退款
        /// </summary>
        [Description("申请退款")]
        applyRefund = 1,
        /// <summary>
        /// 已退款
        /// </summary>
        [Description("已退款")]
        Refunded = 2,
        /// <summary>
        /// 拒绝退款
        /// </summary>
        [Description("拒绝退款")]
        Refuse = 3,
        /// <summary>
        /// 退款中
        /// </summary>
        [Description("退款中")]
        Refunding = 4,

    }
}
