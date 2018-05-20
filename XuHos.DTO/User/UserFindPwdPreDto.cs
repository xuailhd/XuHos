/********************************************************************************
** 作者：郭超
** 创始时间：2004-08-13
** 修改人：郭超
** 修改时间：2016-08-13
** 描述：
** web版本找回密码第一步和第二步验证模型
*********************************************************************************/
using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
  public class UserFindPwdPreDto
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string Mobile { get; set; }

        /// <summary>
        ///  验证码
        /// </summary>
        public string VerificationCode { get; set; }

        public EnumUserType UserType { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        [Required]
        public string MsgVerifyCode { get; set; }

        /// <summary>
        /// 短信验证码类型(1:用户注册,2:找回密码)
        /// </summary>
        [Required]
        public int MsgType { get; set; }
    }

    public class UserFindPwdNextDto
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }


    }
}
