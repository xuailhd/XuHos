using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class ChannelSendGroupMsgEvent<TMsg>: BaseEvent, IEvent
    {
        public int ChannelID { get; set; }

        public int FromAccount { get; set; }

        public TMsg Msg { get; set; }
    }
}
