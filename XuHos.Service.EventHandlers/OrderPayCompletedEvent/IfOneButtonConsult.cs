using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.Entity;

namespace XuHos.Service.EventHandlers.OrderPayCompletedEvent
{
    /// <summary>
    /// 一键咨询，一键呼叫写入队列
    /// 等待医生领单
    /// </summary>
    public class IfOneButtonConsult : IEventHandler<EventBus.Events.OrderPayCompletedEvent>
    {
        XuHos.BLL.Sys.Implements.SysGrabService<string> grabOPDPriorityPoolService = new XuHos.BLL.Sys.Implements.SysGrabService<string>(nameof(UserOPDRegister));

        XuHos.BLL.Sys.Implements.ConversationRoomService roomService = new XuHos.BLL.Sys.Implements.ConversationRoomService();

        XuHos.BLL.UserOPDRegisterService opdService = new BLL.UserOPDRegisterService("");


        public bool Handle(EventBus.Events.OrderPayCompletedEvent evt)
        {
            if (evt == null)
                return true;

            try
            {
                var opd = opdService.Single(evt.OrderOutID);

                if (opd != null && string.IsNullOrEmpty(opd.DoctorID))
                {
                    var room = roomService.GetChannelInfo(opd.OPDRegisterID);

                    if (room != null)
                    {

                        #region 进导诊系统分诊的订单不加入订单池
                        if (opd.DoctorTriage.IsToGuidance == true)
                        {
                            return true;
                        }
                        #endregion

                        if (evt.OrderType == Common.Enum.EnumProductType.video || evt.OrderType == Common.Enum.EnumProductType.Phone)
                        {
                            if (evt.PayPrivilege == Common.Enum.EnumPayPrivilege.FamilyDoctor)
                            {
                                grabOPDPriorityPoolService.DispatchTask(evt.OrderOutID, room.Priority, opd.DoctorGroupID);
                            }
                            else
                            {
                                grabOPDPriorityPoolService.DispatchTask(evt.OrderOutID, room.Priority, "ALL");
                            }

                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XuHos.Common.LogHelper.WriteError(ex);
                return false;
            }

            return true;
        }
    }
}
