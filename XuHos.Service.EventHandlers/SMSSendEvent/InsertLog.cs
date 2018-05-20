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
using XuHos.Entity;
using XuHos.Common.Cache.Keys;
using XuHos.Common.Cache;

namespace XuHos.Service.EventHandlers.SMSSendEvent
{
    /// <summary>
    /// 写入日志
    /// </summary>
    //public class InsertLog : IEventHandler<EventBus.Events.SMSSendEvent>
    //{
    //    public bool Handle(EventBus.Events.SMSSendEvent evt)
    //    {
    //        try
    //        {
    //            evt.OutTime = evt.OutTime ?? DateTime.Now.AddMinutes(5);
    //            BLL.Sys.Implements.SysShortMessageService serviceMsgLog = new BLL.Sys.Implements.SysShortMessageService();
    //            UserShortMessageLog model = new UserShortMessageLog()
    //            {
    //                ShortMessageLogID = Guid.NewGuid().ToString("N"),
    //                MsgLogType = evt.MsgType,
    //                UserID = evt.UserID ?? "",
    //                TelePhoneNum = evt.Mobile,
    //                MsgTitle = evt.Title ?? "",
    //                MsgContent = evt.Content ?? "",
    //                OutTime = evt.OutTime.Value
    //            };

    //            StringCacheKey cacheKey = new StringCacheKey(StringCacheKeyType.SYS_SMSVerifyCode, $"{evt.Mobile}/{evt.MsgType}/{evt.Title}");

    //            model.ToCache(cacheKey, evt.OutTime.Value);

    //            if (!serviceMsgLog.InsertLog(model))
    //            {
    //                LogHelper.WriteWarn("写入短信发送日志失败");
    //            }

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            XuHos.Common.LogHelper.WriteError(ex);
    //        }

    //        return false;
    //    }
    //}
}