using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Common.DTOs.Response
{
    /// <summary>
    /// 获取候诊统计
    /// </summary>
    public class ResponseGetWaitStatisticsDTO
    {
        /// <summary>
        /// 预计等待时间
        /// </summary>
        public long ExpectedWaitTime { get; set; }
    }
}
