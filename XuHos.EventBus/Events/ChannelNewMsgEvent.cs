using XuHos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 频道新消息事件
    
    /// 日期:2017年4月12日
    /// </summary>
    public class ChannelNewMsgEvent : BaseEvent, IEvent
    {

        public int ChannelID { get; set; }

        public XuHos.Common.Enum.EnumDoctorServiceType ServiceType { get; set; }

        public string ServiceID { get; set; }

        public string FromAccount { get; set;}

        public ConversationMessage[] Messages { get; set; }

        public string OptPlatform { get; set; }
    }
}
