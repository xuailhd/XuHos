using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 用户关系 （0-自己、1-配偶、2-父亲、3-母亲、4-儿子、5女儿、6-其他）
    /// </summary>
    [Description("用户关系")]

    public enum EnumUserRelation
    {
        /// <summary>
        /// 自己
        /// </summary>
        [Description("自己")]
        MySelf =0,
        /// <summary>
        /// 配偶
        /// </summary>
        [Description("配偶")]
        Mates= 1,
        /// <summary>
        /// 父亲
        /// </summary>
        [Description("父亲")]
        Father =2,
        /// <summary>
        /// 母亲
        /// </summary>
        [Description("母亲")]
        Monther=3,
        /// <summary>
        /// 儿子
        /// </summary>
        [Description("儿子")]
        Son = 4,
        /// <summary>
        /// 儿子
        /// </summary>
        [Description("女儿")]
        Daughter = 5,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 6
    }
}
