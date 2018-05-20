using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{

    public class ResponseDoctorClinicReturnDTO
    {
        /// <summary>
        /// 义诊月份
        /// </summary>
        public string ClinicMonth { get; set; }

        /// <summary>
        /// 义诊日期列表
        /// </summary>
        public string ClinicDates { get; set; }

        /// <summary>
        /// 每天接受咨询次数
        /// </summary>
        public int AcceptCount { get; set; }

        /// <summary>
        /// 当前咨询次数
        /// </summary>
        public int CurrentCount { get; set; }

        /// <summary>
        /// 状态(0-关闭、1-开通)
        /// </summary>
        public bool State { get; set; }
    }
}
