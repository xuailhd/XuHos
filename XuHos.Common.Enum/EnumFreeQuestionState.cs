using System.ComponentModel;

namespace XuHos.Common.Enum
{

    /// <summary>
    /// 免费问题状态（0-未领取、1-已领取、2-已完成）
    /// </summary>
    [Description("免费问题状态")]
    public enum EnumFreeQuestionState
    {
        /// <summary>
        /// 未领取
        /// </summary>
        [Description("未领取")]
        Unclaimed = 0,
        /// <summary>
        /// 已领取
        /// </summary>
        [Description("已领取")]
        Receive = 1,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Finish = 2
    }
}