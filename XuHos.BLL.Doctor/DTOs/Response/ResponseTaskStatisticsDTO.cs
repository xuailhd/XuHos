using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Doctor.DTOs.Response
{
    public class ResponseTaskStatisticsDTO
    {

        /// <summary>
        /// 视频咨询总数量
        /// </summary>
        public long VideoConsultTotalCount { get; set; }

        /// <summary>
        /// 图文咨询总数量
        /// </summary>
        public long TextConsultTotalCount { get; set; }

        /// <summary>
        /// 图文咨询已经领取的数量
        /// </summary>
        public long TextConsultAlreadyCount { get; set; }

        /// <summary>
        /// 语音咨询已经领取数量
        /// </summary>
        public long VideoConsultAlreadyCount { get; set; }

    }
}
