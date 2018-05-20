using System.ComponentModel;

namespace XuHos.Common.Enum
{
    [Description("会诊单来源")]
    public enum EnumConsultationSource
    {

        /// <summary>
        /// 网络医院网站
        /// </summary>
        [Description("网络医院网站")]
        KMEHospWeb = 0,

        /// <summary>
        /// 导诊平台
        /// </summary>
        [Description("导诊平台")]
        Guidance = 1,

        /// <summary>
        /// 网络医院C/S端
        /// </summary>
        [Description("网络医院C/S端")]
        KMEHospCS = 2,

    }
}
