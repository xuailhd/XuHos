using XuHos.DTO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    public class RequestGetMemberInfoDTO:IRequest
    {
        /// <summary>
        /// 通道编号/房间编号
        /// </summary>
        [Required]
        public int ChannelID
        { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public int[] Identifiers { get; set; }
    }
}
