using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class DoctorInviteInfoDTO
    {   
        
        /// <summary>
        /// 发送邀请人的用户编号
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 邀请人姓名
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// 被邀请人用户编号
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 被邀请人姓名
        /// </summary>
        public string ToName { get; set; }
        
        /// <summary>
        /// 房间编号
        /// </summary>
        public int ChannelID { get; set; }
    }

    /// <summary>
    /// 医生邀请信息
    /// </summary>
    public class DoctorInviteDTO:DoctorDto
    {
        public bool InviteState { get; set; }

        public decimal ConsulServicePrice { get; set; }
    }
}
