using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    [Description("短信模块类型")]
    public enum EnumeMsgType
    {
        /// <summary>
        /// 用户注册
        /// </summary>
        [Description("用户注册")]
        UserRegister = 1,

        /// <summary>
        /// 找回密码
        /// </summary>
        [Description("找回密码")]
        FindPwd = 2,
    }
}
