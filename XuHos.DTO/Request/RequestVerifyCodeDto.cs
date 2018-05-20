using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{

   

    public  class RequestVerifyCodeDto
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }
    }
}
