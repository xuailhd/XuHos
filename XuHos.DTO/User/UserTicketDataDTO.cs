using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using XuHos.Common.Enum;

namespace XuHos.DTO
{

    public class UserTicketReturnDTO : Common.ImageBaseDTO
    {
        public string UserID { get; set; }
        public string UserCNName { get; set; }
        public string UserENName { get; set; }
        public string UserToken { get; set; }
        public XuHos.Common.Enum.EnumUserType UserType { get; set; }
        /// <summary>
        /// 认证状态（0-未认证、1-已通过、2-未通过、3-认证中）
        /// </summary>
        public int CheckState { get; set; }
        public string BJCA_ClientID { get; set; }
        public string MemberID { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IDNumber { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        public EnumUserGender Gender { get; set; }
        /// <summary>
        /// 是否有远程会诊资质
        /// </summary>
        public bool ConsulAptitude { get; set; }

        /// <summary>
        /// 是否审方医生
        /// </summary>
        public bool RecipeDoctor { get; set; }

        public string OrgID { get; set; }

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
                        return PaddingUrlPrefix("images/doctor/unknow.png");
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
    public class UserTicketWebReturnDTO
    {
        public string UserID { get; set; }
        public string UserCNName { get; set; }
        public string UserENName { get; set; }
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

        /// <summary>
        /// 性别（0-男、1-女、2-未知）
        /// </summary>
        public EnumUserGender Gender { get; set; }

        public string PhotoUrl { get; set; }

        public string Mobile { get; set; }

        public string OrgID { get; set; }
    }
}
