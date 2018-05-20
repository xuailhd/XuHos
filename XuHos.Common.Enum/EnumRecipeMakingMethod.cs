using System.ComponentModel;

namespace XuHos.Common.Enum
{

    /// <summary>
    /// 制作方法 0=其他,1=膏肓，2=丸剂,3=散剂,4=饮片
    /// </summary>
    [Description("制作方法")]
    public enum EnumRecipeMakingMethod
    {
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other =0,
        /// <summary>
        /// 膏肓
        /// </summary>
        [Description("膏肓")]
        GaoHang = 1,
        /// <summary>
        /// 丸剂
        /// </summary>
        [Description("丸剂")]
        WanJi = 2,

        /// <summary>
        /// 散剂
        /// </summary>
        [Description("散剂")]
        SanJi = 2,

        /// <summary>
        /// 饮片
        /// </summary>
        [Description("饮片")]
        YingPian = 4,
    }
}
