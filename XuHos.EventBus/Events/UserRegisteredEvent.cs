using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 用户注册
    /// </summary>
    public class UserRegisteredEvent : BaseEvent, IEvent
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount { get; set; }

        public string Mobile { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public EnumUserType UserType { get; set; }

        /// <summary>
        /// 机构编号
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 是否需要短信通知
        /// </summary>
        public bool NeedSendSMS { get; set; }
    }
}
