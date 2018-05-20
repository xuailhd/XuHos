using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    /// <summary>
    /// 音频或视频录制完成的请求
    /// </summary>
    public class RequestSendTextDTO
    {
        public int ChanndlID { get; set; }

        public string Content { get; set;}

    }
}
