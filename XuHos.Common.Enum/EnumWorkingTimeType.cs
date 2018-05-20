using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{

    [Description("工作时段定义")]
    public enum EnumWorkingTimeType
    {
        [Description("时间段 -- 医生排班")]
        DoctorSchedule = 0,

        [Description("时间段 -- 音视频问诊有效性")]
        AudVidValid = 1,

        [Description("时间段 -- 工作时间")]
        Onworking = 2
    }
}
