using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 医生在线状态
    /// </summary>
    public enum EnumOnlineStatus
    {

        /// <summary>
        /// 空闲
        /// </summary>
        [Description("空闲")]
        Free =0,

        /// <summary>
        /// 忙碌
        /// </summary>
        [Description("忙碌")]
        Busy =1
    }
}
