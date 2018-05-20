using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common.Request
{
    public class RequestMySentUserNoticeConditionDTO : RequestSearchCondition
    {

        public EnumUserNoticeType? UserNoticeType { get; set; }

        public EnumNoticeFirstType? NoticeFirstType { get; set; }

        public EnumNoticeSecondType? NoticeSecondType { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string UserID { get; set; }

    }
}
