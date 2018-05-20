using XuHos.Common.Enum;
using XuHos.Integration.QQCloudy;
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
    public class RequestConversationRoomStatusDTO
    {
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
        /// 服务时长（单位：秒）
        /// </summary>
        public int Duration { get; set; }

        public string ServiceID { get; set; }

        public XuHos.Common.Enum.EnumDoctorServiceType ServiceType { get; set; }

        public int TotalTime { get; set; }

        public EnumRoomChargingState ChargingState { get; set; }

        /// <summary>
        /// 禁用互通性（意味着采用P2p通信）
        /// </summary>
        public bool DisableWebSdkInteroperability { get; set; }
    }

   
}
