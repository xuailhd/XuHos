
using System.ComponentModel;
namespace XuHos.Common.Enum
{
    /// <summary>
    /// 审方状态：0 草稿  1 提交等待处理 2 Job已筛选待审方 -2 job筛选驳回 3 审方通过待领取 
    /// -3  审方驳回 4 医生已领取 5 已提交签名 -5 医生驳回 6 医生已签名
    /// 草稿 -》  等待处理 -》Job 筛选通过/不通过 -》审方通过/不通过 -》 医生领取 -》驳回/签名/提交 -》完成
    /// </summary>
    public enum EnumRecipeVerifyState
    {
        /// <summary>
        /// 草稿
        /// </summary>
        [Description("草稿")]
        Draft = 0,

        /// <summary>
        /// 处方提交审方平台 提交等待处理
        /// </summary>
        [Description("提交等待处理")]
        Submited = 1,


        /// <summary>
        /// Job已筛选待审方
        /// </summary>
        [Description("已筛选待审方")]
        Filtered = 2,

        /// <summary>
        ///  -2 job筛选驳回
        /// </summary>
        [Description("筛选驳回")]
        FilterFailed = -2,


        /// <summary>
        /// 审方通过待领取
        /// </summary>
        [Description("审方通过待领取")]
        Verified =3,


        /// <summary>
        /// 审方驳回
        /// </summary>
        [Description("审方驳回")]
        VerifyFailed = -3,

        /// <summary>
        /// 医生已领取
        /// </summary>
        [Description("医生已领取")]
        Received = 4,

        /// <summary>
        /// 医生驳回
        /// </summary>
        [Description("医生驳回")]
        DoctorCanceled = -4,

        /// <summary>
        /// 已提交签名
        /// </summary>
        [Description("已提交签名")]
        SubmitCA =5,

        /// <summary>
        /// 提交签名失败
        /// </summary>
        [Description("提交签名失败")]
        SubmitCAFailed = -5,

        /// <summary>
        /// 医生已签名
        /// </summary>
        [Description("医生已签名")]
        Signed = 6,
    }

}
