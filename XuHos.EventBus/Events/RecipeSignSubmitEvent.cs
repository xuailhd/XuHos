using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 处方提交签名事件
    
    /// 日期：2017年4月28日
    /// 
    /// 前置条件：就诊结束
    /// </summary>
    public class RecipeSignSubmitEvent: BaseEvent, IEvent
    {
        public string ServiceID { get; set; }
    }
}
