using System.ComponentModel;

namespace XuHos.Common.Enum
{

    /// <summary>
    /// 用户证件类型(0-身份证,1-居民户口本,2-护照,3-军官证,4-驾驶证,5-港澳通行证,6-台湾通行证,7-港澳台身份证,99-其它)
    /// </summary>
    [Description("用户证件类型")]
    public enum EnumUserCardType
    {
        /// <summary>
        /// 身份证
        /// </summary>
        [Description("身份证")]
        IDCard = 0,

        /// <summary>
        /// 居民户口本
        /// </summary>
        [Description("居民户口本")]
        ResidentsBooklet = 1,

        /// <summary>
        /// 护照
        /// </summary>
        [Description("护照")]
        Passport = 2,

        /// <summary>
        /// 军官证
        /// </summary>
        [Description("军官证")]
        MilitaryOfficer = 3,

        /// <summary>
        /// 驾驶证
        /// </summary>
        [Description("驾驶证")]
        DriverLicense = 4,

        /// <summary>
        /// 港澳通行证
        /// </summary>
        [Description("港澳通行证")]
        HKMacaoPass = 5,

        /// <summary>
        /// 台湾通行证
        /// </summary>
        [Description("台湾通行证")]
        TaiwanPass = 6,

        /// <summary>
        /// 港澳台身份证
        /// </summary>
        [Description("港澳台身份证")]
        HMTIDCard = 7,

        /// <summary>
        /// 其它
        /// </summary>
        [Description("其它")]
        Other = 99


    }
}
