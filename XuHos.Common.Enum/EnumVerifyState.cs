using System.ComponentModel;

namespace XuHos.Common.Enum
{
    [Description("提现审批状态")]
    public enum EnumVerifyState
    {

        /// <summary>
        /// 已申请
        /// </summary>
        Apply = 1,

        /// <summary>
        /// 已通过
        /// </summary>
        Pass = 2,

        /// <summary>
        /// 已驳回
        /// </summary>
        Reject = 3

    }
}
