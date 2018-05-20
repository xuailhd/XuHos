using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 处方签名完成回调。DB未更新State
    /// </summary>
    public class RecipeSignCallbackEvent : BaseEvent, IEvent
    {
        public string RecipeFileId { get; set; }
        public string SignedStamp { get; set; }
        
    }
}