using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.User.DTOs.Response
{


    public class ResponseUserTicketWebReturnDTO
    {
        public string UserID { get; set; }
        public string UserCNName { get; set; }
        public string UserENName { get; set; }
        public string UserToken { get; set; }
        public XuHos.Common.Enum.EnumUserType UserType { get; set; }

        public string PhotoUrl { get; set; }

        public string Mobile { get; set; }
    }
}
