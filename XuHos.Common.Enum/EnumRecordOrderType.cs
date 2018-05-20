using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    public enum EnumRecordOrderType
    {
        [Description("就诊日期")]
        OPDDate = 0,

        [Description("下单时间")]
        OrderTime = 1,
    }
}