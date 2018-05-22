using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using XuHos.Common.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.DTO.Common;

namespace XuHos.BLL.User.DTOs
{

    /// <summary>
    /// 用户登陆模型
    /// </summary>
    public class RequestUserLoginDTO : IRequest
    {
        public RequestUserLoginDTO()
        {
            VerifyCode = string.Empty;
        }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户类型(0-管理员、1-患者、2-医生)
        /// </summary>
        [Column(TypeName = "int")]
        public EnumUserType UserType { get; set; }

        /// <summary>
        ///  验证码
        /// </summary>
        public string VerifyCode { get; set; }

        public string OpenID { get; set; }

        public string AppID { get; set; }

        public string UserToken { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public XuHos.Common.Enum.EnumRoleType? UserRole { get; set; }

        public EnumLoginType UserLoginType { get; set; }
    }



}
