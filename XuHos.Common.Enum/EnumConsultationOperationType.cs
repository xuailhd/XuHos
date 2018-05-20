using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 会诊操作类型
    /// </summary>
    public enum EnumConsultationOperationType
    {
        /// <summary>
        /// 下单
        /// </summary>
        Create = 1,

        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 2,

        /// <summary>
        /// 确认定单，发起付款
        /// </summary>
        Confirm = 3,

        /// <summary>
        /// 完成付款
        /// </summary>
        Payed = 4,

        /// <summary>
        /// 会诊开始
        /// </summary>
        Start = 5,

        /// <summary>
        /// 填写会诊意见
        /// </summary>
        Comments = 6,

        /// <summary>
        /// 会诊结束
        /// </summary>
        Finished = 7,

        /// <summary>
        /// 取消会诊
        /// </summary>
        Cancel = 8,

        /// <summary>
        /// 退款
        /// </summary>
        Refund = 9,

        /// <summary>
        /// 编辑病历资料
        /// </summary>
        EditMedical = 10,

        /// <summary>
        /// 编辑文件
        /// </summary>
        EditFile = 11,

        /// <summary>
        /// 派单
        /// </summary>
        Dispatch = 12,

        /// <summary>
        /// 申请会诊专家
        /// </summary>
        Specialist = 13
    }
}
