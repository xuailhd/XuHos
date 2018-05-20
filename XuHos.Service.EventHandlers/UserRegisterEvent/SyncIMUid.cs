using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL.Sys.Implements;

namespace XuHos.Service.EventHandlers.UserRegisterEvent
{
    /// <summary>
    /// 默认处理
    /// </summary>
    public class SyncIMUid : IEventHandler<EventBus.Events.UserRegisteredEvent>
    {
        public bool Handle(EventBus.Events.UserRegisteredEvent evt)
        {
            try
            {
                if (evt == null)
                    return true;

                XuHos.Integration.QQCloudy.IMHelper imService = new XuHos.Integration.QQCloudy.IMHelper();

                ConversationIMUidService imUidService = new ConversationIMUidService("");

                var identifier = imUidService.GetUserIMUid(evt.UserID);

                if (imService.CreateMember(identifier, "", ""))
                {
                    return imUidService.EnableUid(new int[] { identifier });
                }
             
            }
            catch (Exception E)
            {
                XuHos.Common.LogHelper.WriteError(E);
            }

            return false;
        }
    }
}
