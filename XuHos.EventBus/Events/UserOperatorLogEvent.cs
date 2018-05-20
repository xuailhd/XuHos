using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 用户操作日志
    /// </summary>
    public class UserOperatorLogEvent : BaseEvent, IEvent
    {

        /// <summary>
        /// 操作机构编号
        /// </summary>
        public string OrgID { get; set; }

        /// <summary>
        /// 操作用户编号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 操作用户类型
        /// </summary>
        public XuHos.Common.Enum.EnumUserType UserType { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 操作说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperatorTime { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public EnumUserOperationType OperatorType { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string OperatorName { get; set; }


        public object OperationData { get; set; }

    }
}
