using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.DTO.Common;
using XuHos.Common.Enum;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 医生的患者列表查询条件
    /// </summary>
    public class RequestDoctorMemberSearchDTO : RequestSearchCondition
    {
        /// <summary>
        /// 患者分组ID
        /// </summary>
        public string MemberGroupID { get; set; }

        public string MemberName { get; set; }

        public string CurrentOperatorDoctorID { get; set; }

    }
}
