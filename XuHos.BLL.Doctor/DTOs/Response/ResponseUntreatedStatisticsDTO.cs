using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Response
{
    public class ResponseUntreatedStatisticsDTO
    {
        /// <summary>
        /// 音视频问诊候诊数量
        /// </summary>
        public int VideoConsultWaitingCount { set; get; }

        /// <summary>
        /// 图文咨询未回复数量
        /// </summary>
        public int TextConsultUnRepliedCount { set; get; }
    }
}
