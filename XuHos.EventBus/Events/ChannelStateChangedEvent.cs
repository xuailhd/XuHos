using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class ChannelStateChangedEvent : BaseEvent, IEvent
    {
        public int ChannelID { get; set; }

        public string FromUserID { get; set; }

        public string FromPlatform { get; set; }

        public EnumRoomState State { get; set; }

        /// <summary>
        /// 期望状态
        /// </summary>
        public EnumRoomState? ExpectedState { get; set; }

        public bool DisableWebSdkInteroperability { get; set; }

        public bool EnterRoomSendNotice { get; set; } = true;
    }
}
