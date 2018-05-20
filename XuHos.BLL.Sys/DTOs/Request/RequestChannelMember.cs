using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestChannelMemberDTO
    {
        public int Identifier { get; set; }

        public string UserID { get; set; }

        public EnumUserType UserType { get; set; }
    

        /// <summary>
        /// 用户成员编号
        /// </summary>
        public string UserMemberID { get; set; }
        public string UserCNName { get; set; }
        public string UserENName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
