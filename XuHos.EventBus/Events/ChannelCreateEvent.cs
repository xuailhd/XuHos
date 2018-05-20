using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// IM通信渠道创建事件
    /// </summary>
    public class ChannelCreateEvent : BaseEvent, IEvent
    {
        public string ServiceID { get; set; }

        public EnumDoctorServiceType ServiceType{get;set;}

        public int ChannelID { get; set; }
    }
}
