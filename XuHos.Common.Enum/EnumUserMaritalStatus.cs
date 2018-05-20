using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 用户婚姻状况 (0-未婚、1-已婚、2-未知)
    /// </summary>
    [Description("用户婚姻状况")]

    public enum EnumUserMaritalStatus
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("未婚")]
        Unmarried = 0,
        /// <summary>
        /// 女
        /// </summary>
        [Description("已婚")]        
        Married =1,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("未知")]
        Other =2
    }
}
