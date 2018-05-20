using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 频道计费
    /// 说明：时钟频率15秒
    
    /// 日期：2017年4月27日
    /// </summary>
    public class ChannelChargingEvent: BaseEvent, IEvent
    {
        /// <summary>
        /// 频道号
        /// </summary>
        public int ChannelID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 计费时间（作为逻辑时间，计费是通过逻辑时间+时钟频率 计算）
        /// </summary>
        public DateTime ChargingTime { get; set; }

        /// <summary>
        /// 时钟频率
        /// </summary>
        public int Interval { get; set; }
    }
}
