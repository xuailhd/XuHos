using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    public enum EnumDoctorConfigType
    {
        /// <summary>
        /// 休诊
        /// </summary>
        [Description("休诊")]
        DiagnoseOff = 0,

        /// <summary>
        /// 休诊 -- 开始时间
        /// </summary>
        [Description("休诊 -- 开始时间")]
        DiagnoseOff_TimeStart = 1,

        /// <summary>
        /// 休诊 -- 时长
        /// </summary>
        [Description("休诊 -- 时长")]
        DiagnoseOff_Duration = 2
    }
}
