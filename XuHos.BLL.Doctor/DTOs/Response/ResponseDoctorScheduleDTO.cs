using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace XuHos.BLL.Doctor.DTOs.Response
{
    /// <summary>
    /// 医生排班，放号
    /// </summary>
    public class ResponseDoctorScheduleDTO
    {

        public string NumberSourceID { get; set; }

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

        /// <summary>
        /// 排班表格
        /// </summary>
        public ResponseTableScheduleDTO TableSchedule { get; set; }

    }

    public class ResponseDoctorScheduleItemDTO
    {
        public string ScheduleID { get; set; }

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

        /// <summary>
        /// 号源数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 已预约号源
        /// </summary>
        public int AppointNumber { get; set; }

        /// <summary>
        /// 医生号源详情
        /// </summary>
        public string NumberSourceDetailID { get; set; }

    }

    public class ResponseTableScheduleDTO
    {
        private List<ResponseWeekDateDTO> _colWeekDateDto;
        private List<ResponseRowScheduleDTO> _scheduleDataDto;

        public List<ResponseWeekDateDTO> DateWeekList
        {
            get
            {
                if (_colWeekDateDto == null)
                {
                    _colWeekDateDto = new List<ResponseWeekDateDTO>();
                }
                return _colWeekDateDto;
            }
            set
            {
                _colWeekDateDto = value;
            }
        }
        public List<ResponseRowScheduleDTO> ScheduleList
        {
            get
            {
                if (_scheduleDataDto == null)
                {
                    _scheduleDataDto = new List<ResponseRowScheduleDTO>();
                }
                return _scheduleDataDto;
            }
            set
            {
                _scheduleDataDto = value;
            }
        }

    }

    public class ResponseRowScheduleDTO
    {
        public string BeginTime { get; set; }

        [Obsolete("改用BeginTime")]
        public string StartTime { get; set; } //为兼容以前版本，保留
        public string EndTime { get; set; }
        public string TimePeriod { get; set; }

        private List<ResponseDoctorScheduleDetailDTO> _rowDoctorScheduleDto;
        public List<ResponseDoctorScheduleDetailDTO> DoctorSchedule
        {
            get
            {
                if (_rowDoctorScheduleDto == null)
                {
                    _rowDoctorScheduleDto = new List<ResponseDoctorScheduleDetailDTO>();
                }
                return _rowDoctorScheduleDto;
            }
            set
            {
                _rowDoctorScheduleDto = value;
            }
        }
    }

    public class ResponseWeekDateDTO
    {
        public string Date { get; set; }
        public string Day { get; set; }
        public EnumDayOfWeek Week { get; set; }
    }

    public class ResponseDoctorScheduleDetailDTO
    {
        public string ScheduleID { get; set; }

        public string OPDate { get; set; }
        public string BeginTime { get; set; }

        [Obsolete("改用BeginTime")]
        public string StartTime { get; set; } //为兼容以前版本，保留
        public string EndTime { get; set; }
        public string DoctorID { get; set; }
        public bool Disable { get; set; }
        public bool Checked { get; set; }
        public string DoctorName { get; set; }
        public int Number { get; set; }
        public int AppointNumber { get; set; }
        public EnumDayOfWeek Week { get; set; }
        /// <summary>
        /// 是否停诊
        /// </summary>
        public bool IsStopDiagnosis { get; set; }
        /// <summary>
        /// 已预约人数
        /// </summary>
        public Dictionary<int, int> AppointmentCounts { get; set; }
    }

}
