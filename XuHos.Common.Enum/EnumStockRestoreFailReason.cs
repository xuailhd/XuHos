using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    [Description("订单退库存操作结果")]
    public enum EnumStockRestoreResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0,
        /// <summary>
        /// 未知
        /// </summary>
        [Description("失败")]
        Fail = 1,

        [Description("拒绝退库存")]
        Reject = 10,

    }
}
