using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 导诊平台事件分类
    /// </summary>
    [Description("导诊平台事件分类")]    
    public enum EnumGuidanceEventType
    {
        /// <summary>
        /// 修改分诊状态
        /// </summary>
        [Description("修改分诊状态")]
        ChangeTriageStatus = 1,
    }
}
