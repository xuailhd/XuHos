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
    /// 家庭成员的病历资料
    /// </summary>
    public class RequestFamilyMemberEMRModifyDTO
    {

        /// <summary>
        /// 患者电子病历ID
        /// </summary>
        public string UserMemberEMRID { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        [Required]
        public string MemberID { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 病历名称
        /// </summary>
        public string EMRName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public virtual List<DTO.UserFileDTO> Files { get; set; }

    }
}
