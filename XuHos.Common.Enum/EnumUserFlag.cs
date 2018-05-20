using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 用户标识(0-普通用户、1-内部员工、2-集团领导、3-政府官员、4-经销商)
    /// </summary>
    [Description("用户标识")]
    public enum EnumUserFlag
    {

        /// <summary>
        /// 普通用户,默认值
        /// </summary>
        [Description("普通用户")]
        NormalUser = 0,

        /// <summary>
        /// 内部员工
        /// </summary>
        [Description("内部员工")]
        InternalStaff = 1,

        /// <summary>
        /// 集团领导
        /// </summary>
        [Description("集团领导")]
        GroupLeader = 2,

        /// <summary>
        /// 政府官员
        /// </summary>
        [Description("政府官员")]
        Official = 3,

        /// <summary>
        /// 经销商
        /// </summary>
        [Description("经销商")]
        Agency = 4
    }
}
