using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{

    /// <summary>
    /// 房间状态
    /// </summary>
    public class RequestConversationRoomRenewUpgradeDTO
    {
        /// <summary>
        /// 预约登记ID
        /// </summary>
        public int ChannelID { get; set; }

        /// <summary>
        /// 房间状态
        /// </summary>
        public XuHos.Common.Enum.EnumRoomState State { get; set; }


        public string ServiceID { get; set; }

        public XuHos.Common.Enum.EnumDoctorServiceType ServiceType { get; set; }

        /// <summary>
        /// 续费订单编号
        /// </summary>
        public string RenewOrderNo { get; set; }

    }
}
