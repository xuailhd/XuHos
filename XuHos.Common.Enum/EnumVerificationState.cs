using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 认证状态（0-未认证、1-已通过、2-未通过、3-认证中、4-第三方认证中）
    /// </summary>
    [Description("账户状态")]
    public enum EnumVerificationState
    {
        /// <summary>
        /// 未认证
        /// </summary>
        [Description("未认证")]
        Unverified = 0,
        /// <summary>
        /// 已通过
        /// </summary>
        [Description("已通过")]
        Passed = 1,
        /// <summary>
        /// 未通过
        /// </summary>
        [Description("未通过")]
        Unpassed = 2,
        /// <summary>
        /// 认证中
        /// </summary>
        [Description("认证中")]
        Verifying = 3,
        /// <summary>
        /// 第三方认证中
        /// </summary>
        [Description("第三方认证中")]
        Verifying2 = 4

    }
}
