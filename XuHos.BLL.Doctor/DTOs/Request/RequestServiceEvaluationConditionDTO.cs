using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Request
{

    public class RequestServiceEvaluationConditionDTO : RequestSearchCondition
    {
        public string ProviderID { get; set; }
        public string OuterID { get; set; }
        public int? Score { get; set; }
        public string EvaluationTag { get; set; }
    }
}
