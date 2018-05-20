using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class RequestMsgCodeMobileDto
    {
        public RequestMsgCodeMobileDto()
        {
            userType = EnumUserType.User;
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string Mobile { get; set; }

        [Required]
        public string TemplateID { get; set; }

        public EnumUserType userType { get; set; }

        /// <summary>
        ///  验证码
        /// </summary>
        public string VerifyCode { get; set; } 

        public string OrgID { get; set; }

        /// <summary>
        /// 需要绑定成员的身份证号码
        /// </summary>
        public string IDNumber { get; set; }
    }
}
