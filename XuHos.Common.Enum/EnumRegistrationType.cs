using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 预约挂号类型1：普通挂号；2：预约挂号；3：转诊挂号；4：体检系统默认挂号    
    /// </summary>
    public enum EnumRegistrationType
    {
        /// <summary>
        /// 普通
        /// </summary>
        [Description("普通挂号")]
        General = 1,
        /// <summary>
        /// 专家
        /// </summary>
        [Description("预约挂号")]
        Appointment = 2,
        /// <summary>
        /// 转诊挂号
        /// </summary>
        [Description("转诊挂号")]
        Referral = 3,
        /// <summary>
        /// 体检系统默认挂号
        /// </summary>
        [Description("体检系统默认挂号")]
        PhysicalExamination = 4,
    }
}
