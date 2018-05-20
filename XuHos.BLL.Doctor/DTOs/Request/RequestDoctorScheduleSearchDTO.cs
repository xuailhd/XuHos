using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 医生排班查询 
    /// </summary>
    public class RequestDoctorScheduleSearchDTO : RequestSearchCondition
    {

        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        public bool IsCurrentWeek { get; set; }

        public DateTime? Date { get; set; }

        public string NumberSourceID { get; set; }

        public bool IsNullRow { get; set; }

        public int? Status { get; set; }

        public bool RemoveExpire { get; set; }

    }
}
