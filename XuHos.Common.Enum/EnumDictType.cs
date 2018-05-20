using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 字典类型，在字典分类表直接用枚举名做DictTypeID
    /// 该枚举与表SysDictTypes中数据一一对应
    /// </summary>
    public enum EnumDictType
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        UserType,

        /// <summary>
        /// 用户状态
        /// </summary>
        UserState,

        /// <summary>
        /// 用户级别
        /// </summary>
        UserLevel,

        /// <summary>
        /// 注册终端
        /// </summary>
        Terminal,

        /// <summary>
        /// 币种
        /// </summary>
        Currency,

        /// <summary>
        /// 账户状态
        /// </summary>
        AccountStatus,

        /// <summary>
        /// 用户性别
        /// </summary>
        UserGender,

        /// <summary>
        /// 会诊状态
        /// </summary>
        ConsultationStatus,
        /// <summary>
        /// 学历
        /// </summary>
        Education,

        /// <summary>
        /// 职称
        /// </summary>
        Title
    }
}
