using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 房间过期时间
    
    /// 日期：2017年4月20日
    /// </summary>
    public class ChannelExpireEvent : BaseEvent, IEvent
    {
        public string ServiceID { get; set; }
    }
}
