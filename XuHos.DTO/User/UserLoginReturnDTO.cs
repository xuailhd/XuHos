using XuHos.Common.Enum;
using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{

    /// <summary>
    /// 登录后，返回客户端的用户信息
    /// </summary>
    public class UserLoginReturnDTO : Common.ImageBaseDTO
    {
        public string UserID { get; set; }
        /// <summary>
        /// 对应会员系统的 Userid
        /// </summary>
        public string OutUserID { get; set; }

        /// <summary>
        /// 对应会员系统的 BU_LoginAccounts  Outid
        /// </summary>
        public string OutID { get; set; }
        public XuHos.Common.Enum.EnumUserType UserType { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 用户级别(0-普通用户、1-会员、2-黑名单、3-内部医生、4-第三方用户)
        /// </summary>
        public int UserLevel { get; set; }
        /// <summary>
        /// 认证状态（0-未认证、1-已通过、2-未通过、3-认证中）
        /// </summary>
        public int CheckState { get; set; }
        public string Url { get; set; }
        public bool Redirect { get; set; }
        public string UserToken { get; set; }
        public string BJCA_ClientID { get; set; }
        public string UserCNName { get; set; }

        public string UserENName { get; set; }

        public string MemberID { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        public EnumUserGender Gender { get; set; }

        /// <summary>
        /// 是否有远程会诊资质
        /// </summary>
        public bool ConsulAptitude { get; set; }

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

    }
}
