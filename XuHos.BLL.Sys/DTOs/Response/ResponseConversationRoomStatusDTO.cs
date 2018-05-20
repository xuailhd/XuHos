using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    /// <summary>
    /// 房间状态
    /// </summary>
    public class ResponseConversationRoomStatusDTO
    {
        /// <summary>
        /// 预约登记ID
        /// </summary>
        public int ChannelID { get; set; }

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
    }
}
