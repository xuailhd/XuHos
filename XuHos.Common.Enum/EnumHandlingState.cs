using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 提现处理状态
    /// </summary>
    [Description("提现处理状态")]
    public enum EnumHandlingState
    {
        /// <summary>
        /// 未打款
        /// </summary>
        NoTransfer = 1,

        /// <summary>
        /// 已打款
        /// </summary>
        Transfer = 2,

        /// <summary>
        /// 已驳回
        /// </summary>
        Reject = 3
    }
}
