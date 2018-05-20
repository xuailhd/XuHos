using System.ComponentModel;

namespace XuHos.Common.Enum
{

    /// <summary>
    /// 1：已提交；2：操作人员审核；3：完成；4：已退药
    /// </summary>
    [Description("审核类型")]
    public enum EnumAuditState
    {
        /// <summary>
        /// 已提交
        /// </summary>
        [Description("已提交")]    
        Submit = 1,
        /// <summary>
        /// 操作人员审核
        /// </summary>
        [Description("操作人员审核")]
        OperatorAudit = 2,

        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        Done = 3,
        /// <summary>
        /// 已退药
        /// </summary>
        [Description("已退药")]
        DrugWithDrawal = 4,
    }
}
