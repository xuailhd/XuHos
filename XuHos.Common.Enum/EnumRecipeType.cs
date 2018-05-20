using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 处方类型 1=中药处方,2=西药处方
    /// </summary>
    [Description("处方类型")]
    public enum EnumRecipeType
    {
        /// <summary>
        /// 中药处方
        /// </summary>
        [Description("中药处方")]
        ChineseRecipe = 1,
        /// <summary>
        /// 西药处方
        /// </summary>
        [Description("西药处方")]
        WesternRecipe = 2
    }
}
