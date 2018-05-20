using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XuHos.EventBus
{  /// <summary>
   /// 事件实体基类
   /// </summary>
    public interface IEvent
    {
        string EventId
        { get; set; }
    }

    public abstract class BaseEvent : IEvent
    {
        public BaseEvent()
        {
            this.EventId = Guid.NewGuid().ToString("N");
        }

        public string EventId
        { get; set; }
    }
}
