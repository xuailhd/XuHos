using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    public enum EnumUserCashStatus
    {
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        Commit = 0,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("审核通过")]
        ApprovalPass = 1,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提现成功")]
        CashSuccuss = 2,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提现失败")]
        CashFailed = 3,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("审核失败")]
        ApprovalFailed = 4,
    }
}
