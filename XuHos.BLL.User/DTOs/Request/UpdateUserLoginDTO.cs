using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XuHos.BLL.User.DTOs.Request
{
    public class UpdateUserLoginDTO
    {
        public string UserID { get; set; }

        public string JRegisterID { get; set; }

        public DateTime LastTime { get; set; }
    }
}