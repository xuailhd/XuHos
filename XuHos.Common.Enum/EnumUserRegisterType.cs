using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 用户注册类型(1-互联网注册,2-后台注册,3-第三方系统导入)
    /// </summary>
    [Description("用户注册类型")]
    public enum EnumUserRegisterType
    {
        /// <summary>
        /// 互联网注册   
        /// </summary>
        [Description("互联网注册")]
        NetWork = 1,

        /// <summary>
        /// 后台注册   
        /// </summary>
        [Description("后台注册")]
        Admin = 2,

        /// <summary>
        /// 第三方系统导入  
        /// </summary>
        [Description("第三方系统导入")]
        ThirdParty = 3,
    }
}
