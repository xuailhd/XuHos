
using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 订单日志操作类型
    /// </summary>
    [Description("房间状态")]
    public enum EnumEnumOrderLogOperationType
    {
        /// <summary>
        /// 订单被更新
        /// </summary>
        [Description("订单被更新")]
        UpdateOrder = 9,
        /// <summary>
        /// 订单被创建
        /// </summary>
        [Description("订单被创建")]
        CreateOrder = 0,
        /// <summary>
        /// 订单被取消
        /// </summary>
        [Description("订单被取消")]
        CancelOrder = -1,
        
        /// <summary>
        /// 订单已确认
        /// </summary>
        [Description("订单已确认")]
        ConfirmOrder = 1,
        /// <summary>
        /// 订单已支付
        /// </summary>
        [Description("订单已支付")]
        OrderPayCompleted = 2,
        /// <summary>
        /// 申请退款
        /// </summary>
        [Description("申请退款")]
        RefundApply = -2,
        /// <summary>
        /// 退款已完成
        /// </summary>
        [Description("退款已完成")]
        RefundCompleted = -3,
        /// <summary>
        /// 退款中 
        /// </summary>
        [Description("退款中")]
        Refunding= -4,
        /// <summary>
        /// 物流状态已变更
        /// </summary>
        [Description("物流状态已变更")]
        LogisticStateChanged = 3,
        /// <summary>
        /// 物流配送资料修改
        /// </summary>
        [Description("物流配送资料修改")]
        LogisticInfoChanged = 4,

        /// <summary>
        /// 使用折扣
        /// </summary>
        [Description("使用折扣")]
        UserDiscount =5,

        [Description("订单升级")]
        RenewUpgrade =6,
        /// <summary>
        /// 订单完成
        /// </summary>
        [Description("订单完成")]
        Complete = 999,
    }

           
}
