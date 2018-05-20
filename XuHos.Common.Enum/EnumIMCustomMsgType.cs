using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace XuHos.Common.Enum
{
    /// <summary>
    /// 实时通信自定义类型
    /// </summary>
    public enum EnumIMCustomMsgType
    {

        /// <summary>
        /// 诊断问诊小结
        /// </summary>
        [Description("Diagnose.Summary.Submit")]
        Diagnose_Summary_Submit,

        /// <summary>
        /// 休诊/开诊状态改变
        /// </summary>
        [Description("Diagnose.OnOff.StateChanged")]
        Diagnose_OnOff_StateChanged,
        /// <summary>
        /// 处方已保存
        /// </summary>
        [Description("Diagnose.Recipe.Saved")]
        Diagnose_Recipe_Saved,
        /// <summary>
        /// 诊断建议被修改
        /// </summary>
        [Description("Diagnose.Suggest.Saved")]
        Diagnose_Suggest_Saved,

        /// <summary>
        /// 订单-购买处方
        /// </summary>
        [Description("Order.Buy.Recipe")]
        Order_Buy_Recipe,

        /// <summary>
        /// 房间状态被修改
        /// </summary>
        [Description("Room.StateChanged")]
        Room_StateChanged,
        /// <summary>
        /// 房间服务时被修改
        /// </summary>
        [Description("Room.DurationChanged")]
        Room_DurationChanged,
        /// <summary>
        /// 房间到期通知
        /// </summary>
        [Description("Room.Expire")]
        Room_Expire,
        /// <summary>
        /// 准备接通
        /// </summary>
        [Description("Room.ReadyTurnOn")]
        Room_ReadyTurnOn,
        /// <summary>
        /// 房间
        /// </summary>
        [Description("Room.Hangup")]
        Room_Hangup,
        /// <summary>
        /// 候诊队列有变化
        /// </summary>
        [Description("QueueChanged")]
        QueueChanged,
        /// <summary>
        /// 通知
        /// </summary>
        [Description("Notice")]
        Notice,
        /// <summary>
        /// 问卷问题
        /// </summary>
        [Description("Survey.Question")]
        Survey_Question,
        /// <summary>
        /// 一键呼叫问诊长时间未领取提醒
        /// </summary>
        [Description("InquiriesUntaken")]
        InquiriesUntaken,

        /// <summary>
        /// 处方预览
        /// </summary>
        [Description("Recipe.Preview")]
        Recipe_Preview
    }
}
