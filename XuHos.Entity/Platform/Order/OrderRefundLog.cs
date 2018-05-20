using System;
using System.Collections.Generic;
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

    public partial class OrderRefundLog : AuditableEntity
    {

        [MaxLength(32)]
        [Key, Required]
        [Column(TypeName = "varchar")]
        public string RefundLog { get; set; }

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
        /// 退款单号
        /// </summary>
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(64)]
        public string RefundNo { get; set; }



        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundFee { get; set; }
        /// <summary>
        /// 退款状态
        /// </summary>
        [Required]
        [Column(TypeName = "int")]
        public EnumRefundState RefundState { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime RefundTime { get; set; }
    }
}
