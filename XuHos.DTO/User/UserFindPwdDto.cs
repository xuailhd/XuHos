using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
/********************************************************************************
** 作者：郭超
** 创始时间：2004-08-13
** 修改人：郭超
** 修改时间：2016-08-13
** 描述：
** app找回密码验证模型
*********************************************************************************/
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{

    public class UserFindPwdDto
    {
        public UserFindPwdDto()
        {
            MsgType = 2;
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string Mobile { get; set; }

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

        public EnumUserType UserType { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 机构ID 对接会员系统
        /// </summary>
        public string OrgID { get; set; }

    }

    public class UserFindPwdBATDto
    {

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string Mobile { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

    }
}
