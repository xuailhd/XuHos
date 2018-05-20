using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    [Description("用户性别")]

    public enum EnumUserGender
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Male =0,
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Female =1,
        ///// <summary>
        ///// 其他
        ///// </summary>
        [Description("未知")]
        Other = 2
    }
}
