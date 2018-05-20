using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class UserOnlineStateChangedEvent: BaseEvent, IEvent
    {
        public string UserID { get; set; }

        public string Action { get; set; }
    }
}
