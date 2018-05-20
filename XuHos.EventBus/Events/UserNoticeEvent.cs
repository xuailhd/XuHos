using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class UserNoticeEvent: BaseEvent, IEvent
    {
        public string FromUserID
        { get; set; }

        public EnumUserType FromUserType { get; set; }

        public EnumNoticeSecondType NoticeType { get; set; }

        public EnumDoctorServiceType ServiceType { get; set; }

        public string ServiceID { get; set; }
    }
}
