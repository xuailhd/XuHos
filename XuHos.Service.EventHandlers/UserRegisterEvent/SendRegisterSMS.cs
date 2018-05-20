using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.EventBus.Events;
using XuHos.BLL.Sys.Implements;
using XuHos.BLL.Sys;
using XuHos.Common.Config.Sections;
using XuHos.Entity;

namespace XuHos.Service.EventHandlers.UserRegisterEvent
{
    /// <summary>
    /// 默认处理
    /// </summary>
    public class SendRegisterSMS : IEventHandler<EventBus.Events.UserRegisteredEvent>
    {
        public bool Handle(EventBus.Events.UserRegisteredEvent evt)
        {
            return true;
            //if (evt == null)
            //    return true;
            //try
            //{
            //    #region 发送密码 短信
            //    //用户可以登录时才发送短信
            //    if (evt.NeedSendSMS && Common.ToolHelper.CheckPhoneNumber(evt.Mobile))
            //    {
            //        //var config = SysConfigService.Get<SMS>(true, "[" + evt.OrgCode + "]");

            //        //if (config == null || string.IsNullOrEmpty(config.MsgAccountPasswordTemlateId))
            //        //{
            //        //    //没取得，取默认 不带机构编码的
            //        //    config = SysConfigService.Get<SMS>(true);

            //        //    if (config == null || string.IsNullOrEmpty(config.MsgAccountPasswordTemlateId))
            //        //    {
            //        //        XuHos.Common.LogHelper.WriteWarn("发送注册短信：错误的短信模板");
            //        //        return false;
            //        //    }
            //        //}
            //        var msgtype = 5;

            //        var shortMessageService = new SysShortMessageService();
            //        var template = shortMessageService.GetTemplate(msgtype, evt.OrgCode);

            //        if(template == null)
            //        {
            //            XuHos.Common.LogHelper.WriteError(new Exception("发送注册短信失败:没有找到模板"));
            //            return true;
            //        }

            //        string content = string.Format(template.TemplateContent, evt.Mobile, evt.UserPassword);
            //        var msgParms = new List<string>();
            //        msgParms.Add(evt.Mobile);
            //        msgParms.Add(evt.UserPassword);

            //        using (var mqChannel = new EventBus.MQChannel())
            //        {
            //            return mqChannel.Publish(new EventBus.Events.SMSSendEvent()
            //            {
            //                TemplateID = template.TemplateID,
            //                MsgType = msgtype,
            //                Content = content,
            //                MsgParms = msgParms,
            //                Title = evt.UserPassword,
            //                Mobile = evt.Mobile,
            //                UserID = evt.UserID,
            //            });
            //        }
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    Common.LogHelper.WriteError(ex);
            //}

            //return false;
        }
    }
}
