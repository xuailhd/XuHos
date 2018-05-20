using System.ComponentModel;

namespace XuHos.Common.Enum
{

    /// <summary>
    /// 审核类型，1：退款；2：已注销作废
    /// </summary>
    [Description("审核类型")]
    public enum EnumAuditType
    {
        /// <summary>
        /// 退款；
        /// </summary>
        [Description("退款")]    
        Drawback = 1,
        /// <summary>
        /// 已注销作废
        /// </summary>
        [Description("已注销作废")]
        BlankOut = 2,
    }
}
