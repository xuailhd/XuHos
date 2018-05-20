using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 关联的业务表
    /// </summary>
    [Description("关联的业务表")]
    public enum EnumExternalTable
    {

        /// <summary>
        /// 医院表
        /// </summary>
        [Description("医院表")]
        Hospitals = 1,

        /// <summary>
        /// 科室表
        /// </summary>
        [Description("科室表")]
        HospitalDepartments = 2,

        /// <summary>
        /// 医生表
        /// </summary>
        [Description("医生表")]
        Doctors = 3
    }
}
