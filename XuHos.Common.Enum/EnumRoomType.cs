
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 房间类型 1=群组，0=1对1
    /// </summary>
    [Description("房间类型")]
    public enum EnumRoomType
    {
        /// <summary>
        /// 线上
        /// </summary>
        [Description("群组")]
        Group = 1,
        /// <summary>
        /// 线下
        /// </summary>
        [Description("1对1")]
        C2C = 0,
       
    }

           
}
