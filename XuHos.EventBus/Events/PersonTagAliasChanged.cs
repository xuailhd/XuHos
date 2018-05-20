using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class PersonTagAliasChanged: BaseEvent, IEvent
    {
        public XuHos.Common.Enum.EnumUserCardType IDType { get; set; }

        public string IDNumber { get; set; }

        /// <summary>
        /// 新增的别名
        /// </summary>
        public List<string> TagAliases { get; set; }
        
    }
}
