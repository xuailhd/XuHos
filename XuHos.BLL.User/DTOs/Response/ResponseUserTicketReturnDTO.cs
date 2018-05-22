using XuHos.Common.Enum;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.User.DTOs.Response
{
    public class ResponseUserTicketReturnDTO : ImageBaseDTO
    {
        public string UserID { get; set; }
        public string UserCNName { get; set; }
        public string UserToken { get; set; }
        public XuHos.Common.Enum.EnumUserType UserType { get; set; }
        /// <summary>
        /// 认证状态（0-未认证、1-已通过、2-未通过、3-认证中）
        /// </summary>
        public int CheckState { get; set; }
        public string MemberID { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }
        public EnumUserCardType IDType { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        public EnumUserGender Gender { get; set; }

        string _PhotoUrl;
        public string PhotoUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PhotoUrl))
                {
                    if (UserType == XuHos.Common.Enum.EnumUserType.Doctor)
                    {
                        return PaddingUrlPrefix("images/doctor/default.jpg");
                    }
                    else
                    {
                        return PaddingUrlPrefix("images/doctor/unknow.jpg");
                    }

                }
                else
                {
                    return PaddingUrlPrefix(_PhotoUrl);
                }
            }
            set
            {
                _PhotoUrl = value;
            }
        }

        public int Identifier { get; internal set; }
    }
}
