using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.Common.Enum
{
    /// <summary>
    ///  商品类型：药品,保健食品、食品、医疗器械、化妆品、日用品、农产品、消毒用品、其他
    /// </summary>
    /// 

    public enum EnumGoodType
    {
        /// <summary>
        /// 药品
        /// </summary>
        Drug = 0,
        /// <summary>
        /// 保健食品
        /// </summary>
        HealthFood,
        /// <summary>
        /// 食品
        /// </summary>
        Food,
        /// <summary>
        /// 医疗器械
        /// </summary>
        MedicalEquitment,
        /// <summary>
        /// 化妆品
        /// </summary>
        Cosmetics,
        /// <summary>
        /// 日用品
        /// </summary>
        DailyUse,
        /// <summary>
        /// 农产品
        /// </summary>
        Agriculture,
        /// <summary>
        /// 消毒用品
        /// </summary>
        Disinfectant,
        /// <summary>
        /// 其他
        /// </summary>
        Other,

    }
}
