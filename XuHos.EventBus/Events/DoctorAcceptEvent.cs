using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{

    public class DoctorAcceptEvent : BaseEvent, IEvent
    {
        /// <summary>
        /// 医生编号
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 医生用户编号
        /// </summary>
        public string DoctorUserID { get; set; }

        /// <summary>
        /// 房间编号
        /// </summary>
        public int ChannelID { get; set; }

        /// <summary>
        /// 业务编号
        /// </summary>
        public string ServiceID { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户成员编号
        /// </summary>
        public string UserMemberID { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public XuHos.Common.Enum.EnumDoctorServiceType ServiceType { get; set; }

    }
}
