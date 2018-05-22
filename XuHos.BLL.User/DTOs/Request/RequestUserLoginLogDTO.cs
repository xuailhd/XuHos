using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.User.DTOs
{
    public class RequestUserLoginLogDTO
    {
        public string TopDrugStoreID { get; set; }

        public string TopPath { get; set; }
        public string HospitalID { get; set; }


        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
