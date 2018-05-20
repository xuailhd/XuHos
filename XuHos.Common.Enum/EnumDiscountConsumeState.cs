using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 折扣消费状态（state：0=未消费、1=已消费、2=已取消）
    /// </summary>
    [Description("折扣消费状态")]
    public enum EnumDiscountConsumeState
    {
        [Description("未消费")]
        /// <summary>
        /// 待支付
        /// </summary>
        NoPay = 0,
        /// <summary>
        /// 已支付
        /// </summary>
        [Description("已消费")]
        Paid = 1,
        /// <summary>
        /// 已经取消
        /// </summary>
        [Description("已取消")]
        Canceled = 2
    }
}
