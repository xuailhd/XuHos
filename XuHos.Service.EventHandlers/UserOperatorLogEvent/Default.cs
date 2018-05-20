using XuHos.BLL.Common;
using XuHos.BLL.Common.DTOs;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.EventHandlers.UserOperatorLogEvent
{
    /// <summary>
    /// 写入用户日志
    /// </summary>
    public class DefaultHandler : IEventHandler<EventBus.Events.UserOperatorLogEvent>
    {
        XuHos.BLL.User.Implements.UserService service = new BLL.User.Implements.UserService();

        public bool Handle(EventBus.Events.UserOperatorLogEvent evt)
        {
            if (evt == null)
                return true;

            try
            {
                service.AppendOperateLog(new RequestUserOperateLogDTO<object>()
                {
                    UserID = evt.UserID,
                    OpTime = DateTime.Now,
                    UserType = evt.UserType,
                    OperationType = evt.OperatorType,
                    Remark = evt.Remark,
                    OperationData = evt.OperationData
                });
            }
            catch (Exception E)
            {
                LogHelper.WriteError(E);
                return false;
            }

            return true;
        }
    }
}