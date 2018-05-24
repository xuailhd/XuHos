using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common.Enum;

namespace XuHos.DTO
{

    /// <summary>
    /// 预约提交
    /// </summary>
    public class RequestUserOPDRegisterSubmitDTO
    {
        public RequestUserOPDRegisterSubmitDTO()
        {
            OPDType = EnumDoctorServiceType.VidServiceType;
        }

        /// <summary>
        /// 预约类型（0-挂号、1-图文、2-语音、3-视频）
        /// </summary>
        public EnumDoctorServiceType OPDType { get; set; }

        /// <summary>
        /// 排班ID
        /// </summary>
        public string ScheduleID { get; set; }

        public string MemberID { get; set; }
        
        /// <summary>
        /// 咨询内容
        /// </summary>
        [Column(TypeName = "nvarchar")]
        [MaxLength(400)]
        public string ConsultContent { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public virtual List<RequestUserFileDTO> Files { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        public EnumUserType UserType { get; set; }

        /// <summary>
        /// 没有指定排版的时候需要指定医生编号
        /// </summary>
        public string DoctorID { get; set; }
    }
}
