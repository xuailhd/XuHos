using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using XuHos.Common.Enum;

namespace XuHos.DTO
{
    /// <summary>
    /// 用户注册模型
    /// </summary>
    public class RequestUserRegisterDTO:Common.IRequest
    {
        public RequestUserRegisterDTO()
        {
            MsgType = 1;
            UserType =  EnumUserType.User;
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [RegularExpression(@"^\d{11}$")]
        public string Mobile { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        [Required]
        public string MsgVerifyCode { get; set; }

        /// <summary>
        /// 短信类型(1:用户注册,2:找回密码)
        /// </summary>
        [Required]
        public int MsgType{ get; set; }

        /// <summary>
        /// 用户类型(1:普通用户,2:医生用户)
        /// </summary>
        public EnumUserType UserType { get; set; }

        /// <summary>
        /// 注册终端(0-Web、1-安卓、2-IOS)
        /// </summary>
        public string Terminal { get; set; }

        /// <summary>
        /// 来源 机构ID  对应会员系统
        /// </summary>
        public string OrgID { get; set; }
    }
    
}
