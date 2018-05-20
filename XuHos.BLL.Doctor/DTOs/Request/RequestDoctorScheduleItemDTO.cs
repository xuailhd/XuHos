using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{
    /// <summary>
    /// 医生排班项(对应表格中的每一格数据)
    /// </summary>
    public class RequestDoctorScheduleItemDTO
    {

        /// <summary>
        /// 周(周一到周日)
        /// </summary>
        public EnumDayOfWeek Week { get; set; }

        /// <summary>
        /// 放号数量
        /// </summary>
        public int Number { get; set; }


    }
}
