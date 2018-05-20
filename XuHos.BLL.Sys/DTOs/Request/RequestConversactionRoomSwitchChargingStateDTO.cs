using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestConversactionRoomSwitchChargingStateDTO
    {
        [Required]
        /// <summary>
        /// 预约登记ID
        /// </summary>
        public int ChannelID { get; set; }

        [Required]
        public bool Switch { get; set; }
    }
}
