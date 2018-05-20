using System.ComponentModel;

namespace XuHos.Common.Enum
{
    [Description("资金流向类型")]
    public enum EnumFlowsType
    {
        /// <summary>
        /// 流入
        /// </summary>
        [Description("收入")]
        Inflow = 1,
        /// <summary>
        /// 流出
        /// </summary>
        [Description("支出")]
        Outflow = 2
    }
}
