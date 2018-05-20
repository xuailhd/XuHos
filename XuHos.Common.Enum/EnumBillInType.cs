using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 收费单类型（1=处方）
    /// </summary>
    [Description("收费单类型")]    
    public enum EnumBillInType
    {
        /// <summary>
        /// 处方
        /// </summary>
        [Description("处方")]
        Recipe =1,
    }
}
