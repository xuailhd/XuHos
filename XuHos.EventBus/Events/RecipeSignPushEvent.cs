using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 处方推送
    
    /// 日期：2017年4月28日
    /// </summary>
    public class RecipeSignPushEvent: BaseEvent, IEvent
    {
        public string RecipeFileID { get; set; }
    }
}
