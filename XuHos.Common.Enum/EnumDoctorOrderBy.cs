using System.ComponentModel;

namespace XuHos.Common.Enum
{

    /// <summary>
    /// 医生列表查询的排序字段 
    /// </summary>
    [Description("医生列表查询的排序字段")]
    public enum EnumDoctorOrderBy
    {

        /// <summary>
        /// 综合
        /// </summary>
        [Description("综合")]
        Together = 0,

        /// <summary>
        /// 好评量
        /// </summary>
        [Description("好评量")]
        CommentedNum = 1,

        /// <summary>
        /// 问诊量
        /// </summary>
        [Description("问诊量")]
        DiagnoseNum = 2,

        /// <summary>
        /// 价格
        /// </summary>
        [Description("价格")]
        ServicePrice = 3,

        /// <summary>
        /// 有无排班
        /// </summary>
        [Description("有无排班")]
        ScheduleStatus = 4,

        /// <summary>
        /// 有无服务套餐
        /// </summary>
        [Description("有无服务套餐")]
        PackageStatus = 5,

        /// <summary>
        /// 评分
        /// </summary>
        [Description("评分")]
        EvaluationScore = 6,

        /// <summary>
        /// 问诊回复数量
        /// </summary>
        [Description("问诊回复数量")]
        RepliedCount = 7

    }
}
