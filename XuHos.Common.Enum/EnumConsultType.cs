using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 咨询类型（0-付费、1-免费、2-义诊、3-套餐、4-会员、5-家庭医生）
    /// </summary>
    [Description("咨询类型")]
    public enum EnumConsultType
    {
        /// <summary>
        /// 付费
        /// </summary>
        [Description("付费")]
        Pay = 0,
        /// <summary>
        /// 免费
        /// </summary>
        [Description("免费")]
        Free = 1,
        /// <summary>
        /// 义诊
        /// </summary>
        [Description("义诊")]
        Clinic = 2,
        /// <summary>
        /// 套餐
        /// </summary>
        [Description("套餐")]
        Package = 3,
        /// <summary>
        /// 会员
        /// </summary>
        [Description("会员")]
        Member = 4,
        /// <summary>
        /// 家庭医生
        /// </summary>
        [Description("家庭医生")]
        FamilyDoctor = 5
    }
}
