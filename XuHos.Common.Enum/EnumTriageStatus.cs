using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 分诊状态（0无，1待分诊，2分诊中，3已分诊）
    /// </summary>
    [Description("分诊状态")]
    public enum EnumTriageStatus
    {

        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,

        /// <summary>
        /// 待分诊
        /// </summary>
        [Description("待分诊")]
        WaitTriage = 1,

        /// <summary>
        /// 分诊中
        /// </summary>
        [Description("分诊中")]
        Triaging = 2,

        /// <summary>
        /// 已分诊
        /// </summary>
        [Description("已分诊")]
        Triaged = 3

    }
}
