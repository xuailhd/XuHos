using System.ComponentModel;

namespace XuHos.Common.Enum
{
    [Description("用户类型")]
    public enum EnumUserType
    {
        /// <summary>
        /// 默认用户   //不限制权限时的默认值
        /// </summary>
        [Description("默认用户")]
        Default = 0,

        /// <summary>
        /// 普通用户
        /// </summary>
        [Description("普通用户")]
        User = 1,

        /// <summary>
        /// 医生用户
        /// </summary>
        [Description("医生用户")]
        Doctor = 2,

        /// <summary>
        /// 医院用户
        /// </summary>
        [Description("医院用户")]
        Hospital = 3,

        /// <summary>
        /// 药店用户
        /// </summary>
        [Description("药店用户")]
        Drugstore = 4,

        /// <summary>
        /// 药剂师
        /// </summary>
        [Description("药剂师")]
        Apothecary = 5,

        /// <summary>
        /// 护士
        /// </summary>
        [Description("护士")]
        Nurse = 6,

        /// <summary>
        /// 平台机器人
        /// </summary>
        [Description("平台机器人")]
        SysRobot = 7,
        /// <summary>
        /// 平台管理员
        /// </summary>
        [Description("平台管理员")]
        SysAdmin = 99,
    }
}