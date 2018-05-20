using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common.Request
{
    public class RequestMyReviceUserNoticeConditionDTO : RequestSearchCondition
    {
        public string MessageID { get; set; }

        /// <summary>
        /// 0所有消息，1已读消息，2未读消息
        /// </summary>
        public int ReadStatus { get; set; }

        public EnumUserNoticeType? UserNoticeType { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string UserID { get; set; }

    }
}
