using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 医生排班模板
    /// </summary>
    public class RequestDoctorScheduleTmpDTO
    {

        public string NumberSourceTmpID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TmpName { get; set; }

        /// <summary>
        /// 时段列表(行，08:00-09:00，09:00-10:00，10:00-11:00，...)
        /// </summary>
        public List<RequestDoctorScheduleRowDTO> ScheduleRows { get; set; }


    }
}
