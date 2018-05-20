using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 医生排班事件
    /// </summary>
    public class DoctorSchduleEvent : BaseEvent, IEvent
    {
        public EnumSchduleEventType EventType { get; set; }

        public string OldNumberSourceID { get; set; }

        public string NumberSourceID { get; set; }

        public bool IsTableChanged { get; set; }

        public bool IsDateChanged { get; set; }

        public bool IsStatusChanged { get; set; }

        /// <summary>
        /// 状态(0未启用，1启用)
        /// </summary>
        public int Status { get; set; }

        public List<string> AppointSchduleIds { get; set; }

        public string CurrentUserID { get; set; }

    }
}
