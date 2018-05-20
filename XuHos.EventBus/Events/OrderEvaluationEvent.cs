using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 订单评价事件
    /// </summary>
    public class OrderEvaluationEvent : BaseEvent, IEvent
    {
        /// <summary>
        /// 外部订单ID
        /// </summary>		
        public string OuterID { get; set; }

        /// <summary>
        /// 评价分值
        /// </summary>		
        public int Score { get; set; }

        /// <summary>
        /// 评价标签，多个标签以(;)分割
        /// </summary>		
        public string EvaluationTags { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>		
        public string Content { get; set; }

        public string CreateUserID { get; set; }
    }
}
