using System.ComponentModel;

namespace XuHos.Common.Enum
{

    /// <summary>
    /// 分诊叫号类型
    /// </summary>
    [Description("分诊叫号类型")]
    public enum EnumTriageCallType
    {
        /// <summary>
        /// 呼叫
        /// </summary>
        [Description("呼叫")]
        Call = 0,
        /// <summary>
        /// 看诊
        /// </summary>
        [Description("看诊")]
        In =1,
        /// <summary>
        /// 过号
        /// </summary>
        [Description("过号")]
        Ignore = 2
    }
}
