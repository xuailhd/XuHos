using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 医生排班
    public class RequestDoctorScheduleRowDTO
    {

        /// <summary>
        /// 开始时段
        /// </summary>
        public string BeginTime { get; set; }

        /// <summary>
        /// 结束时段
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 排班项（列，周一，周二，...）
        /// </summary>
        public List<RequestDoctorScheduleItemDTO> ScheduleItems { get; set; }


    }
}
