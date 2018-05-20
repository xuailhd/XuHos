using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestGetConversationMessageDTO:IPagerRequest,IRequest
    {
        /// <summary>
        /// 通道编号/房间编号
        /// </summary>
        public int ChannelID
        { get; set; }

        public int CurrentPage
        {
            get; set;
        }

        public int PageSize
        {
            get; set;
        }
    }
}
