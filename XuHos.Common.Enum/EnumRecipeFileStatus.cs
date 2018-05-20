using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace XuHos.Common.Enum
{
    /// <summary>
    /// 处方文件状态（-1：提交失败,0:未签名,1:已签名,2:已提交）
    /// </summary>
    public enum EnumRecipeFileStatus
    {
        /// <summary>
        /// 提交失败
        /// </summary>
        [Description("提交失败")]
        Failed = -1,
        /// <summary>
        /// 已提交但尚未签名
        /// </summary>
        [Description("已提交")]
        Submited = 2,
        /// <summary>                 
        /// 已签名
        /// </summary>
        [Description("已签名")]
        Signed = 1,
        /// <summary>
        /// 未签名
        /// </summary>
        [Description("未签名")]
        UnSign = 0,

    }
}
