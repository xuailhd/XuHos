using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Common.DTOs.Request
{
    /// <summary>
    /// 统计获取统计信息
    
    /// 日期：2017年7月10日
    /// </summary>
    public class RequestGetWaitStatisticsDTO
    {
        /// <summary>
        /// 机构编号
        /// </summary>
        public string OrgID { get; set; }

        /// <summary>
        /// 医生编号
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 频道编号
        /// </summary>
        public int ChannelID { get; set; }
    }
}
