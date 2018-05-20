using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class UserFollowEvent: BaseEvent, IEvent
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 关不医生编号
        /// </summary>
        public string FollowDoctorID { get; set; }
    }
}
