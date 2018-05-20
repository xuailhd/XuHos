using System.ComponentModel;

namespace XuHos.Common.Enum
{

    /// <summary>
    /// 药品类型(1-中药,2-西药,3-中成药)
    /// </summary>
    [Description("药品类型")]
    public enum EnumDrugType
    {
        /// <summary>
        /// 中药
        /// </summary>
        [Description("中药")]
        Chinese = 1,
        /// <summary>
        /// 西药
        /// </summary>
        [Description("西药")]    
        Western = 2,
        /// <summary>
        /// 中成药
        /// </summary>
        [Description("中成药")]
        ChinesePatentDrug = 3,

        /// <summary>
        /// 辅料
        /// </summary>
        [Description("辅料")]
        Accessories = 9999,
    }
}
