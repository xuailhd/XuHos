using XuHos.Common.Enum;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{

    public class RequestStopDiagnosisDTO
    {

        public RequestStopDiagnosisDTO()
        {

        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 开始时段
        /// </summary>
        public string BeginWorkingTimeBaseID { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 结束时段
        /// </summary>
        public string EndWorkingTimeBaseID { get; set; }

        public string CurrentOperatorDoctorID { get; set; }

    }
}
