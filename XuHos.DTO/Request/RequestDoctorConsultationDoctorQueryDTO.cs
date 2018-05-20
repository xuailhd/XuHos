using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Request
{

    public class RequestDoctorConsultationDoctorQueryDTO : RequestSearchCondition
    {
        public string ConsultationInviteID { get; set; }

        public string ConsultationDoctorID { get; set; }
    }
}
