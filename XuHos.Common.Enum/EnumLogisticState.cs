using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace XuHos.Common.Enum
{

    /// <summary>
    /// 物流状态 -2=待审核，-1=审核中,0=审核通过,1=已备货,2=已发货,3=配送中,4=已送达
    /// </summary>
    public enum EnumLogisticState
    {
        [Description("待审核")]
        待审核 = -2,
        [Description("审核中")]
        审核中 = -1,
        [Description("审核失败")]
        审核失败 = -3,
        [Description("审核通过")]
        审核通过 = 0,
        [Description("已备货")]
        已备货 = 1,
        [Description("已发货")]
        已发货 = 2,
        [Description("配送中")]
        配送中 = 3,
        [Description("已送达")]
        已送达 = 4,
        [Description("已取消")]
        已取消 = 99,
    }
}
