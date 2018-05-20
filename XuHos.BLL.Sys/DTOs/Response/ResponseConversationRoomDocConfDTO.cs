using XuHos.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.BLL.Sys.DTOs.Response
{
    public class ResponseConversationRoomDocConfDTO
    {

        /// <summary>
        /// 医生ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 配置类型
        /// </summary>
        public EnumDoctorConfigType ConfigType { get; set; }

        /// <summary>
        /// 配置内容
        /// </summary>
        public string ConfigContent { get; set; }
        
    }

    public class ConversationRoomDocDiagStateDTO
    {
        /// <summary>
        /// 休诊/开诊状态
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
        /// 休诊结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get
            {
                if (!IsDiagnoseOff || !StartTime.HasValue || !Duration.HasValue)
                    return null;

                return StartTime.Value.AddSeconds(Duration.Value);
            }

        }

        /// <summary>
        /// 需要等待时长（单位：秒）
        /// </summary>
        public int WaitingTime
        {
            get
            {
                if (!IsDiagnoseOff || !StartTime.HasValue || !Duration.HasValue)
                    return 0;


                return (int)(EndTime.Value - DateTime.Now).TotalSeconds;
            }
        }
    }
}
