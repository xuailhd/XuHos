using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 医生分诊完成
    /// </summary>
    public class DoctorGuidancedEvent : BaseEvent, IEvent
    {

        /// <summary>
        /// 预约ID
        /// </summary>
        public string OPDRegisterID { get; set; }

        /// <summary>
        /// 分诊医生
        /// </summary>
        public string TriageDoctorID { get; set; }

        /// <summary>
        /// 预约医生
        /// </summary>
        public string DoctorID { get; set; }

        public string CurrentOperatorUserID { get; set; }

    }
}
