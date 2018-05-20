using System.ComponentModel;

namespace XuHos.Common.Enum
{
    [Description("交易类型")]
    public enum EnumTransType
    {

        /// <summary>
        /// 收入
        /// </summary>
        [Description("收入")]
        Income = 1,

        /// <summary>
        /// 充值
        /// </summary>
        [Description("充值")]
        Recharge = 2,

        /// <summary>
        /// 消费
        /// </summary>
        [Description("消费")]
        Consume = 3,

        /// <summary>
        /// 提现
        /// </summary>
        [Description("提现")]
        Cash = 4,

        /// <summary>
        /// 退款
        /// </summary>
        [Description("退款")]
        Refund = 5,

        /// <summary>
        /// 中转
        /// </summary>
        [Description("中转")]
        Transfer = 6,

        /// <summary>
        /// 补贴
        /// </summary>
        [Description("补贴")]
        Subsidy = 7

    }
}
