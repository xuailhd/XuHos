using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace XuHos.DTO
{

   
    public class RequestDelete : IRequest
    {
        public string ID { get; set; }
    }
}
