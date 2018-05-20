using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    public class UserLoginedEvent : BaseEvent, IEvent
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        public XuHos.Common.Enum.EnumUserType UserType { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 客户端编号
        /// </summary>
        public string ClientID { get; set; }

        public string ClientSourceType { get; set; }

        /// <summary>
        /// 注销时间
        /// </summary>
        public DateTime LoginTime { get; set; }
    }
}

