using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.EventBus.Events
{
    /// <summary>
    /// 医生休诊事件
    /// 沈腾飞
    /// 2017-10-31
    /// </summary>
    public class DoctorDiagnoseOffEvent : BaseEvent, IEvent
    {
        /// <summary>
        /// 医生编号
        /// </summary>
        public string DoctorID { set; get; }
        
        /// <summary>
        /// 是否休诊
        /// </summary>
        public bool IsDiagnoseOff { set; get; }

        /// <summary>
        /// 休诊开始时间
        /// </summary>
        public DateTime? StartTime { set; get; }

        /// <summary>
        /// 休诊时长
        /// </summary>
        public int? Duration { set; get; }

        /// <summary>
        /// 是否发送休诊通知
        /// </summary>
        public bool IsNotify { set; get; }

        /// <summary>
        /// 时钟频率
        /// </summary>
        public int Interval { get; set; }

    }
}
