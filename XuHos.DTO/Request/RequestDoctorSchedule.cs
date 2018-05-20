using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{

    public class RequestDoctorSchedule<T>
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public string BeginDate { get; set; }

        public T Data { get; set; }

        /// <summary>
        /// 医生排班数据(此参数为app提供)
        /// </summary>
        public string ScheduleData { get; set; }
    }
}
