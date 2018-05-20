
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 家庭医生包使用人群范围
    /// </summary>
    [Description("家庭医生包使用人群范围")]
    public enum EnumFamilyUserRange
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        Default = 0,

        /// <summary>
        /// 一般人群服务包
        /// </summary>
        [Description("一般人群服务包")]
        Normal = 1,

        /// <summary>
        /// 老年人服务包
        /// </summary>
        [Description("老年人服务包")]
        OldPeople = 2,

        /// <summary>
        /// 0-6岁服务包
        /// </summary>
        [Description("0-6岁服务包")]
        AtAgeOf6 = 3,

        /// <summary>
        /// 孕产妇服务包
        /// </summary>
        [Description("孕产妇服务包")]
        Maternal = 4

    }


}
