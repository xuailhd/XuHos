using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO.Request
{
    public class RequestInterpretationConsultingRoomChangeDTO
    {
        [Required]
        public string MemberID { get; set; }
        [Required]
        public string OriginalDoctorID { get; set; }
        [Required]
        public string OriginalOrderNO { get; set; }
    }
}
