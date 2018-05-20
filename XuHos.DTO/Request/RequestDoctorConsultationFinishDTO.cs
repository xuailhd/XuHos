using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Request
{

    public class RequestDoctorConsultationFinishDTO
    {
        public string ConsultationID { get; set; }
        public string ConsultationResult { get; set; }
        public bool IsFinish { get; set; }

        public string ConsultationDoctorID { get; set; }
    }
}
