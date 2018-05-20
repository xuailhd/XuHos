using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class ChannelTriageChangeEvent: BaseEvent, IEvent
    {
        /// <summary>
        /// 医生编号
        /// </summary>
        public string DoctorID { get; set; }
        
        /// <summary>
        /// 频道编号
        /// </summary>
        public int ChannelID { get; set; }

        /// <summary>
        /// 分诊编号
        /// </summary>
        public string TriageID { get; set; }

    }
}
