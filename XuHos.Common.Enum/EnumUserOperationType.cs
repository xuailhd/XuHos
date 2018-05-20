using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 用户操作类型
    /// </summary>
    [Description("用户操作类型")]
    public enum EnumUserOperationType
    {
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        Login = 0,

        /// <summary>
        /// 注销
        /// </summary>
        [Description("注销")]
        Logout = 1,

        /// <summary>
        /// 进入图文
        /// </summary>
        [Description("进入图文")]
        EnterPicService = 2,

        /// <summary>
        /// 退出图文
        /// </summary>
        [Description("退出图文")]
        LeavePicService = 3,

        /// <summary>
        /// 进入语音
        /// </summary>
        [Description("进入语音")]
        EnterAudService = 4,

        /// <summary>
        /// 退出语音
        /// </summary>
        [Description("退出语音")]
        LeaveAudService = 5,

        /// <summary>
        /// 进入视频
        /// </summary>
        [Description("进入视频")]
        EnterVidService = 6,

        /// <summary>
        /// 退出视频
        /// </summary>
        [Description("退出视频")]
        LeaveVidService = 7,
    }
}