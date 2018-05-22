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
    /// 用户退出
    /// </summary>
    public class RequestUserLogoutDTO : IRequest
    {

        /// <summary>
        /// 客户端名称（包名或域名）
        /// </summary>
        public string ClientName { get; set; }

    }



}
