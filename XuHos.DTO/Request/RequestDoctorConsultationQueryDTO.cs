using XuHos.Common.Enum;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Request
{

    public class RequestDoctorConsultationQueryDTO : RequestSearchCondition
    {
        public string DoctorMemberID { get; set; }
        public string DoctorID { get; set; }
        public string MemberID { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        public EnumUserType UserType { get; set; }

        public EnumOrderState? OrderState { get; set; }
    }
}
