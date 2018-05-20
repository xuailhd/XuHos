using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    public enum EnumRegistrationMedicalType
    {
        /// <summary>
        /// 普通
        /// </summary>
        [Description("普通")]
        General = 1,
        /// <summary>
        /// 专家
        /// </summary>
        [Description("专家")]
        Expert = 2,
    }
}
