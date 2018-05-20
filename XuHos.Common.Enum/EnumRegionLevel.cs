using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 区域级别(0-国家,1-省,2-市,3-区/县,4-街道（乡镇）,5-社区，村)
    /// </summary>
    [Description("区域级别")]
    public enum EnumRegionLevel
    {
        /// <summary>
        /// 国家
        /// </summary>
        Country = 0,

        /// <summary>
        /// 省
        /// </summary>
        Province = 1,

        /// <summary>
        /// 市
        /// </summary>
        City = 2,

        /// <summary>
        /// 区 / 县
        /// </summary>
        District = 3,

        /// <summary>
        /// 街道 （乡镇）
        /// </summary>
        Town = 4,

        /// <summary>
        /// 社区，村
        /// </summary>
        Village = 5,



    }
}
