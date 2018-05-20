using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// Banner终端类型
    /// </summary>
    [Description("Banner终端类型")]
    public enum EnumBannerTerminal
    {

        /// <summary>
        /// WEB端
        /// </summary>
        [Description("WEB端")]
        WEB = 0,

        /// <summary>
        /// 医生APP端
        /// </summary>
        [Description("医生APP端")]
        DoctorAPP = 1,

        /// <summary>
        /// 患者APP端
        /// </summary>
        [Description("患者APP端")]
        UserAPP = 2


        //自定义添加banner类型，如：可以添加具体医院的banner，结合OutID使用

    }
}
