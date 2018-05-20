using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 账户状态(0-正常、1-冻结)
    /// </summary>
    [Description("账户状态")]
    public enum EnumAccountState
    {
        /// <summary>
        /// 用户状态
        /// </summary>
        [Description("正常")]
        Normal = 0,
        /// <summary>
        /// 用户状态
        /// </summary>
        [Description("冻结")]
        Freeze = 1

    }
}
