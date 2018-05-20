using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.Extensions;
using XuHos.DTO.Common;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestCustomMsgRoomStateChanged:IRequestIMCustomMsg<RequestConversationRoomStatusDTO>
    {
        public RequestConversationRoomStatusDTO Data
        {
            get; set;
        }

        public string Desc
        {
            get; set;
        }

        public string Ext
        {
            get
            {
                return EnumIMCustomMsgType.Room_StateChanged.GetEnumDescript();
            }
        }
    }
}
