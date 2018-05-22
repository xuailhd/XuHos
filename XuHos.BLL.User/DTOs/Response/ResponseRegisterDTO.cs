using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.User.DTOs.Response
{
    public class ResponseRegisterDTO
    {
        public string UserID { get; set; }

        /// <summary>
        /// 默认新增的memberid
        /// </summary>
        public string MemberID { get; set; }

        public string DoctorID { get; set; }

        public string UserCNName { get; set; }

        public string UserENName { get; set; }

        public XuHos.Common.Enum.EnumUserType UserType { get; set; }

        public string Mobile { get; set; }

        public string UserToken { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// 用户级别(0-普通用户、1-会员、2-黑名单、3-内部医生、4-第三方用户)
        /// </summary>
        public int UserLevel { get; set; }

        /// <summary>
        /// 认证状态（0-未认证、1-已通过、2-未通过、3-认证中）
        /// </summary>
        public int CheckState { get; set; }

        public string BJCA_ClientID { get; set; }
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

        public string PhotoUrl { get; set; }
        public string OrgID { get; set; }
    }
}
