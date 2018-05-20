using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 医生排班模板查询 
    /// </summary>
    public class RequestDoctorScheduleTmpSearchDTO : RequestSearchCondition
    {
        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

    }
}
