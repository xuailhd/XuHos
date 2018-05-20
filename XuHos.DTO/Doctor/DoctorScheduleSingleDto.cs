using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
  public  class DoctorScheduleSingleDto
    {
        /// <summary>
        /// 排班ID
        /// </summary>
        public string ScheduleID { get; set; }

        // <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public string OPDate { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
    }
}
