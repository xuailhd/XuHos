using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Common.Request
{
    public class RequestNoticeMessageInsertDTO
    {
        public RequestNoticeMessageDTO Model { get; set; }

        public List<string> ToUserList { get; set; }

        public EnumTargetUserCodeType ToUserListType { get; set; }
    }
}
