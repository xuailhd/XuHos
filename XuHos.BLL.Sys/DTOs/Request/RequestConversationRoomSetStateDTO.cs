using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    /// <summary>
    /// 设置房间状态
    /// </summary>
    public class RequestConversationRoomSetStateDTO
    {
        public RequestConversationRoomSetStateDTO()
        {
            this.DisableWebSdkInteroperability = false;
        }

        [Required]
        /// <summary>
        /// 预约登记ID
        /// </summary>
        public int ChannelID { get; set; }

        [Required]
        /// <summary>
        /// 房间状态
        /// </summary>
        public XuHos.Common.Enum.EnumRoomState State { get; set; }

        /// <summary>
        /// 房间状态
        /// </summary>
        public XuHos.Common.Enum.EnumRoomState? ExpectedState { get; set; }


        /// <summary>
        /// 默认不禁用
        /// </summary>
        public bool DisableWebSdkInteroperability { get; set; }

    }
}
