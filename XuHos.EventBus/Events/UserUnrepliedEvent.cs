using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 用户问诊未回复监测事件
    /// </summary>
    public class UserUnrepliedEvent : BaseEvent, IEvent
    {
        /// <summary>
        /// 服务ID
        /// </summary>
        public string ServiceID { set; get; }

        /// <summary>
        /// 频道ID
        /// </summary>
        public int ChannelID { set; get; }

        /// <summary>
        /// 时钟频率
        /// </summary>
        public int Interval { set; get; }
    }
}
