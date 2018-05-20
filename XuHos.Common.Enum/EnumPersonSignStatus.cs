using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 私人医生签约状态：0：未支付，1：生效，2：过期，3：已解约
    /// </summary>
    public enum EnumPersonSignStatus
    {
        /// <summary>
        /// 未支付
        /// </summary>
        [Description("未支付")]
        Unpaid = 0,
        /// <summary>
        /// 生效
        /// </summary>
        [Description("生效")]
        Signed = 1,
        /// <summary>
        /// 过期
        /// </summary>
        [Description("过期")]
        OverDate = 2,
        /// <summary>
        /// 已解约
        /// </summary>
        [Description("已解约")]
        Canceled = 3,
    }


   
}
