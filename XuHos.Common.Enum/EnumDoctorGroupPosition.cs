using System.ComponentModel;

namespace XuHos.Common.Enum
{

    /// <summary>
    /// 家庭医生组员职位  1成员；2队长
    /// </summary>
    [Description("家庭医生组员职位")]
    public enum EnumDoctorGroupPosition
    {
        /// <summary>
        /// 成员
        /// </summary>
        [Description("成员")]
        Member = 1,

        /// <summary>
        /// 组长
        /// </summary>
        [Description("组长")]
        GroupLeader = 2


    }
}
