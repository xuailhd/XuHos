using System.ComponentModel;

namespace XuHos.Common.Enum
{
    [Description("会诊进度")]
    public enum EnumConsultationProgress
    {

        /// <summary>
        /// 待处理
        /// </summary>
        [Description("待处理")]
        Pending = 0,

        /// <summary>
        /// 已派单给主诊医生
        /// </summary>
        [Description("已派单")]
        Dispatch = 1,

        /// <summary>
        /// 申请会诊专家
        /// </summary>
        [Description("申请会诊专家")]
        Specialist = 2,

        /// <summary>
        /// 待会诊(会诊就绪)
        /// </summary>
        [Description("待会诊")]
        Arranged = 3,

        /// <summary>
        /// 会诊中
        /// </summary>
        [Description("会诊中")]
        InProgress = 4,

        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Finished = 5,

        /// <summary>
        /// 已失效
        /// </summary>
        [Description("已失效")]
        Invalid = 6

    }
}
