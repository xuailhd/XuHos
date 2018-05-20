using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.EventHandlers.ChanneCreateEvent
{
    /// <summary>
    /// 创建频道
    
    /// 日期：2017年5月5日
    /// 
    /// 触发条件：订单支付成功
    /// 前置条件：订单已支付
    /// 后置条件：调用云通信接口创建频道、更新频道启用状态
    /// </summary>
    public class DefaultHandler : IEventHandler<EventBus.Events.ChannelCreateEvent>
    {
        public bool Handle(EventBus.Events.ChannelCreateEvent evt)
        {
            try
            {
                if (evt == null)
                    return true;

                if (evt.ChannelID <= 0)
                    return true;

                string UserID = "";

              
                if (evt.ServiceType == EnumDoctorServiceType.PicServiceType || evt.ServiceType == EnumDoctorServiceType.VidServiceType || evt.ServiceType == EnumDoctorServiceType.AudServiceType)
                {
                    return new BLL.UserOPDRegisterService(UserID).CreateIMRoom(evt.ServiceID);
                }
                else
                {
                    return true;
                }
            }
            catch (Exception E)
            {
                LogHelper.WriteError(E);
            }

            return false;
        }
    }


}
