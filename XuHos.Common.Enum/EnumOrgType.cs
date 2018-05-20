using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 机构类型(0-医院机构,1-药店机构)
    /// </summary>
    [Description("机构类型")]
    public enum EnumOrgType
    {

        /// <summary>
        /// 医院机构   
        /// </summary>
        [Description("医院机构")]
        Hospital = 0,

        /// <summary>
        /// 药店机构
        /// </summary>
        [Description("药店机构")]
        Drugstore = 1,

        
    }
}
