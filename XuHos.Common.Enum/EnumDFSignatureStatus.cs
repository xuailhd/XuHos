using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 家庭医生签约状态：0-待确认，1-已签约，2-申请解约，3-已解约
    /// </summary>
    public enum EnumDFSignatureStatus
    {
        /// <summary>
        /// 待确认
        /// </summary>
        [Description("待确认")]
        UnConfirmed = 0,
        /// <summary>
        /// 已签约
        /// </summary>
        [Description("已签约")]
        Signed = 1,
        /// <summary>
        /// 申请解约
        /// </summary>
        [Description("申请解约")]
        ApplyCancel = 2,
        /// <summary>
        /// 已签约
        /// </summary>
        [Description("已解约")]
        Canceled = 3,
    }
}
