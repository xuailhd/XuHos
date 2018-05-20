using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace XuHos.Common.Enum
{
    /// <summary>
    /// 处方单状态：1=申请；2=申请提交；3=锁定；4=已缴费；5=已发药; 6=已退药; 7=拒绝发药；8=作废处理；9=未发药已退款; 10=已注射; 20=部分退款处理完成
    /// </summary>
    public enum EnumRecipeState
    {
        /// <summary>
        /// 1申请
        /// </summary>
        [Description("待申请收费")]
        待申请收费 = 1,
        /// <summary>
        /// 申请提交
        /// </summary>
        [Description("已申请收费")]
        已申请收费 = 2,
        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        锁定 = 3,
        /// <summary>
        /// 已缴费
        /// </summary>
        [Description("已缴费")]
        已缴费 =4,
        /// <summary>
        /// 已发药
        /// </summary>
        [Description("已发药")]
        已发药 = 5,
        /// <summary>
        /// 已退药
        /// </summary>
        [Description("已退药")]
        已退药 = 6,
        /// <summary>
        /// 拒绝发药
        /// </summary>
        [Description("拒绝发药")]
        拒绝发药 = 7,
        /// <summary>
        /// 作废处理
        /// </summary>
        [Description("作废处理")]
        作废处理 = 8,
        /// <summary>
        /// 未发药已退款
        /// </summary>
        [Description("未发药已退款")]
        未发药已退款 = 9,
        /// <summary>
        /// 未发药已退款
        /// </summary>
        [Description("已注射")]
        已注射 = 10,
        /// <summary>
        /// 部分退款处理完成
        /// </summary>
        [Description("部分退款处理完成")]
        部分退款处理完成 = 20,
    }
}
