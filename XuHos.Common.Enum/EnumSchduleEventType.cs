using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 医生排班
    /// </summary>
    [Description("医生排班")]
    public enum EnumSchduleEventType
    {
        /// <summary>
        /// 添加排班
        /// </summary>
        [Description("添加")]
        Add = 0,

        /// <summary>
        /// 修改排班
        /// </summary>
        [Description("修改")]
        Modify = 1,

        /// <summary>
        /// 启用，禁用
        /// </summary>
        [Description("启用，禁用")]
        ChangeStatus = 2,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 3

    }
}
