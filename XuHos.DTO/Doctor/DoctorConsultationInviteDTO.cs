using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.Common.Enum;
using XuHos.Extensions;
using XuHos.DTO.Common;
using XuHos.Entity;

namespace XuHos.DTO
{
    /// <summary>
    /// 我的会诊
    /// </summary>
    public class DoctorConsultationInviteDTO
    {
        /// <summary>
        /// 会诊邀请编号
        /// </summary>
        public string ConsultationInviteID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 会诊单ID
        /// </summary>
        public string ConsultationID { get; set; }

        /// <summary>
        /// 会诊医生意见
        /// </summary>
        public string Opinion { get; set; }

        /// <summary>
        /// 会诊医生金额
        /// </summary>
        public decimal Amount { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 是否是主诊医生
        /// </summary>
        public bool IsConsult { get; set; }

        /// <summary>
        /// 房间信息
        /// </summary>
        public ConversationRoomDTO Room { get; set; }

        /// <summary>
        /// 成员信息
        /// </summary>
        public UserMemberDTO Member { get; set; }

        /// <summary>
        /// 会诊
        /// </summary>
        public DoctorConsultationDTO Consultation { get; set; }

        /// <summary>
        /// 医生
        /// </summary>
        public DoctorDto Doctor { get; set; }

        /// <summary>
        /// 主治医生
        /// </summary>
        public DoctorDto ConsDoctor { get; set; }


        public UserDTO User { get; set; }
    }

 


}
