using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.DTO.Common;
using XuHos.Common.Enum;
using System.ComponentModel.DataAnnotations;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 医生患者分组查询
    /// </summary>
    public class RequestDoctorMemberGroupSearchDTO : RequestSearchCondition
    {

        public string MemberGroupID { get; set; }

        public string GroupName { get; set; }


    }
}
