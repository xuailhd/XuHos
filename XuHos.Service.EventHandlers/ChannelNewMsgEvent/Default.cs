using XuHos.BLL;
using XuHos.BLL.Sys;
using XuHos.BLL.Sys.Implements;
using XuHos.Common;
using XuHos.Common.Enum;
using XuHos.Entity;
using XuHos.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Service.EventHandlers.ChannelNewMsgEvent
{
    /// <summary>
    /// 房间收到新消息
    
    /// 日期：2017年4月27日
    /// 
    /// 触发条件：云通信回调
    /// 前置条件：无
    /// 后置条件：消息记录写入数据库
    /// </summary>
    public class DefaultHandler : IEventHandler<EventBus.Events.ChannelNewMsgEvent>
    {
        public bool Handle(EventBus.Events.ChannelNewMsgEvent evt)
        {
            try
            {
                if (evt == null)
                    return true;

                ConversationMessageService bll = new ConversationMessageService(null);

                if(evt.Messages!=null && evt.Messages.Length>0)
                {
                    List<ConversationMessage> msgs = new List<ConversationMessage>();
                    for (int i= 0; i < evt.Messages.Length; i ++)
                    {
                        if (bll.Single<ConversationMessage>(evt.Messages[i].ConversationMessageID) == null)
                        {
                            msgs.Add(evt.Messages[i]);
                        }
                    }

                    if (msgs.Count > 0)
                    {
                        if (bll.Insert(msgs.ToArray()))
                        {
                            return true;

                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception E)
            {
                LogHelper.WriteError(E);
            }

            return false;
        }
    }


}
