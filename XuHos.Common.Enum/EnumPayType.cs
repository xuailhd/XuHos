using System.ComponentModel;

namespace XuHos.Common.Enum
{
    /// <summary>
    /// 支付类型（state：-1-免支付、0-康美支付、1-微信支付、2-支付宝、3-中国银联、4-MasterCard、5-PayPal、6-VISA,7-HIS,8=余额）
    /// </summary>
    [Description("支付类型")]
    public enum EnumPayType
    {
        /// <summary>
        /// 免支付
        /// </summary>
        [Description("免支付")]
        None = -1,
        /// <summary>
        /// 康美支付
        /// </summary>
        [Description("康美支付")]
        KMPay = 0,
        /// <summary>
        /// 微信支付
        /// </summary>
        [Description("微信支付")]
        WxPay = 1,
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        AliPay = 2,
        /// <summary>
        /// 中国银联
        /// </summary>
        [Description("中国银联")]
        UnionPay = 3,
        /// <summary>
        /// MasterCard
        /// </summary>
        [Description("MasterCard")]
        MasterCard = 4,
        /// <summary>
        /// PayPal
        /// </summary>
        [Description("PayPal")]
        PayPal = 5,
        /// <summary>
        /// VISA
        /// </summary>
        [Description("VISA")]
        VISA = 6,
        /// <summary>
        /// HIS
        /// </summary>
        [Description("HIS")]
        HIS = 7,

        /// <summary>
        /// 账户余额付款
        /// </summary>
        [Description("余额付款")]
        BalancePay = 8,

        /// <summary>
        /// 远程会诊,线下付款
        /// </summary>
        [Description("线下付款")]
        OfflinePay = 9
    }
}
