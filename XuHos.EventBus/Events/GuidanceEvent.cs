using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 导诊平台事件
    /// </summary>
    public class GuidanceEvent : BaseEvent, IEvent
    {
        public string OPDRegisterID { get; set; }

        public int TriageStatusInt { get; set; }

        public int EventTypeInt { get; set; }

        public EnumTriageStatus TriageStatus { get { return (EnumTriageStatus)TriageStatusInt; } }

        public EnumGuidanceEventType EventType { get { return (EnumGuidanceEventType)EventTypeInt; } }

        public string CurrentOperatorUserID { get; set; }
    }
}
