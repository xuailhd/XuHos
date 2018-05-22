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
    /// 绑定手机号
    /// </summary>
    public class RequestBingdMobileDTO : IRequest
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        ///  验证码
        /// </summary>
        public string VerifyCode { get; set; }

        public string OpenID { get; set; }

        public string AppID { get; set; }

        public string UserToken { get; set; }

    }
}
