using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using XuHos.DTO.Common;

namespace XuHos.DTO
{
   public class RequestPatientEMRSearch : RequestSearchCondition
    {
        public RequestPatientEMRSearch()
        {
            this.MemberID = "";
        }

        [Required]
        public string MemberID
        { get; set; }
    }
}
