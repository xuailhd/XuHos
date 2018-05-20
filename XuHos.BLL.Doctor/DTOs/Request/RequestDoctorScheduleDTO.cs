using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 医生排班
    /// </summary>
    public class RequestDoctorScheduleDTO
    {
        public string NumberSourceID { get; set; }

        public string OutNumberSourceID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 生效开始日期
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 生效结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 状态(0未启用，1启用)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否应用到图文服务
        /// </summary>
        public bool IsUsedInPic { get; set; }

        ///// <summary>
        ///// 时段列表(行，08:00-09:00，09:00-10:00，10:00-11:00，...)
        ///// </summary>
        //public List<RequestDoctorScheduleRowDTO> ScheduleRows { get; set; }


        public List<RequestRowScheduleDTO> ScheduleList { get; set; }


    }

    public class RequestRowScheduleDTO
    {
        public string BeginTime { get; set; }

        [Obsolete("改用BeginTime")]
        public string StartTime { get; set; } //为兼容以前版本，保留
        public string EndTime { get; set; }

        public List<RequestNumberDTO> DoctorSchedule { get; set; }
    }

    public class RequestNumberDTO
    {
        public EnumDayOfWeek Week { get; set; }

        public int Number { get; set; }
       
    }

}
