using XuHos.Common.Enum;
using XuHos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Response
{
    /// <summary>
    /// 当天预约数据
    /// </summary>
    public class ResponseTodayApptCountDTO
    {
        public int PicNoReplyCount { get; set; }
        public int AudCount { get; set; }
        public int VidCount { get; set; }
    }
}
