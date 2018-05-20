using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;

namespace XuHos.Service.EventHandlers.OrderCompletedEvent
{
    /// <summary>
    /// 默认处理
    /// </summary>
    public class DefaultHandler : IEventHandler<EventBus.Events.OrderCompletedEvent>
    {
        public bool Handle(EventBus.Events.OrderCompletedEvent evt)
        {
            //清医生 统计缓存
            XuHos.BLL.Doctor.Implements.DoctorService dse = new BLL.Doctor.Implements.DoctorService();
            dse.CleanDoctorCacheAndSendNoticeToDoctor(evt.OrderNo);
            return true;
        }
    }
}
