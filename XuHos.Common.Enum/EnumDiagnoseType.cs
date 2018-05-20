using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace XuHos.Common.Enum
{
    /// <summary>
    ///  诊断类型（1-中医诊断、2-西医诊断）
    /// </summary>
    public enum EnumDiagnoseType
    {
        /// <summary>
        /// 西药
        /// </summary>
        [Description("西药诊断")]
        Western =2,
        /// <summary>
        /// 中药
        /// </summary>
        [Description("中药诊断")]
        Chinese = 1,
    }
}
