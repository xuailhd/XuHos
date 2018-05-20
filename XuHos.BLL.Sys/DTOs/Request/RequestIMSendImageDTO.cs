using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Request
{
    /// <summary>
    /// 发送图片的请求
    /// </summary>
    public class RequestSendImageDTO
    {
        public int ChannelID { get; set; }

        public string FileMD5 { get; set;}

    }
}
