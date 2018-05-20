using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class UserLogoutedEvent: BaseEvent, IEvent
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public XuHos.Common.Enum.EnumUserType UserType { get; set; }

        /// <summary>
        /// 注销时间
        /// </summary>
        public DateTime LogoutTime { get; set; }


        public string ClientName { get; set; }

        public string ClientSourceType { get; set; }
    }
}
