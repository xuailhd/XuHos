using XuHos.Common.Enum;
using XuHos.DTO.Common;
using XuHos.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.DTO;

namespace XuHos.BLL.Doctor.DTOs.Response
{

    /// <summary>
    /// 医生患者分组
    /// </summary>
    public class ResponseDoctorMemberGroupDTO
    {
        public ResponseDoctorMemberGroupDTO()
        {

        }

        /// <summary>
        /// 分组ID
        /// </summary>
        public string MemberGroupID { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }

    }
}
