using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 处方删除
    /// </summary>
    public class RecipeDelEvent : BaseEvent, IEvent
    {
        /// <summary>
        /// 处方唯一标识
        /// </summary>
        public string uniqueId { get; set; }

        /// <summary>
        /// 医师唯一标识
        /// </summary>
        public string doctorId { get; set; }
    }
}
