using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 会诊状态(-1未提交、0-待支付、1-待开始、2-进行中、3-已完成)
    /// </summary>
    [Description("会诊状态")]
    public enum EnumConsultationStatus
    {

        /// <summary>
        /// 未提交
        /// </summary>
        [Description("未提交")]
        Uncommitted = -1,
        /// <summary>
        /// 待支付
        /// </summary>
        [Description("待支付")]
        UnPay = 0,
        /// <summary>
        /// 待开始
        /// </summary>
        [Description("待开始")]
        UnStart = 1,
        /// <summary>
        /// 进行中
        /// </summary>
        [Description("进行中")]
        Executing = 2,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Finished=3

    }
}
