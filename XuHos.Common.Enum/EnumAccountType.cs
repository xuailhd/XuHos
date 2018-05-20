using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 账户类型
    /// </summary>
    [Description("账户类型")]
    public enum EnumAccountType
    {
        /// <summary>
        /// 平台账户
        /// </summary>
        [Description("平台账户")]
        Platform = 0,

        /// <summary>
        /// 用户账户
        /// </summary>
        [Description("用户账户")]
        User = 1,

        /// <summary>
        /// 微信，支付宝，康美支付时生成的流水，会关联此账号
        /// </summary>
        [Description("外部账号")]
        Online = 2

    }
}
