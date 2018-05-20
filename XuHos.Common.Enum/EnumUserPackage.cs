
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 套餐类型（0-会员套餐、1-家庭医生、2-普通套餐、3-机构折扣）
    /// </summary>
    [Description("套餐类型")]
    public enum EnumPackageForType
    {
        /// <summary>
        /// 会员套餐
        /// </summary>
        [Description("会员套餐")]
        User = 0,

        /// <summary>
        /// 家庭医生
        /// </summary>
        [Description("家庭医生")]
        FamilyDoctor = 1,

        /// <summary>
        /// 普通套餐
        /// </summary>
        [Description("普通套餐")]
        Normal = 2,

        /// <summary>
        /// 机构折扣
        /// </summary>
        [Description("机构折扣")]
        OrgDiscount = 3,
    }


    /// <summary>
    /// 套餐归属（0-平台套餐，1-机构套餐，2-医生套餐）
    /// </summary>
    [Description("套餐归属")]
    public enum EnumPackageType
    {
        /// <summary>
        /// 平台套餐
        /// </summary>
        [Description("平台套餐")]
        Platform = 0,

        /// <summary>
        /// 机构套餐
        /// </summary>
        [Description("机构套餐")]
        Orgnazition = 1,

        /// <summary>
        /// 医生套餐
        /// </summary>
        [Description("医生套餐")]
        Doctor = 2,
    }
}
