using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace XuHos.Common.Enum
{
    /// <summary>
    /// 处方单状态：-1=作废；-2=草稿；3=锁定；1=已提交；2=锁定
    /// </summary>
    public enum EnumEditState
    {
        /// <summary>
        /// 作废
        /// </summary>
        [Description("作废")]
        Rollback = -2,
        /// <summary>
        /// 草稿
        /// </summary>
        [Description("草稿")]
        Draft = -1,
        /// <summary>
        /// 已提交
        /// </summary>
        [Description("已提交")]
        Commit = 1,        
        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        Lock =2
    }
}
