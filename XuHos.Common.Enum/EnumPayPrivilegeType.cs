using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 折扣类型(不使用特权=0,义诊=2,套餐=3,家庭医生=5,机构折扣=6)
    /// </summary>
    [Description("消费类型")]
    public enum EnumPayPrivilege
    {
        /// <summary>
        /// 不使用特权
        /// </summary>
        [Description("不使用特权")]
        None = 0,
        /// <summary>
        /// 义诊
        /// </summary>
        [Description("义诊")]
        FreeClinic = 2,
        /// <summary>
        /// 套餐
        /// </summary>
        [Description("套餐")]
        MemberPackage = 3,
        /// <summary>
        /// 家庭医生
        /// </summary>
        [Description("家庭医生")]
        FamilyDoctor = 5,
        /// <summary>
        /// 机构折扣
        /// </summary>
        [Description("机构折扣")]
        OrgDiscount = 6,

        /// <summary>
        /// 家庭医生服务平台
        /// </summary>
        [Description("家庭医生服务平台")]
        FDPlatform = 7,
    }
}
