
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 房间计费状态 0=已停止，1=已启动，2=已暂停
    /// </summary>
    [Description("房间计费状态")]
    public enum EnumRoomChargingState
    {
        /// <summary>
        /// 已停止
        /// </summary>
        [Description("已停止")]
        Stoped = 0,
        /// <summary>
        /// 线下
        /// </summary>
        [Description("已启动")]
        Started = 1,

        /// <summary>
        /// 已暂停
        /// </summary>
        [Description("已暂停")]
        Paused = 2
    }

           
}
