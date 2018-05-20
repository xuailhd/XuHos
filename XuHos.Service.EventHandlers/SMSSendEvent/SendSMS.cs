using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL;
using XuHos.BLL.Sys.DTOs.Request;
using XuHos.Common.Enum;
using XuHos.BLL.Sys.Implements;
using XuHos.Common;
using XuHos.Common.Cache;
using XuHos.Extensions;
using XuHos.Entity;
using XuHos.Common.Cache.Keys;

namespace XuHos.Service.EventHandlers.SMSSendEvent
{
    /// <summary>
    /// 发送短信
    /// </summary>
    public class SendSMS : IEventHandler<EventBus.Events.SMSSendEvent>
    {
        public bool Handle(EventBus.Events.SMSSendEvent evt)
        {
            var service = new SysShortMessageService();
            var sendSmsdto = evt.Map<EventBus.Events.SMSSendEvent, RequestSendSMSDTO>();
            var result = service.SendMsg(sendSmsdto);

            //短信发送失败不重试。
            if(result.Status == EnumApiStatus.BizError)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}