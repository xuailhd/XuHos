using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuHos.DTO.Common;
using XuHos.Common.Enum;

namespace XuHos.BLL.Common.DTOs.Request.Platform
{
    public class RequestBannerSearchDTO : RequestSearchCondition
    {
        /// <summary>
        /// 终端类型
        /// </summary>
        public EnumBannerTerminal? Terminal { get; set; }

    }
}
