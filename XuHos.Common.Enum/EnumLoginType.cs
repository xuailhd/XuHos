using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 0-Web登录,1-桌面版登录,2-Android登录,3-IOS登录
    /// </summary>
    public enum EnumLoginType
    {
        /// <summary>
        /// Web登录
        /// </summary>
        [Description("Web")]
        Web =0,
        /// <summary>
        /// 桌面版登录
        /// </summary>
        [Description("CS")]
        Cs,
        /// <summary>
        /// Android登录
        /// </summary>
        [Description("Android")]
        Android,
        /// <summary>
        /// IOS登录
        /// </summary>
        [Description("IOS")]
        IOS
    }
}
