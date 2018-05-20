using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 用户状态(-1=无效，0-正常、1-冻结、2-销户)
    /// </summary>
    [Description("用户状态")]
    public enum EnumUserState
    {
        /// <summary>
        /// 无效
        /// </summary>
        [Description("未开通")]
        Disabled = -1,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        /// 冻结
        /// </summary>
        [Description("冻结")]
        Freeze = 1,
        /// <summary>
        /// 销户
        /// </summary>
        [Description("销户")]
        Cancellation = 2
    }
}
