using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class ConsultationOperationEvent : BaseEvent, IEvent
    {

        public string ConsultationID { get; set; }

        public string CurrentOperatorUserID { get; set; }

        public string TradeNo { get; set; }
    }
}
