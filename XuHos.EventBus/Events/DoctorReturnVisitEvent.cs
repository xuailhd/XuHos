using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 医生回访事件
    
    /// 日期：2017年8月25日
    /// </summary>
    public  class DoctorReturnVisitEvent: BaseEvent, IEvent
    {
        public string ReturnVisitContent { get; set; }
        public string ReturnVisitID { get; set; }
        public string ServiceID { get; set; }
    }
}
