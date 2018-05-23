using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace XuHos.DTO
{

    public class WeekDateDto
    {
        public string Date { get; set; }
        public string Day { get; set; }
    }

    public class DoctorWorkTime
    {
        public string OPDate { get; set; }

        public List<DoctorScheduleDto> Details { get; set; }
    }

    public class DoctorScheduleDto
    {
        public string ScheduleID { get; set; }

        public DateTime OPDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DoctorID { get; set; }
        public bool Disable { get; set; }
        public bool Checked { get; set; }
        public string DoctorName { get; set; }
        /// <summary>
        /// 是否停诊
        /// </summary>
        public bool IsStopDiagnosis { get; set; }
        /// <summary>
        /// 已预约人数
        /// </summary>
        public Dictionary<int, int> AppointmentCounts { get; set; }

        public int Number { get; set; } //号源数量
    }

    public class DoctorScheduleTmpDto
    {
        public string ScheduleID { get; set; }
        public XuHos.Common.Enum.EnumDoctorServiceType OPDType { get; set; }

        public int AppointmentCount { get; set; }

        public DateTime OPDDate { get; set; }
    }


    public class RowScheduleDto
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        private List<DoctorScheduleDto> _rowDoctorScheduleDto;
        public List<DoctorScheduleDto> DoctorSchedule
        {
            get
            {
                if (_rowDoctorScheduleDto == null)
                {
                    _rowDoctorScheduleDto = new List<DoctorScheduleDto>();
                }
                return _rowDoctorScheduleDto;
            }
            set
            {
                _rowDoctorScheduleDto = value;
            }
        }
    }

    public class TableScheduleDto
    {
        private List<WeekDateDto> _colWeekDateDto;
        private List<RowScheduleDto> _scheduleDataDto;

        public List<WeekDateDto> DateWeekList
        {
            get
            {
                if (_colWeekDateDto == null)
                {
                    _colWeekDateDto = new List<WeekDateDto>();
                }
                return _colWeekDateDto;
            }
            set
            {
                _colWeekDateDto = value;
            }
        }
        public List<RowScheduleDto> ScheduleList
        {
            get
            {
                if (_scheduleDataDto == null)
                {
                    _scheduleDataDto = new List<RowScheduleDto>();
                }
                return _scheduleDataDto;
            }
            set
            {
                _scheduleDataDto = value;
            }
        }

    }
}
