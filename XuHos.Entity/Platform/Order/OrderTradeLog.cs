using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XuHos.Common.Enum;

namespace XuHos.Entity
{
    /// <summary>
    /// 订单交易编号
    /// </summary>

    public partial class OrderTradeLog : AuditableEntity
    {
        public OrderTradeLog()
        {
            this.TradeStatus = EnumTradeState.WAIT_BUYER_PAY;
            this.TradeFee = 0;
            this.TradeNo = "";            
        }

        [MaxLength(32)]
        [Key, Required]
        [Column(TypeName = "varchar")]
        public string TradeLog { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [MaxLength(32)]
        [Required]
        [Column(TypeName = "varchar")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public EnumPayType PayType { get; set; }

        /// <summary>
        /// 交易编号
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(32)]
        public string TradeNo { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime TradeTime { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TradeFee { get; set; }

        /// <summary>
        /// 收款账号
        /// </summary>
        public string SellerID { get; set; }

        /// <summary>
        ///交易状态 WAIT_BUYER_PAY（交易创建，等待买家付款）、TRADE_CLOSED（未付款交易超时关闭，或支付完成后全额退款）、TRADE_SUCCESS（交易支付成功）、TRADE_FINISHED（交易结束，不可退款）
        /// </summary>
        public EnumTradeState TradeStatus { get; set; }

        /// <summary>
        /// 在线交易编号
        /// </summary>
        public string OnlineTransactionNo { get; set; }
    }
}
