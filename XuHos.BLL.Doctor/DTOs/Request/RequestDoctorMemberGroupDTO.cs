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
    /// 医生患者分组
    /// </summary>
    public class RequestDoctorMemberGroupDTO
    {

        /// <summary>
        /// 分组ID
        /// </summary>
        public string MemberGroupID { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        [Required]
        public string GroupName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

    }
}
