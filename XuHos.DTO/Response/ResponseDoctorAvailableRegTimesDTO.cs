using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class ResponseDoctorAvailableRegTimesDTO
    {
        public string DoctorId { get; set; }
        public List<DateWeek> DateWeekList { get; set; }

        public List<RegNumOfSpecifiedSchedule> ScheduleList { get; set; }


        public class DateWeek
        {
            public string DateStr { get; set; }
            public string WeekStr { get; set; }
        }

        public class RegNumOfSpecifiedSchedule
        {
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public List<RegDSchedule> RegNumList { get; set; }
        }


        public class RegDSchedule
        {
            //医生排班标识
            public string DoctorScheduleID { get; set; }

            /// <summary>
            /// 预约上限
            /// </summary>
            public int RegSum { get; set; }

            /// <summary>
            /// 已预约的数量
            /// </summary>
            public int RegNum { get; set; }
        }

        public class RegNumOfSchedule
        {
            public string ScheduleId { get; set; }
            public string OPDDate { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }

            public int Quantity { get; set; }

            public int Number { get; set; }  //号源数量
        }
    }

}
