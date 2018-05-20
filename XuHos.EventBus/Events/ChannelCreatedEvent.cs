using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class ChannelCreatedEvent: BaseEvent, IEvent
    {
        public string ServiceID { get; set; }

        public EnumDoctorServiceType ServiceType { get; set; }

        public int ChannelID { get; set; }

    }
}
