using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Common.DTOs.Response
{
    /// <summary>
    /// 登录后，缓存在服务器的用户信息
    /// </summary>
    public class UserLoginServerTicketDTO
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }

        public string OpenID { get; set; }

        public EnumUserType UserType { get; set; }
    }

}
