using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    /// <summary>
    /// 房间状态
    /// </summary>
    public class RequestConversationRoomTriageDTO
    {
        [Required]
        /// <summary>
        /// 频道编号
        /// </summary>
        public int ChannelID { get; set; }

    }
}
