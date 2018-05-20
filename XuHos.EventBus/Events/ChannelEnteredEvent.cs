using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class ChannelEnteredEvent : BaseEvent, IEvent
    {
        public int ChannelID { get; set; }

        public string UserID { get; set; }
    }
}
