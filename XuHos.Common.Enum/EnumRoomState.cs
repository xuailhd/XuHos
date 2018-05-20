
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 房间状态 -1 其它，0 未就诊，1 候诊中，2 就诊中，3 已就诊，4 呼叫中，5 离开中，6 断开连接，7重新候诊
    /// </summary>
    [Description("房间状态")]
    public enum EnumRoomState
    {
        /// <summary>
        /// 未就诊
        /// </summary>
        [Description("未就诊")]
        NoTreatment = 0,
        /// <summary>
        /// 候诊中
        /// </summary>
        [Description("候诊中")]
        Waiting = 1,
        /// <summary>
        /// 就诊中
        /// </summary>
        [Description("就诊中")]
        InMedicalTreatment = 2,
        /// <summary>
        /// 已就诊
        /// </summary>
        [Description("已就诊")]
        AlreadyVisit = 3,
        /// <summary>
        /// 呼叫中
        /// </summary>
        [Description("呼叫中")]
        Calling = 4,
        ///// <summary>
        ///// 离开中
        ///// </summary>
        //[Description("离开中")]
        //Aborting = 5,
        /// <summary>
        /// 断开连接
        /// </summary>
        [Description("断开连接")]
        Disconnection = 6,

        /// <summary>
        /// 重新候诊（断开连接后重新候诊）
        /// </summary>
        [Description("重新候诊")]
        WaitAgain = 7
    }

           
}
